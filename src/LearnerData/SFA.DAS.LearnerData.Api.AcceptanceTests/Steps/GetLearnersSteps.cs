using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Application.GetLearners;
using System.Net;
using System.Net.Http.Headers;
using Reqnroll;
using Reqnroll.Assist;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps;

[Binding]
public class GetLearnersSteps(TestContext testContext, ScenarioContext scenarioContext)
{
    private readonly Fixture _fixture = new Fixture();
    private const string ukprn = "10000001";
    private const int academicYear = 2425;
    private const int defaultPageSize = 20;

    private const string NumberOfLearnersKey = "NumberOfLearners";
    private const string HeadersResponseKey = "HeadersResponse";

    public enum PageNumber
    {
        First = 1,
        Second = 2,
        Fifth = 5
    }

    [Given(@"there are (.*) learners in the system")]
    public void GivenThereAreLearnersInTheSystem(int numberOfLearners)
    {
        scenarioContext.Set(numberOfLearners, NumberOfLearnersKey);
    }

    [Given(@"there are no learners in the system")]
    public void GivenThereAreNoLearnersInTheSystem()
    {
        scenarioContext.Set(0, NumberOfLearnersKey);
    }

    [When(@"I request the (.*) page of learners")]
    public async Task WhenIRequestTheFirstPageOfLearners(PageNumber pageNumber)
    {
        ConfigureInnerApi(pageNumber);
        await CallGetLearnersEndpoint(pageNumber);
    }

    [When(@"I request the (.*) page of learners with a page size of (.*)")]
    public async Task WhenIRequestTheFirstPageOfLearnersWithAPageSizeOf(PageNumber pageNumber, int pageSize)
    {
        ConfigureInnerApi(pageNumber, pageSize);
        await CallGetLearnersEndpoint(pageNumber, pageSize);
    }

    [Then(@"I should receive a response with (.*) learners")]
    public void ThenIShouldReceiveAResponseWithLearners(int numberOfLearners)
    {
        var result = scenarioContext.Get<GetLearnersResponse>();
        result.Should().NotBeNull($"Response {nameof(GetLearnersResponse)} should not be null");
        result.Learners.Should().NotBeNull($"Learners list in {nameof(GetLearnersResponse)} should not be null");
        result.Learners.Should().HaveCount(numberOfLearners, $"Expected {numberOfLearners} learners in the {nameof(GetLearnersResponse)}");
    }

    [Then(@"the response has no headers for next or previous pages")]
    public void ThenTheResponseHasNoHeadersForNextOrPreviousPages()
    {
        ValidateResponseHeaders(false, false);
    }

    [Then(@"the response has headers for the (.*) page only")]
    public void ThenTheResponseHasHeadersForTheNextPageOnly(string pageString)
    {
        switch(pageString)
        {
            case "next":
                ValidateResponseHeaders(false, true);
                break;
            case "previous":
                ValidateResponseHeaders(true, false);
                break;
            default:
                throw new ArgumentException($"Unknown page string: {pageString}");
        }
    }

    [Then(@"the response has headers for both next and previous pages")]
    public void ThenTheResponseHasHeadersForBothNextAndPreviousPages()
    {
        ValidateResponseHeaders(true, true);
    }

    [Then(@"the reponse contains the following information")]
    public void ThenTheReponseContainsTheFollowingInformation(DataTable table)
    {
        var expectedResponse = table.CreateSet<ExpectedResponse>().First();
        var result = scenarioContext.Get<GetLearnersResponse>();
        result.Should().NotBeNull($"Response {nameof(GetLearnersResponse)} should not be null");
        result.Total.Should().Be(expectedResponse.Total, $"Total should be {expectedResponse.Total}");
        result.Page.Should().Be(expectedResponse.Page, $"Page should be {expectedResponse.Page}");
        result.PageSize.Should().Be(expectedResponse.PageSize, $"PageSize should be {expectedResponse.PageSize}");
        result.TotalPages.Should().Be(expectedResponse.TotalPages, $"TotalPages should be {expectedResponse.TotalPages}");
    }

    private async Task CallGetLearnersEndpoint(PageNumber pageNumber, int? pageSize = null)
    {
        var queryString = $"?page={(int)pageNumber}";
        if (pageSize.HasValue)
        {
            queryString += $"&pageSize={pageSize.Value}";
        }
        var response = await testContext.OuterApiClient.GetAsync($"/learners/providers/{ukprn}/academicyears/{academicYear}/learners{queryString}");
        var contentString = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            NUnit.Framework.TestContext.WriteLine($"Outer api GET Learners failed: {response.StatusCode}");
            NUnit.Framework.TestContext.WriteLine($"Response body: {contentString}");
            return;
        }
        var getLearnersResponse = JsonConvert.DeserializeObject<GetLearnersResponse>(contentString);
        scenarioContext.Set(getLearnersResponse);
        scenarioContext.Set(response.Headers, HeadersResponseKey);
    }

    private void ValidateResponseHeaders(bool hasPrevious, bool hasNext)
    {
        var headersResponse = scenarioContext.Get<HttpResponseHeaders>(HeadersResponseKey);
        headersResponse.Should().NotBeNull("Headers response should not be null");
        headersResponse.Contains("links").Should().BeTrue("Headers should contain links");

        var linksHeader = headersResponse.GetValues("links").First();

        if (hasPrevious)
        {
            linksHeader.Should().Contain("rel=\"prev\"", "Links header should contain previous link");
        }
        else
        {
            linksHeader.Should().NotContain("rel=\"prev\"", "Links header should not contain previous link");
        }

        if (hasNext)
        {
            linksHeader.Should().Contain("rel=\"next\"", "Links header should contain next link");
        }
        else
        {
            linksHeader.Should().NotContain("rel=\"next\"", "Links header should not contain next link");
        }
    }

    private void ConfigureInnerApi(PageNumber pageNumber, int? requestedPageSize = null)
    {
        var totalNumberOfLearners = scenarioContext.Get<int>(NumberOfLearnersKey);
        int pageSize = requestedPageSize ?? defaultPageSize;
        var innerApiResponse = GenerateApiResponse((int)pageNumber, totalNumberOfLearners, pageSize);
        testContext.ApprenticeshipsApi.MockServer
        .Given(
            Request
            .Create()
            .WithPath($"/{ukprn}/academicyears/{academicYear}/learnings")
            .WithParam("page", ((int)pageNumber).ToString())
            .WithParam("pageSize", pageSize.ToString())
            .UsingGet())
        .RespondWith(
            Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithBodyAsJson(innerApiResponse)
        );
    }

    private GetLearnersQueryResult GenerateApiResponse(int page, int totalItems, int pageSize)
    {

        var learners = _fixture.CreateMany<Learning>(totalItems).ToList();

        var returnedLearners = learners
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new GetLearnersQueryResult
        {
            Items = returnedLearners,
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize
        };

    } 
}

internal class ExpectedResponse
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}