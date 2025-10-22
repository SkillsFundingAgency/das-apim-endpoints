using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Api.AcceptanceTests.Extensions;
using SFA.DAS.LearnerData.Api.AcceptanceTests.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using TechTalk.SpecFlow;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Types;
using WireMock.Util;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps.Fm36;

[Binding]
public class PagingSteps(TestContext testContext, ScenarioContext scenarioContext)
{
    private const int _ukprn = 10005077;
    private const short _academicYear = 2425;
    private const string _deliveryPeriod = "6";
    private readonly Fixture _fixture = new();

    [Given(@"there are (.*) records in the system")]
    public void GivenThereAreRecordsInTheSystem(int numberOfRecords)
    {
        var learnerResponses = new List<Learning>();
        var earningsReponse = new GetFm36DataResponse { Apprenticeships = new List<Apprenticeship>() };

        for (var i = 0; i < numberOfRecords; i++)
        {
            var apprenticeshipModel = GetApprenticeshipModel();
            var apiResponses = apprenticeshipModel.GetInnerApiResponses();

            learnerResponses.Add(apiResponses.UnPagedLearningsInnerApiResponse.Single());
            earningsReponse.Apprenticeships.Add(apiResponses.EarningsInnerApiResponse.Apprenticeships.Single());
        }

        testContext.EarningsApi.MockServer
            .Given(
                Request.Create().WithPath($"/{10005077}/fm36/{_academicYear}/{_deliveryPeriod}")
                    .UsingPost())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBodyAsJson(earningsReponse)
            );

        testContext.ApprenticeshipsApi.MockServer
            .Given(
                Request.Create().WithPath($"/{_ukprn}/{_academicYear}/{_deliveryPeriod}")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                .WithCallback(request => GetPagedResponse(request, learnerResponses))
            );

    }

    [When(@"I call the API with page size (.*) and page number (.*)")]
    public async Task WhenICallTheAPIWithPageSizeAndPageNumber(int pageSize, int pageNumber)
    {
        var response = await testContext.OuterApiClient.GetAsync($"/learners/providers/10005077/collectionPeriod/{_academicYear}/{_deliveryPeriod}/fm36Data?page={pageNumber}&pageSize={pageSize}");
        await scenarioContext.SetFm36Response(response);
    }

    [Then(@"I receive (.*) records")]
    public void ThenIReceiveRecords(int numberOfRecordsRecieved)
    {
        var fm36Response = scenarioContext.GetFm36ResponseBody();
        fm36Response.Items.Count.Should().Be(numberOfRecordsRecieved);
    }

    [Then(@"the following paging metadata is returned")]
    public void ThenTheFollowingPagingMetadataIsReturned(Table table)
    {
        var fm36Response = scenarioContext.GetFm36ResponseBody();

        fm36Response.TotalItems.Should().Be(int.Parse(table.Rows[0]["TotalRecords"]));
        fm36Response.PageSize.Should().Be(int.Parse(table.Rows[0]["PageSize"]));
        fm36Response.Page.Should().Be(int.Parse(table.Rows[0]["PageNumber"]));
        fm36Response.TotalPages.Should().Be(int.Parse(table.Rows[0]["TotalPages"]));

        var hasNext = bool.Parse(table.Rows[0]["HasNextPage"]);
        var hasPrevious = bool.Parse(table.Rows[0]["HasPreviousPage"]);

        var headers = scenarioContext.GetFm36ResponseHeaders();
        var linksHeader = string.Join(",", headers.Single(h => h.Key.Equals("links", StringComparison.OrdinalIgnoreCase)).Value);
        linksHeader.Should().NotBeNull();
        if (hasNext)
        {
            linksHeader.Should().Contain("rel=\"next\"");
        }
        else
        {
            linksHeader.Should().NotContain("rel=\"next\"");
        }
        if (hasPrevious)
        {
            linksHeader.Should().Contain("rel=\"prev\"");
        }
        else
        {
            linksHeader.Should().NotContain("rel=\"prev\"");
        }
    }

    // gets a valid apprenticeship model with price episodes, instalments and additional payments
    // the content of the models is not important for paging tests
    private ApprenticeshipModel GetApprenticeshipModel()
    {
        var priceEpisode = new PriceEpisodeModel
        {
            Key = _fixture.Create<Guid>(),
            PriceEpisodeId = 1,
            StartDate = DateTime.Now.AddMonths(-6),
            EndDate = DateTime.Now.AddMonths(6)
        };

        var instalments = new List<InstalmentModel>();
        for (var i = 0; i < 12; i++)
        {
            var instalment = new InstalmentModel
            {
                PriceEpisodeId = 1,
                AcademicYear = _academicYear,
                DeliveryPeriod = (byte)(i + 1),
                Amount = 600m,
                InstalmentType = "Regular"
            };

            instalments.Add(instalment);
        }

        return new ApprenticeshipModel
        {
            PriceEpisodes = new List<PriceEpisodeModel> { priceEpisode },
            AdditionalPayments = new List<AdditionalPaymentModel>(),
            Instalments = instalments
        };
    }

    private ResponseMessage GetPagedResponse(IRequestMessage request, List<Learning> learnings)
    {
        var query = request.Query;

        var page = int.Parse(query!["page"].Single());
        var pageSize = int.Parse(query!["pageSize"].Single());

        var paged = learnings
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var responseBody = JsonConvert.SerializeObject(new
        {
            Page = page,
            TotalItems = learnings.Count,
            PageSize = pageSize,
            Items = paged
        });

        return new ResponseMessage
        {
            StatusCode = (int)HttpStatusCode.OK,
            BodyData = new BodyData
            {
                DetectedBodyType = BodyType.String,
                BodyAsString = responseBody
            },
            Headers = new Dictionary<string, WireMockList<string>>
                    {
                        { "Content-Type", new WireMockList<string>("application/json") }
                    }
        };
    }
}
