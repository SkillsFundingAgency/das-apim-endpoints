using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class ShortCourseLookupServiceTests
{
    private Mock<ICoursesApiClient<CoursesApiConfiguration>> _coursesApiClient;
    private ShortCourseLookupService _sut;

    [SetUp]
    public void SetUp()
    {
        _coursesApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        _sut = new ShortCourseLookupService(_coursesApiClient.Object, Mock.Of<ILogger<ShortCourseLookupService>>());
    }

    [Test]
    public async Task GetCourseDetails_Returns_Price_From_Funding_Band_Active_On_StartDate()
    {
        _coursesApiClient
            .Setup(x => x.Get<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(new CourseLookupDetailResponse
            {
                LearningType = "ApprenticeshipUnit",
                ApprenticeshipFunding =
                [
                    new ApprenticeshipFunding { MaxEmployerLevyCap = 6000, EffectiveFrom = new DateTime(2020, 1, 1), EffectiveTo = new DateTime(2022, 12, 31) },
                    new ApprenticeshipFunding { MaxEmployerLevyCap = 9000, EffectiveFrom = new DateTime(2023, 1, 1), EffectiveTo = null }
                ]
            });

        var result = await _sut.GetCourseDetails("ZSC00001", new DateTime(2024, 6, 1));

        result.Price.Should().Be(9000);
    }

    [Test]
    public async Task GetCourseDetails_Returns_LearningType_From_Response()
    {
        _coursesApiClient
            .Setup(x => x.Get<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(new CourseLookupDetailResponse
            {
                LearningType = "FoundationApprenticeship",
                ApprenticeshipFunding =
                [
                    new ApprenticeshipFunding { MaxEmployerLevyCap = 6000, EffectiveFrom = new DateTime(2020, 1, 1), EffectiveTo = null }
                ]
            });

        var result = await _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow);

        result.LearningType.Should().Be(LearningType.FoundationApprenticeship);
    }

    [Test]
    public async Task GetCourseDetails_CallsApi_WithCourseCode()
    {
        _coursesApiClient
            .Setup(x => x.Get<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(new CourseLookupDetailResponse
            {
                LearningType = "ApprenticeshipUnit",
                ApprenticeshipFunding =
                [
                    new ApprenticeshipFunding { MaxEmployerLevyCap = 5000, EffectiveFrom = new DateTime(2020, 1, 1), EffectiveTo = null }
                ]
            });

        await _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow);

        _coursesApiClient.Verify(x => x.Get<CourseLookupDetailResponse>(
            It.Is<IGetApiRequest>(r => r.GetUrl.Contains("ZSC00001"))), Times.Once);
    }

    [Test]
    public async Task GetCourseDetails_WhenApiThrows_ReturnsDefaults()
    {
        _coursesApiClient
            .Setup(x => x.Get<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ThrowsAsync(new HttpRequestException("Service unavailable"));

        var result = await _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow);

        result.Price.Should().Be(0);
        result.LearningType.Should().Be(LearningType.ApprenticeshipUnit);
    }

    [Test]
    public async Task GetCourseDetails_WhenApiThrows_LogsError()
    {
        var logger = new Mock<ILogger<ShortCourseLookupService>>();
        _sut = new ShortCourseLookupService(_coursesApiClient.Object, logger.Object);

        _coursesApiClient
            .Setup(x => x.Get<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ThrowsAsync(new HttpRequestException("Service unavailable"));

        await _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow);

        logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public async Task GetCourseDetails_WhenApiReturnsNull_ReturnsDefaults()
    {
        _coursesApiClient
            .Setup(x => x.Get<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync((CourseLookupDetailResponse)null!);

        var result = await _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow);

        result.Price.Should().Be(0);
        result.LearningType.Should().Be(LearningType.ApprenticeshipUnit);
    }

    [Test]
    public async Task GetCourseDetails_WhenNoFundingBands_ReturnsDefaults()
    {
        _coursesApiClient
            .Setup(x => x.Get<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(new CourseLookupDetailResponse
            {
                LearningType = "FoundationApprenticeship",
                ApprenticeshipFunding = []
            });

        var result = await _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow);

        result.Price.Should().Be(0);
        result.LearningType.Should().Be(LearningType.ApprenticeshipUnit);
    }

    [Test]
    public async Task GetCourseDetails_WhenUnrecognisedLearningType_DefaultsToApprenticeshipUnit()
    {
        _coursesApiClient
            .Setup(x => x.Get<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(new CourseLookupDetailResponse
            {
                LearningType = "SomethingUnknown",
                ApprenticeshipFunding =
                [
                    new ApprenticeshipFunding { MaxEmployerLevyCap = 5000, EffectiveFrom = new DateTime(2020, 1, 1), EffectiveTo = null }
                ]
            });

        var result = await _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow);

        result.LearningType.Should().Be(LearningType.ApprenticeshipUnit);
        result.Price.Should().Be(5000);
    }
}
