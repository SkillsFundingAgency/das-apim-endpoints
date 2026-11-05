using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.CoursesApi;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.Approvals.UnitTests.Services;

[TestFixture]
public class ApprenticeshipUnitBulkVersionTests
{
    [TestCase("ZSC00001")]
    [TestCase("ZSC00002")]
    public async Task MultiSelectFetchesVersionsOncePerReferenceAndUsesEachLearnersActualStartDay(string otherCourseCode)
    {
        var rules = new Mock<ICourseTypeRulesService>();
        var courses = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        var mapper = new Mock<IMapper>();
        rules.Setup(x => x.GetCourseTypeRulesAsync(It.IsAny<string>())).ReturnsAsync(new CourseTypeRulesResult
        {
            Course = new GetCourseLookupResponse { LearningType = "ApprenticeshipUnit", IfateReferenceNumber = "AU0001" },
            LearnerAgeRules = new GetLearnerAgeResponse()
        });
        mapper.Setup(x => x.Map<BulkUploadAddDraftApprenticeshipExtendedRequest>(It.IsAny<object>()))
            .Returns(() => new BulkUploadAddDraftApprenticeshipExtendedRequest());
        courses.Setup(x => x.Get<GetCourseVersionsResponse>(It.IsAny<GetCourseVersionsRequest>()))
            .ReturnsAsync(new GetCourseVersionsResponse
            {
                Courses =
                [
                    new()
                    {
                        Version = "1.0", VersionMajor = 1, StandardUId = "AU0001_1.0",
                        VersionDetail = new StandardVersionDetail
                        {
                            EarliestStartDate = new DateTime(2026, 1, 1), LatestStartDate = new DateTime(2026, 1, 14)
                        }
                    },
                    new()
                    {
                        Version = "1.1", VersionMajor = 1, VersionMinor = 1, StandardUId = "AU0001_1.1",
                        VersionDetail = new StandardVersionDetail { EarliestStartDate = new DateTime(2026, 1, 15) }
                    }
                ]
            });
        var service = new AddCourseTypeFromRulesService(rules.Object,
            new ApprenticeshipUnitVersionService(courses.Object), mapper.Object,
            Mock.Of<ILogger<AddCourseTypeFromRulesService>>());
        var learners = Enumerable.Range(0, 50).Select(i => new BulkUploadAddDraftApprenticeshipRequest
        {
            CourseCode = i % 2 == 0 ? "ZSC00001" : otherCourseCode,
            ActualStartDate = new DateTime(2026, 1, 14 + i % 2)
        }).ToList();

        var result = await service.MapAndAddCourseTypeData(learners);

        result.Should().HaveCount(50);
        for (var i = 0; i < result.Count; i++)
        {
            var expectedVersion = i % 2 == 0 ? "1.0" : "1.1";
            result[i].TrainingCourseVersion.Should().Be(expectedVersion);
            result[i].StandardUId.Should().Be($"AU0001_{expectedVersion}");
        }
        rules.Verify(x => x.GetCourseTypeRulesAsync("ZSC00001"), Times.Once);
        courses.Verify(x => x.Get<GetCourseVersionsResponse>(It.Is<GetCourseVersionsRequest>(
            r => r.GetUrl == "api/courses/lookup/versions/AU0001")), Times.Once);
    }
}
