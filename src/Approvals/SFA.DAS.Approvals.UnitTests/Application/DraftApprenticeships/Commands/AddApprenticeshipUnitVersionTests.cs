using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddDraftApprenticeship;
using SFA.DAS.Approvals.InnerApi.CoursesApi;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Commands;

[TestFixture]
public class AddApprenticeshipUnitVersionTests
{
    [TestCase(14, "1.0")]
    [TestCase(15, "1.1")]
    public async Task FirstLearnerIsStampedWhenCreatingCohort(int day, string expectedVersion)
    {
        var commitments = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        var rules = Rules();
        var versions = Versions();
        commitments.Setup(x => x.PostWithResponseCode<CreateCohortResponse>(It.IsAny<PostCreateCohortRequest>(), true))
            .ReturnsAsync(new ApiResponse<CreateCohortResponse>(new CreateCohortResponse(), HttpStatusCode.OK, null));
        var handler = new CreateCohortCommandHandler(commitments.Object, Mock.Of<IAutoReservationsService>(), rules.Object, versions);
        var command = new CreateCohortCommand
        {
            CourseCode = "ZSC00001", ActualStartDate = new DateTime(2026, 1, day),
            StartDate = new DateTime(2026, 1, 1), ReservationId = Guid.NewGuid()
        };

        await handler.Handle(command, CancellationToken.None);

        commitments.Verify(x => x.PostWithResponseCode<CreateCohortResponse>(It.Is<PostCreateCohortRequest>(r =>
            ((CreateCohortRequest)r.Data).CourseCode == "ZSC00001"
            && ((CreateCohortRequest)r.Data).TrainingCourseVersion == expectedVersion
            && ((CreateCohortRequest)r.Data).StandardUId == $"AU0001_{expectedVersion}"), true), Times.Once);
    }

    [TestCase(14, "1.0")]
    [TestCase(15, "1.1")]
    public async Task AnotherLearnerIsStampedWhenAddingToExistingCohort(int day, string expectedVersion)
    {
        var commitments = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        commitments.Setup(x => x.Get<GetCohortResponse>(It.IsAny<GetCohortRequest>())).ReturnsAsync(new GetCohortResponse());
        commitments.Setup(x => x.PostWithResponseCode<AddDraftApprenticeshipResponse>(It.IsAny<PostAddDraftApprenticeshipRequest>(), true))
            .ReturnsAsync(new ApiResponse<AddDraftApprenticeshipResponse>(new AddDraftApprenticeshipResponse(), HttpStatusCode.OK, null));
        var handler = new AddDraftApprenticeshipCommandHandler(commitments.Object, Mock.Of<IAutoReservationsService>(), Rules().Object,
            Versions(), Mock.Of<ILogger<AddDraftApprenticeshipCommandHandler>>());
        var command = new AddDraftApprenticeshipCommand
        {
            CohortId = 123, CourseCode = "ZSC00001", ActualStartDate = new DateTime(2026, 1, day),
            StartDate = new DateTime(2026, 1, 1), ReservationId = Guid.NewGuid()
        };

        await handler.Handle(command, CancellationToken.None);

        commitments.Verify(x => x.PostWithResponseCode<AddDraftApprenticeshipResponse>(It.Is<PostAddDraftApprenticeshipRequest>(r =>
            ((AddDraftApprenticeshipRequest)r.Data).CourseCode == "ZSC00001"
            && ((AddDraftApprenticeshipRequest)r.Data).TrainingCourseVersion == expectedVersion
            && ((AddDraftApprenticeshipRequest)r.Data).StandardUId == $"AU0001_{expectedVersion}"), true), Times.Once);
    }

    private static Mock<ICourseTypeRulesService> Rules()
    {
        var rules = new Mock<ICourseTypeRulesService>();
        rules.Setup(x => x.GetCourseTypeRulesAsync("ZSC00001")).ReturnsAsync(new CourseTypeRulesResult
        {
            Course = new GetCourseLookupResponse { LearningType = "ApprenticeshipUnit", IfateReferenceNumber = "AU0001", LarsCode = "ZSC00001" },
            LearnerAgeRules = new GetLearnerAgeResponse()
        });
        return rules;
    }

    private static ApprenticeshipUnitVersionService Versions()
    {
        var courses = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        courses.Setup(x => x.Get<GetCourseVersionsResponse>(It.Is<GetCourseVersionsRequest>(r => r.GetUrl == "api/courses/lookup/versions/AU0001")))
            .ReturnsAsync(new GetCourseVersionsResponse
            {
                Courses = new List<GetCourseLookupResponse>
                {
                    new()
                    {
                        Version = "1.0", VersionMajor = 1, StandardUId = "AU0001_1.0",
                        VersionDetail = new StandardVersionDetail { EarliestStartDate = new DateTime(2026, 1, 1), LatestStartDate = new DateTime(2026, 1, 14) }
                    },
                    new()
                    {
                        Version = "1.1", VersionMajor = 1, VersionMinor = 1, StandardUId = "AU0001_1.1",
                        VersionDetail = new StandardVersionDetail { EarliestStartDate = new DateTime(2026, 1, 15) }
                    }
                }
            });
        return new ApprenticeshipUnitVersionService(courses.Object);
    }
}
