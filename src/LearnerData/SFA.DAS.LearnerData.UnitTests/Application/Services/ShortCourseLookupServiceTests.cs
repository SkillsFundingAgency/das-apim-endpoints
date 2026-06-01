using System.Net;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Constants;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

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
            .Setup(x => x.GetWithResponseCode<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(new ApiResponse<CourseLookupDetailResponse>(
                new CourseLookupDetailResponse
                {
                    LearningType = "ApprenticeshipUnit",
                    ApprenticeshipFunding =
                    [
                        new ApprenticeshipFunding { MaxEmployerLevyCap = 6000, EffectiveFrom = new DateTime(2020, 1, 1), EffectiveTo = new DateTime(2022, 12, 31) },
                        new ApprenticeshipFunding { MaxEmployerLevyCap = 9000, EffectiveFrom = new DateTime(2023, 1, 1), EffectiveTo = null }
                    ]
                }, HttpStatusCode.OK, string.Empty));

        var result = await _sut.GetCourseDetails("ZSC00001", new DateTime(2024, 6, 1));

        result.Price.Should().Be(9000);
    }

    [Test]
    public async Task GetCourseDetails_Returns_LearningType_From_Response()
    {
        _coursesApiClient
            .Setup(x => x.GetWithResponseCode<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(new ApiResponse<CourseLookupDetailResponse>(
                new CourseLookupDetailResponse
                {
                    LearningType = "FoundationApprenticeship",
                    ApprenticeshipFunding =
                    [
                        new ApprenticeshipFunding { MaxEmployerLevyCap = 6000, EffectiveFrom = new DateTime(2020, 1, 1), EffectiveTo = null }
                    ]
                }, HttpStatusCode.OK, string.Empty));

        var result = await _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow);

        result.LearningType.Should().Be(LearningType.FoundationApprenticeship);
    }

    [Test]
    public async Task GetCourseDetails_CallsApi_WithCourseCode()
    {
        _coursesApiClient
            .Setup(x => x.GetWithResponseCode<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(new ApiResponse<CourseLookupDetailResponse>(
                new CourseLookupDetailResponse
                {
                    LearningType = "ApprenticeshipUnit",
                    ApprenticeshipFunding =
                    [
                        new ApprenticeshipFunding { MaxEmployerLevyCap = 5000, EffectiveFrom = new DateTime(2020, 1, 1), EffectiveTo = null }
                    ]
                }, HttpStatusCode.OK, string.Empty));

        await _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow);

        _coursesApiClient.Verify(x => x.GetWithResponseCode<CourseLookupDetailResponse>(
            It.Is<IGetApiRequest>(r => r.GetUrl.Contains("ZSC00001"))), Times.Once);
    }

    [Test]
    public async Task GetCourseDetails_WhenApiThrowsNetworkError_ThrowsCoursesApiUnavailableException()
    {
        _coursesApiClient
            .Setup(x => x.GetWithResponseCode<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ThrowsAsync(new HttpRequestException("Service unavailable"));

        await FluentActions.Invoking(() => _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow))
            .Should().ThrowAsync<CoursesApiUnavailableException>()
            .WithMessage("*ZSC00001*");
    }

    [Test]
    public async Task GetCourseDetails_WhenApiReturns404_ThrowsInvalidCourseException()
    {
        _coursesApiClient
            .Setup(x => x.GetWithResponseCode<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(new ApiResponse<CourseLookupDetailResponse>(null, HttpStatusCode.NotFound, string.Empty));

        await FluentActions.Invoking(() => _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow))
            .Should().ThrowAsync<InvalidCourseException>()
            .WithMessage("*ZSC00001*");
    }

    [Test]
    public async Task GetCourseDetails_WhenApiReturns5xx_ThrowsCoursesApiUnavailableExceptionAfterRetries()
    {
        _coursesApiClient
            .Setup(x => x.GetWithResponseCode<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(new ApiResponse<CourseLookupDetailResponse>(null, HttpStatusCode.InternalServerError, string.Empty));

        await FluentActions.Invoking(() => _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow))
            .Should().ThrowAsync<CoursesApiUnavailableException>()
            .WithMessage("*ZSC00001*");

        // 1 initial attempt + 3 retries
        _coursesApiClient.Verify(
            x => x.GetWithResponseCode<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()),
            Times.Exactly(4));
    }

    [Test]
    public async Task GetCourseDetails_WhenNoFundingBandForStartDate_ThrowsInvalidCourseException()
    {
        _coursesApiClient
            .Setup(x => x.GetWithResponseCode<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(new ApiResponse<CourseLookupDetailResponse>(
                new CourseLookupDetailResponse
                {
                    LearningType = "FoundationApprenticeship",
                    ApprenticeshipFunding = []
                }, HttpStatusCode.OK, string.Empty));

        await FluentActions.Invoking(() => _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow))
            .Should().ThrowAsync<InvalidCourseException>()
            .WithMessage("*ZSC00001*");
    }

    [Test]
    public async Task GetCourseDetails_WhenUnrecognisedLearningType_ThrowsInvalidOperationException()
    {
        _coursesApiClient
            .Setup(x => x.GetWithResponseCode<CourseLookupDetailResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(new ApiResponse<CourseLookupDetailResponse>(
                new CourseLookupDetailResponse
                {
                    LearningType = "SomethingUnknown",
                    ApprenticeshipFunding =
                    [
                        new ApprenticeshipFunding { MaxEmployerLevyCap = 5000, EffectiveFrom = new DateTime(2020, 1, 1), EffectiveTo = null }
                    ]
                }, HttpStatusCode.OK, string.Empty));

        await FluentActions.Invoking(() => _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*SomethingUnknown*");
    }
}
