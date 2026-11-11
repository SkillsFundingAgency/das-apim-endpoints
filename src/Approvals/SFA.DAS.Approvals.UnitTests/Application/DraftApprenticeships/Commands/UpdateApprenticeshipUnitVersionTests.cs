using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.UpdateDraftApprenticeship;
using SFA.DAS.Approvals.InnerApi.CoursesApi;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Commands;

[TestFixture]
public class UpdateApprenticeshipUnitVersionTests
{
    [TestCase(false, false, 0, false)]
    [TestCase(false, false, -1, false)]
    [TestCase(true, false, 0, false)]
    [TestCase(false, true, 0, false)]
    [TestCase(false, false, 1, false)]
    [TestCase(true, true, 1, false)]
    [TestCase(false, false, 1, true)]
    public async Task OnlyResolvesVersionForCourseOrActualStartDayChangesOrExplicitResync(
        bool startDateChanged, bool courseChanged, int syncMinutesDelta, bool firstSync)
    {
        var commitments = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        var rules = new Mock<ICourseTypeRulesService>();
        var versions = new Mock<IApprenticeshipUnitVersionService>();
        var course = new GetCourseLookupResponse { LearningType = "ApprenticeshipUnit" };
        var startDate = new DateTime(2026, 1, 15);
        var lastSync = new DateTime(2026, 1, 16, 12, 0, 0, 123).AddTicks(3333);
        var shouldResolve = startDateChanged || courseChanged || syncMinutesDelta > 0;
        var request = new UpdateDraftApprenticeshipCommand
        {
            CourseCode = "ZSC00001", ActualStartDate = startDate, Reference = "Updated reference",
            LastLearnerDataSync = lastSync.AddMinutes(syncMinutesDelta)
        };
        rules.Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode)).ReturnsAsync(new CourseTypeRulesResult
        {
            Course = course, LearnerAgeRules = new GetLearnerAgeResponse()
        });
        commitments.Setup(x => x.Get<GetDraftApprenticeshipResponse>(It.IsAny<GetDraftApprenticeshipRequest>()))
            .ReturnsAsync(new GetDraftApprenticeshipResponse
            {
                CourseCode = courseChanged ? "ZSC00002" : "ZSC00001",
                ActualStartDate = startDateChanged ? startDate.AddDays(-1) : startDate.AddHours(3),
                LastLearnerDataSync = firstSync ? null : lastSync,
                TrainingCourseVersion = "1.0", StandardUId = "AU0001_1.0"
            });
        versions.Setup(x => x.Resolve(course, startDate)).ReturnsAsync(new GetCourseLookupResponse
        {
            Version = "1.1", StandardUId = "AU0001_1.1"
        });
        var handler = new UpdateDraftApprenticeshipCommandHandler(commitments.Object, rules.Object, versions.Object);

        await handler.Handle(request, CancellationToken.None);

        versions.Verify(x => x.Resolve(course, startDate), shouldResolve ? Times.Once() : Times.Never());
        commitments.Verify(x => x.PutWithResponseCode<SFA.DAS.Apim.Shared.Infrastructure.NullResponse>(
            It.Is<PutUpdateDraftApprenticeshipRequest>(r =>
                ((UpdateDraftApprenticeshipRequest)r.Data).TrainingCourseVersion == (shouldResolve ? "1.1" : null)
                && ((UpdateDraftApprenticeshipRequest)r.Data).StandardUId == (shouldResolve ? "AU0001_1.1" : null))), Times.Once);
    }
}
