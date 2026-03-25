using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Application.GetShortCourseEarnings;
using SFA.DAS.LearnerData.Responses.Learning;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using LearningResponse = SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.GetShortCourseLearnersForEarningsResponse;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LearnerData.UnitTests.Application.ShortCourses;

[TestFixture]
public class WhenHandlingGetShortCourseEarningsQuery
{
    private GetShortCourseEarningsQueryHandler _handler;
    private Mock<ILogger<GetShortCourseEarningsQueryHandler>> _logger;
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;

    [SetUp]
    public void SetUp()
    {
        _logger = new Mock<ILogger<GetShortCourseEarningsQueryHandler>>();
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();

        _handler = new GetShortCourseEarningsQueryHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object);
    }

    [Test]
    public async Task Then_LearnerRef_Is_Mapped_From_Episode()
    {
        var learning = BuildLearning(learnerRef: "ABC123");
        SetupLearningApi([learning]);
        SetupEarningsApi(learning.LearningKey, []);

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        result.Learners[0].LearnerRef.Should().Be("ABC123");
    }

    [Test]
    public async Task Then_LearnerRef_Is_Empty_When_No_Episodes()
    {
        var learning = BuildLearning(episodes: []);
        SetupLearningApi([learning]);
        SetupEarningsApi(learning.LearningKey, []);

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        result.Learners[0].LearnerRef.Should().BeEmpty();
    }

    [Test]
    public async Task Then_Empty_Result_Returned_When_No_Learnings()
    {
        _learningApiClient
            .Setup(x => x.Get<GetPagedShortCourseLearnersResponse>(It.IsAny<GetShortCourseLearningsForEarnings>()))
            .ReturnsAsync(new GetPagedShortCourseLearnersResponse { Items = [], TotalItems = 0 });

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        result.Learners.Should().BeEmpty();
        result.Total.Should().Be(0);
        _earningsApiClient.Verify(x => x.GetWithResponseCode<GetFm99ShortCourseDataResponse>(It.IsAny<GetFm99ShortCourseDataRequest>()), Times.Never);
    }

    [Test]
    public async Task Then_Empty_Result_Returned_When_Learning_Api_Returns_Null()
    {
        _learningApiClient
            .Setup(x => x.Get<GetPagedShortCourseLearnersResponse>(It.IsAny<GetShortCourseLearningsForEarnings>()))
            .ReturnsAsync((GetPagedShortCourseLearnersResponse)null);

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        result.Learners.Should().BeEmpty();
        result.Total.Should().Be(0);
    }

    [Test]
    public async Task Then_Exception_Thrown_When_Earnings_Api_Returns_Non_Success()
    {
        var learning = BuildLearning();
        SetupLearningApi([learning]);

        _earningsApiClient
            .Setup(x => x.GetWithResponseCode<GetFm99ShortCourseDataResponse>(It.IsAny<GetFm99ShortCourseDataRequest>()))
            .ReturnsAsync(new ApiResponse<GetFm99ShortCourseDataResponse>(null, System.Net.HttpStatusCode.NotFound, ""));

        var act = () => _handler.Handle(BuildQuery(), CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationException>();
    }

    [Test]
    public async Task Then_LearningKey_Is_Mapped_To_String()
    {
        var learning = BuildLearning();
        SetupLearningApi([learning]);
        SetupEarningsApi(learning.LearningKey, []);

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        result.Learners[0].LearningKey.Should().Be(learning.LearningKey.ToString());
    }

    [Test]
    public async Task Then_Course_Fields_Are_Mapped_Correctly()
    {
        var episode = new LearningResponse.Episode { CourseCode = "91", Price = 1500m, IsApproved = true, LearnerRef = "X1" };
        var learning = BuildLearning(episodes: [episode]);
        SetupLearningApi([learning]);
        SetupEarningsApi(learning.LearningKey, []);

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        var course = result.Learners[0].Courses[0];
        course.CoursePrice.Should().Be(1500m);
        course.Approved.Should().BeTrue();
        course.FundingLineType.Should().Be("GSO Short Courses - Apprenticeship Units - Levy");
        course.AimSequenceNumber.Should().Be(1);
    }

    [Test]
    public async Task Then_Earnings_Are_Mapped_Correctly()
    {
        var learning = BuildLearning();
        SetupLearningApi([learning]);
        var earnings = new List<ShortCourseEarning>
        {
            new() { CollectionYear = 2526, CollectionPeriod = 3, Amount = 750m, Type = "ThirtyPercentLearningComplete" }
        };
        SetupEarningsApi(learning.LearningKey, earnings);

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        var earning = result.Learners[0].Courses[0].Earnings[0];
        earning.CollectionYear.Should().Be(2526);
        earning.CollectionPeriod.Should().Be(3);
        earning.Amount.Should().Be(750m);
        earning.Milestone.Should().Be("ThirtyPercentLearningComplete");
    }

    [Test]
    public async Task Then_Pagination_Fields_Are_Mapped_Correctly()
    {
        var learning = BuildLearning();
        SetupLearningApi([learning], totalItems: 45);
        SetupEarningsApi(learning.LearningKey, []);

        var result = await _handler.Handle(new GetShortCourseEarningsQuery(12345678, 2526, 1, page: 2, pageSize: 20), CancellationToken.None);

        result.Total.Should().Be(45);
        result.Page.Should().Be(2);
        result.PageSize.Should().Be(20);
        result.TotalPages.Should().Be(3);
    }

    private static LearningResponse.Learning BuildLearning(string learnerRef = "REF001", List<LearningResponse.Episode> episodes = null)
    {
        return new LearningResponse.Learning
        {
            LearningKey = Guid.NewGuid(),
            Learner = new LearningResponse.Learner { Uln = "1000000001", FirstName = "Test", LastName = "Learner", DateOfBirth = new DateTime(2000, 1, 1) },
            Episodes = episodes ?? [new LearningResponse.Episode { CourseCode = "91", Price = 1000m, IsApproved = true, LearnerRef = learnerRef }]
        };
    }

    private void SetupLearningApi(List<LearningResponse.Learning> items, int totalItems = -1)
    {
        _learningApiClient
            .Setup(x => x.Get<GetPagedShortCourseLearnersResponse>(It.IsAny<GetShortCourseLearningsForEarnings>()))
            .ReturnsAsync(new GetPagedShortCourseLearnersResponse { Items = items, TotalItems = totalItems < 0 ? items.Count : totalItems });
    }

    private void SetupEarningsApi(Guid learningKey, List<ShortCourseEarning> earnings)
    {
        _earningsApiClient
            .Setup(x => x.GetWithResponseCode<GetFm99ShortCourseDataResponse>(It.IsAny<GetFm99ShortCourseDataRequest>()))
            .ReturnsAsync(new ApiResponse<GetFm99ShortCourseDataResponse>(
                new GetFm99ShortCourseDataResponse { Earnings = earnings },
                System.Net.HttpStatusCode.OK, ""));
    }

    private static GetShortCourseEarningsQuery BuildQuery() =>
        new(12345678, 2526, 1, page: 1, pageSize: 20);
}
