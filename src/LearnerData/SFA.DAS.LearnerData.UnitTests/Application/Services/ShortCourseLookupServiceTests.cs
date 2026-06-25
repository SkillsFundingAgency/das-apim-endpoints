using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.SharedOuterApi.Types.Constants;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class ShortCourseLookupServiceTests
{
    private Mock<ICourseService> _courseService;
    private ShortCourseLookupService _sut;

    [SetUp]
    public void SetUp()
    {
        _courseService = new Mock<ICourseService>();
        _sut = new ShortCourseLookupService(_courseService.Object, Mock.Of<ILogger<ShortCourseLookupService>>());
    }

    [Test]
    public async Task GetCourseDetails_Returns_Price_From_Funding_Band_Active_On_StartDate()
    {
        _courseService
            .Setup(x => x.GetCourseLookupDetailsById(It.IsAny<string>()))
            .ReturnsAsync(
                new CourseLookupDetailResponse
                {
                    LearningType = "ApprenticeshipUnit",
                    ApprenticeshipFunding =
                    [
                        new ApprenticeshipFunding
                        {
                            MaxEmployerLevyCap = 6000, EffectiveFrom = new DateTime(2020, 1, 1),
                            EffectiveTo = new DateTime(2022, 12, 31)
                        },
                        new ApprenticeshipFunding { MaxEmployerLevyCap = 9000, EffectiveFrom = new DateTime(2023, 1, 1), EffectiveTo = null }
                    ]
                });

        var result = await _sut.GetCourseDetails("ZSC00001", new DateTime(2024, 6, 1));

        result.Price.Should().Be(9000);
    }

    [Test]
    public async Task GetCourseDetails_Returns_LearningType_From_Response()
    {
        _courseService
            .Setup(x => x.GetCourseLookupDetailsById(It.IsAny<string>()))
            .ReturnsAsync(
                new CourseLookupDetailResponse
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
    public async Task GetCourseDetails_CallsService_WithCourseCode()
    {
        _courseService
            .Setup(x => x.GetCourseLookupDetailsById(It.IsAny<string>()))
            .ReturnsAsync(
                new CourseLookupDetailResponse
                {
                    LearningType = "ApprenticeshipUnit",
                    ApprenticeshipFunding =
                    [
                        new ApprenticeshipFunding { MaxEmployerLevyCap = 5000, EffectiveFrom = new DateTime(2020, 1, 1), EffectiveTo = null }
                    ]
                });

        await _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow);

        _courseService.Verify(x => x.GetCourseLookupDetailsById("ZSC00001"), Times.Once);
    }

    [Test]
    public async Task GetCourseDetails_WhenServiceReturnsNull_ThrowsInvalidOperationException()
    {
        _courseService
            .Setup(x => x.GetCourseLookupDetailsById(It.IsAny<string>()))
            .ReturnsAsync((CourseLookupDetailResponse)null);

        await FluentActions.Invoking(() => _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*ZSC00001*");
    }

    [Test]
    public async Task GetCourseDetails_WhenNoFundingBandForStartDate_ThrowsInvalidCourseException()
    {
        _courseService
            .Setup(x => x.GetCourseLookupDetailsById(It.IsAny<string>()))
            .ReturnsAsync(
                new CourseLookupDetailResponse
                {
                    LearningType = "FoundationApprenticeship",
                    ApprenticeshipFunding = []
                });

        await FluentActions.Invoking(() => _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow))
            .Should().ThrowAsync<InvalidCourseException>()
            .WithMessage("*ZSC00001*");
    }

    [Test]
    public async Task GetCourseDetails_WhenUnrecognisedLearningType_ThrowsInvalidOperationException()
    {
        _courseService
            .Setup(x => x.GetCourseLookupDetailsById(It.IsAny<string>()))
            .ReturnsAsync(
                new CourseLookupDetailResponse
                {
                    LearningType = "SomethingUnknown",
                    ApprenticeshipFunding =
                    [
                        new ApprenticeshipFunding { MaxEmployerLevyCap = 5000, EffectiveFrom = new DateTime(2020, 1, 1), EffectiveTo = null }
                    ]
                });

        await FluentActions.Invoking(() => _sut.GetCourseDetails("ZSC00001", DateTime.UtcNow))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*SomethingUnknown*");
    }
}
