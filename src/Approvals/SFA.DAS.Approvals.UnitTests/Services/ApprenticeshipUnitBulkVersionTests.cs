using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.CoursesApi;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.Services;

namespace SFA.DAS.Approvals.UnitTests.Services;

[TestFixture]
public class ApprenticeshipUnitBulkVersionTests
{
    [Test]
    public async Task MultiSelectStampsEachLearnerUsingTheirOwnActualStartDay()
    {
        var rules = new Mock<ICourseTypeRulesService>();
        var versions = new Mock<IApprenticeshipUnitVersionService>();
        var mapper = new Mock<IMapper>();
        var course = new GetCourseLookupResponse { LearningType = "ApprenticeshipUnit" };
        rules.Setup(x => x.GetCourseTypeRulesAsync("ZSC00001")).ReturnsAsync(new CourseTypeRulesResult
        {
            Course = course, LearnerAgeRules = new GetLearnerAgeResponse()
        });
        mapper.Setup(x => x.Map<BulkUploadAddDraftApprenticeshipExtendedRequest>(It.IsAny<object>()))
            .Returns(() => new BulkUploadAddDraftApprenticeshipExtendedRequest());
        versions.Setup(x => x.Resolve(course, new DateTime(2026, 1, 14)))
            .ReturnsAsync(new GetCourseLookupResponse { Version = "1.0", StandardUId = "AU0001_1.0" });
        versions.Setup(x => x.Resolve(course, new DateTime(2026, 1, 15)))
            .ReturnsAsync(new GetCourseLookupResponse { Version = "1.1", StandardUId = "AU0001_1.1" });
        var service = new AddCourseTypeFromRulesService(rules.Object, versions.Object, mapper.Object,
            Mock.Of<ILogger<AddCourseTypeFromRulesService>>());

        var result = await service.MapAndAddCourseTypeData(new List<BulkUploadAddDraftApprenticeshipRequest>
        {
            new() { CourseCode = "ZSC00001", ActualStartDate = new DateTime(2026, 1, 14) },
            new() { CourseCode = "ZSC00001", ActualStartDate = new DateTime(2026, 1, 15) }
        });

        result[0].TrainingCourseVersion.Should().Be("1.0");
        result[0].StandardUId.Should().Be("AU0001_1.0");
        result[1].TrainingCourseVersion.Should().Be("1.1");
        result[1].StandardUId.Should().Be("AU0001_1.1");
        rules.Verify(x => x.GetCourseTypeRulesAsync("ZSC00001"), Times.Once);
    }
}
