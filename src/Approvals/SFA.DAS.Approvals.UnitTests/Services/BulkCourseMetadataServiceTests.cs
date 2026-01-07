using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;

namespace SFA.DAS.Approvals.UnitTests.Services;

[TestFixture]
public class BulkCourseMetadataServiceTests
{
    [Test, MoqAutoData]
    public async Task GetOtjTrainingHoursForBulkUploadAsync_WhenAllCoursesFound_ReturnsOtjTrainingHours(
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        [Frozen] Mock<ILogger<BulkCourseMetadataService>> logger,
        BulkCourseMetadataService service,
        string courseCode1,
        string courseCode2,
        int otjHours1,
        int otjHours2)
    {
        var courseCodes = new[] { courseCode1, courseCode2 };
        var rplResponse1 = new GetRecognitionOfPriorLearningResponse { OffTheJobTrainingMinimumHours = otjHours1 };
        var rplResponse2 = new GetRecognitionOfPriorLearningResponse { OffTheJobTrainingMinimumHours = otjHours2 };
        var standard = new GetStandardsListItem();

        courseTypeRulesService
            .Setup(x => x.GetRplRulesAsync(courseCode1))
            .ReturnsAsync(new RplRulesResult { Standard = standard, RplRules = rplResponse1 });

        courseTypeRulesService
            .Setup(x => x.GetRplRulesAsync(courseCode2))
            .ReturnsAsync(new RplRulesResult { Standard = standard, RplRules = rplResponse2 });

        var result = await service.GetOtjTrainingHoursForBulkUploadAsync(courseCodes);

        result.Should().HaveCount(2);
        result[courseCode1].Should().Be(otjHours1);
        result[courseCode2].Should().Be(otjHours2);
    }

    [Test, MoqAutoData]
    public async Task GetOtjTrainingHoursForBulkUploadAsync_WhenSomeCoursesFail_ReturnsPartialResults(
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        [Frozen] Mock<ILogger<BulkCourseMetadataService>> logger,
        BulkCourseMetadataService service,
        string courseCode1,
        string courseCode2,
        int otjHours1)
    {
        var courseCodes = new[] { courseCode1, courseCode2 };
        var rplResponse1 = new GetRecognitionOfPriorLearningResponse { OffTheJobTrainingMinimumHours = otjHours1 };
        var standard = new GetStandardsListItem();

        courseTypeRulesService
            .Setup(x => x.GetRplRulesAsync(courseCode1))
            .ReturnsAsync(new RplRulesResult { Standard = standard, RplRules = rplResponse1 });

        courseTypeRulesService
            .Setup(x => x.GetRplRulesAsync(courseCode2))
            .ThrowsAsync(new Exception("API Error"));

        var result = await service.GetOtjTrainingHoursForBulkUploadAsync(courseCodes);

        result.Should().HaveCount(2);
        result[courseCode1].Should().Be(otjHours1);
        result[courseCode2].Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task GetOtjTrainingHoursForBulkUploadAsync_WhenRplRulesIsNull_ReturnsNullOtjHours(
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        [Frozen] Mock<ILogger<BulkCourseMetadataService>> logger,
        BulkCourseMetadataService service,
        string courseCode)
    {
        var courseCodes = new[] { courseCode };
        var standard = new GetStandardsListItem();

        courseTypeRulesService
            .Setup(x => x.GetRplRulesAsync(courseCode))
            .ReturnsAsync(new RplRulesResult { Standard = standard, RplRules = null });

        var result = await service.GetOtjTrainingHoursForBulkUploadAsync(courseCodes);

        result.Should().HaveCount(1);
        result[courseCode].Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task GetOtjTrainingHoursForBulkUploadAsync_WhenDuplicateCourseCodes_ReturnsUniqueResults(
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        [Frozen] Mock<ILogger<BulkCourseMetadataService>> logger,
        BulkCourseMetadataService service,
        string courseCode,
        int otjHours)
    {
        var courseCodes = new[] { courseCode, courseCode, courseCode };
        var rplResponse = new GetRecognitionOfPriorLearningResponse { OffTheJobTrainingMinimumHours = otjHours };
        var standard = new GetStandardsListItem();

        courseTypeRulesService
            .Setup(x => x.GetRplRulesAsync(courseCode))
            .ReturnsAsync(new RplRulesResult { Standard = standard, RplRules = rplResponse });

        var result = await service.GetOtjTrainingHoursForBulkUploadAsync(courseCodes);

        result.Should().HaveCount(1);
        result[courseCode].Should().Be(otjHours);
        courseTypeRulesService.Verify(x => x.GetRplRulesAsync(courseCode), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task GetOtjTrainingHoursForBulkUploadAsync_WhenAllCoursesFail_ReturnsEmptyDictionary(
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        [Frozen] Mock<ILogger<BulkCourseMetadataService>> logger,
        BulkCourseMetadataService service,
        string courseCode1,
        string courseCode2)
    {
        var courseCodes = new[] { courseCode1, courseCode2 };

        courseTypeRulesService
            .Setup(x => x.GetRplRulesAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("API Error"));

        var result = await service.GetOtjTrainingHoursForBulkUploadAsync(courseCodes);

        result.Should().HaveCount(2);
        result[courseCode1].Should().BeNull();
        result[courseCode2].Should().BeNull();
    }
}