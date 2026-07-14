using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LearnerData.Application.GetShortCourseEarnings;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using GetShortCourseLearningsForEarnings = SFA.DAS.LearnerData.Requests.LearningInner.GetShortCourseLearningsForEarnings;
using LearningResponse = SFA.DAS.LearnerData.Responses.LearningInner.GetShortCourseLearnersForEarningsResponse;

namespace SFA.DAS.LearnerData.UnitTests.Application.ShortCourses;

[TestFixture]
public class WhenHandlingGetShortCourseEarningsQuery
{
    private GetShortCourseEarningsQueryHandler _handler;
    private Mock<ILogger<GetShortCourseEarningsQueryHandler>> _mockLogger;
    private Mock<ILearningApiClient<LearningApiConfiguration>> _mockLearningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _mockEarningsApiClient;
    private Mock<ILearnerDataCacheService> _mockDistributedCache;

    [SetUp]
    public void SetUp()
    {
        _mockLogger = new Mock<ILogger<GetShortCourseEarningsQueryHandler>>();
        _mockLearningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _mockEarningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        _mockDistributedCache = new Mock<ILearnerDataCacheService>();

        _handler = new GetShortCourseEarningsQueryHandler(
            _mockLogger.Object,
            _mockLearningApiClient.Object,
            _mockEarningsApiClient.Object,
            _mockDistributedCache.Object);
    }

    [Test]
    public async Task Then_LearnerRef_Is_Mapped_From_Episode()
    {
        var learning = BuildLearning(learnerRef: "ABC123");
        SetupLearningApi([learning]);
        SetupEarningsApi(learning.LearningKey, []);
        SetupCacheService([learning]);

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        result.Learners[0].LearnerRef.Should().Be("ABC123");
    }

    [Test]
    public async Task Then_LearnerRef_Is_Empty_When_No_Episodes()
    {
        var learning = BuildLearning(episodes: []);
        SetupLearningApi([learning]);
        SetupEarningsApi(learning.LearningKey, []);
        SetupCacheService([learning]);

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        result.Learners[0].LearnerRef.Should().BeEmpty();
    }

    [Test]
    public async Task Then_Empty_Result_Returned_When_No_Learnings()
    {
        _mockLearningApiClient
            .Setup(x => x.Get<GetPagedShortCourseLearnersResponse>(It.IsAny<GetShortCourseLearningsForEarnings>()))
            .ReturnsAsync(new GetPagedShortCourseLearnersResponse { Items = [], TotalItems = 0 });

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        result.Learners.Should().BeEmpty();
        result.Total.Should().Be(0);
        _mockEarningsApiClient.Verify(x => x.GetWithResponseCode<GetFm99ShortCourseDataResponse>(It.IsAny<GetFm99ShortCourseDataRequest>()), Times.Never);
    }

    [Test]
    public async Task Then_Empty_Result_Returned_When_Learning_Api_Returns_Null()
    {
        _mockLearningApiClient
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
        SetupCacheService([learning]);

        _mockEarningsApiClient
            .Setup(x => x.GetWithResponseCode<GetFm99ShortCourseDataResponse>(It.IsAny<GetFm99ShortCourseDataRequest>()))
            .ReturnsAsync(new ApiResponse<GetFm99ShortCourseDataResponse>(null, System.Net.HttpStatusCode.NotFound, ""));

        var act = () => _handler.Handle(BuildQuery(), CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationException>();
    }

    [Test]
    public async Task Then_LearnerKey_Is_Mapped_To_Key_And_LearningKey()
    {
        var learning = BuildLearning();
        SetupLearningApi([learning]);
        SetupEarningsApi(learning.LearningKey, []);
        SetupCacheService([learning]);

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        result.Learners[0].Key.Should().Be(learning.LearnerKey.ToString());
        result.Learners[0].LearningKey.Should().Be(learning.LearnerKey.ToString());
    }

    [Test]
    public async Task Then_Course_Fields_Are_Mapped_Correctly()
    {
        var episode = new LearningResponse.Episode { CourseCode = "91", Price = 1500m, IsApproved = true, LearnerRef = "X1" };
        var learning = BuildLearning(episodes: [episode]);
        SetupLearningApi([learning]);
        SetupEarningsApi(learning.LearningKey, []);
        SetupCacheService([learning]);

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        var course = result.Learners[0].Courses[0];
        course.CoursePrice.Should().Be(1500m);
        course.Approved.Should().BeTrue();
        course.AimSequenceNumber.Should().Be(1);
    }

    [Test]
    public async Task Then_AimSequenceNumber_Is_Mapped_By_CourseCode()
    {
        var episodes = new List<LearningResponse.Episode>
        {
            new() { CourseCode = "91", Price = 1500m, IsApproved = true, LearnerRef = "X1" },
            new() { CourseCode = "92", Price = 1200m, IsApproved = false, LearnerRef = "X1" }
        };

        var learning = BuildLearning(episodes: episodes);
        SetupLearningApi([learning]);
        SetupEarningsApi(learning.LearningKey, []);
        SetupCacheService([learning], new Dictionary<string, int>
        {
            ["91"] = 7,
            ["92"] = 3
        });

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        var coursesByPrice = result.Learners[0].Courses.ToDictionary(x => x.CoursePrice);
        coursesByPrice[1500m].AimSequenceNumber.Should().Be(7);
        coursesByPrice[1200m].AimSequenceNumber.Should().Be(3);
    }

    [TestCase(EmployerType.Levy, "GSO Short Courses (Apprenticeship Units) Levy")]
    [TestCase(EmployerType.NonLevy, "GSO Short Courses (Apprenticeship Units) Non-Levy")]
    public async Task Then_FundingLineType_Is_Derived_From_EmployerType(EmployerType employerType, string expectedFundingLineType)
    {
        var episode = new LearningResponse.Episode { CourseCode = "91", Price = 1000m, IsApproved = true, LearnerRef = "X1", EmployerType = employerType };
        var learning = BuildLearning(episodes: [episode]);
        SetupLearningApi([learning]);
        SetupEarningsApi(learning.LearningKey, []);
        SetupCacheService([learning]);

        var result = await _handler.Handle(BuildQuery(), CancellationToken.None);

        result.Learners[0].Courses[0].FundingLineType.Should().Be(expectedFundingLineType);
    }

    [Test]
    public async Task Then_Earnings_Are_Mapped_Correctly()
    {
        var learning = BuildLearning();
        SetupLearningApi([learning]);
        SetupCacheService([learning]);
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
        SetupCacheService([learning]);

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
            LearnerKey = Guid.NewGuid(),
            Learner = new LearningResponse.Learner { Uln = "1000000001", FirstName = "Test", LastName = "Learner", DateOfBirth = new DateTime(2000, 1, 1) },
            Episodes = episodes ?? [new LearningResponse.Episode { CourseCode = "91", Price = 1000m, IsApproved = true, LearnerRef = learnerRef }]
        };
    }

    private void SetupLearningApi(List<LearningResponse.Learning> items, int totalItems = -1)
    {
        _mockLearningApiClient
            .Setup(x => x.Get<GetPagedShortCourseLearnersResponse>(It.IsAny<GetShortCourseLearningsForEarnings>()))
            .ReturnsAsync(new GetPagedShortCourseLearnersResponse { Items = items, TotalItems = totalItems < 0 ? items.Count : totalItems });
    }

    private void SetupEarningsApi(Guid learningKey, List<ShortCourseEarning> earnings)
    {
        _mockEarningsApiClient
            .Setup(x => x.GetWithResponseCode<GetFm99ShortCourseDataResponse>(It.IsAny<GetFm99ShortCourseDataRequest>()))
            .ReturnsAsync(new ApiResponse<GetFm99ShortCourseDataResponse>(
                new GetFm99ShortCourseDataResponse { Earnings = earnings },
                System.Net.HttpStatusCode.OK, ""));
    }

    private void SetupCacheService(List<LearningResponse.Learning> learningInnerLearners, Dictionary<string, int> aimSequenceNumberByCourseCode = null)
    {
        var aimNumber = 1;

        var cachedSldLearners = learningInnerLearners.Select(l => new ShortCourseRequest
        {
            Learner = new ShortCourseLearnerRequestDetails
            {
                Uln = long.Parse(l.Learner.Uln)
            },
            Delivery = new ShortCourseDelivery{
                OnProgramme = l.Episodes.Select(e => new ShortCourseOnProgramme
                {
                    CourseCode = e.CourseCode,
                    AimSequenceNumber = aimSequenceNumberByCourseCode != null &&
                                        aimSequenceNumberByCourseCode.TryGetValue(e.CourseCode, out var aimSequenceNumber)
                        ? aimSequenceNumber
                        : aimNumber++,
                }).ToList()
            }
        }).ToList();

        _mockDistributedCache.Setup(x=>
            x.GetLearners<ShortCourseRequest>(It.IsAny<long>(), It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedSldLearners);
    }

    private static GetShortCourseEarningsQuery BuildQuery() =>
        new(12345678, 2526, 1, page: 1, pageSize: 20);
}
