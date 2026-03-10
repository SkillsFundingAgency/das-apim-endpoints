using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.LearnerData.Api.AcceptanceTests.Models;
using SFA.DAS.LearnerData.Application.GetShortCourseEarnings;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.GetShortCourseLearnersForEarningsResponse;
using System.Net;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using LearningEpisode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.GetShortCourseLearnersForEarningsResponse.Episode;
using InnerEarningRecord = SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.ShortCourseEarning;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Steps;

[Binding]
public class ShortCourseSteps
{
    private readonly TestContext _testContext;
    private readonly ScenarioContext _scenarioContext;
    private readonly Fixture _fixture;
    private const string GetShortCourseResponseKey = "GetShortCourseResponse";

    public ShortCourseSteps(TestContext testContext, ScenarioContext scenarioContext)
    {
        _testContext = testContext;
        _scenarioContext = scenarioContext;
        _fixture = new Fixture();
    }

    [Given(@"there are (.*) short course learning records for ukprn (.*) in academic year (.*) period (.*)")]
    public void GivenThereAreShortCourseLearningRecordsForUkprnInAcademicYearPeriod(
        int totalCourses, string ukprn, int academicYear, byte period)
    {
        var learnings = _fixture.CreateMany<Learning>(totalCourses).ToList();
        var earnings = _fixture.CreateMany<GetShortCourseDataResponse>(totalCourses).ToList();

        _scenarioContext.Set(new ShortCourseTestData(ukprn, learnings, earnings));
    }

    [When(@"the get short course earnings endpoint is called for ukprn (.*) in academic year (.*) period (.*) for page (.*) with page size (.*)")]
    public async Task WhenTheGetShortCourseEarningsEndpointIsCalledForUkprnInAcademicYearPeriodForPageWithPageSize(
        string ukprn, int academicYear, byte period, int pageNumber, int pageSize)
    {
        var testData = _scenarioContext.Get<ShortCourseTestData>();
        MockInnerApiResponses(testData, academicYear, period, pageNumber, pageSize);

        var response = await _testContext.OuterApiClient.GetAsync($"/providers/{testData.Ukprn}/collectionPeriods/{academicYear}/{period}/shortCourses?page={pageNumber}&pagesize={pageSize}");
        var contentString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            NUnit.Framework.TestContext.WriteLine($"Outer api GET short course earnings failed: {response.StatusCode}");
            NUnit.Framework.TestContext.WriteLine($"Response body: {contentString}");
        }

        var log = _testContext.ApprenticeshipsApi.MockServer.LogEntries;
        var last = log.LastOrDefault();

        var shortCourseEarningsResponse = JsonConvert.DeserializeObject<GetShortCourseEarningsResult>(contentString);
        _scenarioContext.Set(shortCourseEarningsResponse, GetShortCourseResponseKey);
    }

    [Then(@"the short course response should contain the following pagination details:")]
    public void ThenTheResponseShouldContainTheFollowingPaginationDetails(Table table)
    {
        var paginationExpectations = table.CreateSet<ShortCoursePaginationExpectations>().Single();
        var response = _scenarioContext.Get<GetShortCourseEarningsResult>(GetShortCourseResponseKey);
        response.TotalItems.Should().Be(paginationExpectations.TotalRecords, "Expected TotalItems to match");
        response.Page.Should().Be(paginationExpectations.PageNumber, "Expected Page to match");
        response.PageSize.Should().Be(paginationExpectations.PageSize, "Expected PageSize to match");
        response.Items.Count.Should().Be(paginationExpectations.NumberOfRecordsInPage, "Expected NumberOfRecordsInPage to match");
    }

    [Given(@"for ukprn (.*) there are short course learning with")]
    public void GivenThereIsAShortCourseLearningWith(string ukprn, Table table)
    {
        var expectedLearnings = table.CreateSet<ShortCourseDetailsExpectations>();
        
        var testData = new ShortCourseTestData(ukprn);
        
        foreach(var expectedLearning in expectedLearnings)
        {
            var learning = _fixture.Create<Learning>();
            learning.Episodes = new List<LearningEpisode>
            {
                new LearningEpisode
                {
                    CourseCode = _fixture.Create<string>(),
                    IsApproved = expectedLearning.IsApproved,
                    Price = expectedLearning.Price,
                }
            };
            testData.ShortCourseLearnings.Add(learning);
        }

        _scenarioContext.Set(testData);
    }

    [Given(@"for ukprn (.*) there are short course earnings with")]
    public void GivenThereIsAShortCourseEarningWith(string ukprn, Table table)
    {
        var expectedEarnings = table.CreateSet<ShortCourseDetailsExpectations>().ToList();
        var testData = _scenarioContext.Get<ShortCourseTestData>();

        for(var i = 0; i< expectedEarnings.Count(); i++)
        {
            var response = new GetShortCourseDataResponse { Earnings = new List<InnerEarningRecord>() };

            response.Earnings.Add(new InnerEarningRecord
            {
                CollectionYear = expectedEarnings[i].CollectionYear1,
                CollectionPeriod = expectedEarnings[i].Period1,
                Amount = expectedEarnings[i].Amount1,
                Type = expectedEarnings[i].Type1
            });

            response.Earnings.Add(new InnerEarningRecord
            {
                CollectionYear = expectedEarnings[i].CollectionYear2,
                CollectionPeriod = expectedEarnings[i].Period2,
                Amount = expectedEarnings[i].Amount2,
                Type = expectedEarnings[i].Type2
            });

            testData.ShortCourseEarnings.Add(testData.ShortCourseLearnings.ElementAt(i).LearningKey, response);
        }
    }

    [Then(@"the short course response should contain a record with the following details:")]
    public void ThenTheShortCourseResponseShouldContainARecordWithTheFollowingDetails(Table table)
    {
        var expectedEarnings = table.CreateSet<ShortCourseDetailsExpectations>().ToList();
        var response = _scenarioContext.Get<GetShortCourseEarningsResult>(GetShortCourseResponseKey);

        foreach (var expected in expectedEarnings)
        {
            var record = response.Items.SingleOrDefault(r => r.Courses.Any(c => c.Approved == expected.IsApproved && c.CoursePrice == expected.Price));

            record.Should().NotBeNull($"Expected to find a record with IsApproved {expected.IsApproved} and Price {expected.Price} but did not.");
            
            var earnings = record.Courses.SelectMany(c => c.Earnings).ToList();

            earnings.Should().Contain(e => e.Amount == expected.Amount1 && e.CollectionPeriod == expected.Period1 && e.CollectionYear == expected.CollectionYear1 && e.Milestone == expected.Type1,
                $"Expected to find an earning with Amount {expected.Amount1}, CollectionPeriod {expected.Period1}, CollectionYear {expected.CollectionYear1} and Type {expected.Type1} but did not.");
            earnings.Should().Contain(e => e.Amount == expected.Amount2 && e.CollectionPeriod == expected.Period2 && e.CollectionYear == expected.CollectionYear2 && e.Milestone == expected.Type2,
                $"Expected to find an earning with Amount {expected.Amount2}, CollectionPeriod {expected.Period2}, CollectionYear {expected.CollectionYear2} and Type {expected.Type2} but did not.");
        }
    }

    private void MockInnerApiResponses(ShortCourseTestData testData, int academicYear, byte period, int pageNumber, int pageSize)
    {
        var slicedLearnings = testData.ShortCourseLearnings.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        var learningResponse = new GetPagedLearnersFromLearningInner
        {
            Page = pageNumber,
            PageSize = pageSize,
            TotalItems = testData.ShortCourseLearnings.Count,
            Items = slicedLearnings
        };

        _testContext.ApprenticeshipsApi.MockServer
            .Given(
                Request.Create().WithPath($"/{testData.Ukprn}/{academicYear}/shortCourses")
                    .WithParam("page", $"{pageNumber}")
                    .WithParam("pageSize", $"{pageSize}")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBodyAsJson(learningResponse)
            );

        foreach (var learning in slicedLearnings)
        {
            var earning = testData.ShortCourseEarnings[learning.LearningKey];

            _testContext.EarningsApi.MockServer
                .Given(
                    Request.Create().WithPath($"/{learning.LearningKey}/shortCourses")
                        .WithParam("ukprn", testData.Ukprn)
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithBodyAsJson(earning)
                );
        }

    }
}
