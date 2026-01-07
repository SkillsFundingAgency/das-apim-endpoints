using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.UpdateDraftApprenticeship;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Commands;

[TestFixture]
public class UpdateDraftApprenticeshipCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_Update_Draft_Apprenticeship(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        GetStandardsListItem standardResponse,
        GetLearnerAgeResponse learnerAgeResponse,
        UpdateDraftApprenticeshipCommand request,
        UpdateDraftApprenticeshipCommandHandler handler)
    {
        // Arrange
        standardResponse.ApprenticeshipType = "Apprenticeship";
            
        courseTypeRulesService
            .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult
            {
                Standard = standardResponse,
                LearnerAgeRules = learnerAgeResponse
            });

        commitmentsApiClient.Setup(x => x.PutWithResponseCode<NullResponse>(
            It.Is<PutUpdateDraftApprenticeshipRequest>(r =>
                r.CohortId == request.CohortId &&
                r.ApprenticeshipId == request.ApprenticeshipId &&
                ((UpdateDraftApprenticeshipRequest)r.Data).ActualStartDate == request.ActualStartDate &&
                ((UpdateDraftApprenticeshipRequest)r.Data).Cost == request.Cost &&
                ((UpdateDraftApprenticeshipRequest)r.Data).TrainingPrice == request.TrainingPrice &&
                ((UpdateDraftApprenticeshipRequest)r.Data).EndPointAssessmentPrice == request.EndPointAssessmentPrice &&
                ((UpdateDraftApprenticeshipRequest)r.Data).CourseCode == request.CourseCode &&
                ((UpdateDraftApprenticeshipRequest)r.Data).DateOfBirth == request.DateOfBirth &&
                ((UpdateDraftApprenticeshipRequest)r.Data).DeliveryModel == request.DeliveryModel &&
                ((UpdateDraftApprenticeshipRequest)r.Data).Email == request.Email &&
                ((UpdateDraftApprenticeshipRequest)r.Data).EmploymentEndDate == request.EmploymentEndDate &&
                ((UpdateDraftApprenticeshipRequest)r.Data).EmploymentPrice == request.EmploymentPrice &&
                ((UpdateDraftApprenticeshipRequest)r.Data).EndDate == request.EndDate &&
                ((UpdateDraftApprenticeshipRequest)r.Data).FirstName == request.FirstName &&
                ((UpdateDraftApprenticeshipRequest)r.Data).IgnoreStartDateOverlap == request.IgnoreStartDateOverlap &&
                ((UpdateDraftApprenticeshipRequest)r.Data).IsOnFlexiPaymentPilot == request.IsOnFlexiPaymentPilot &&
                ((UpdateDraftApprenticeshipRequest)r.Data).LastName == request.LastName &&
                ((UpdateDraftApprenticeshipRequest)r.Data).CourseOption == request.CourseOption &&
                ((UpdateDraftApprenticeshipRequest)r.Data).Reference == request.Reference &&
                ((UpdateDraftApprenticeshipRequest)r.Data).ReservationId == request.ReservationId &&
                ((UpdateDraftApprenticeshipRequest)r.Data).StartDate == request.StartDate &&
                ((UpdateDraftApprenticeshipRequest)r.Data).Uln == request.Uln &&
                ((UpdateDraftApprenticeshipRequest)r.Data).UserInfo == request.UserInfo &&
                ((UpdateDraftApprenticeshipRequest)r.Data).RequestingParty == request.RequestingParty &&
                ((UpdateDraftApprenticeshipRequest)r.Data).MinimumAgeAtApprenticeshipStart ==
                learnerAgeResponse.MinimumAge &&
                ((UpdateDraftApprenticeshipRequest)r.Data).MaximumAgeAtApprenticeshipStart ==
                learnerAgeResponse.MaximumAge
            ))).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, string.Empty))
            .Verifiable(Times.Once());

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        commitmentsApiClient.Verify();
        courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(request.CourseCode), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenNoStandardDetails_DoNotUpdateDraftApprenticeship(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        UpdateDraftApprenticeshipCommand request,
        UpdateDraftApprenticeshipCommandHandler handler)
    {
        // Arrange
        courseTypeRulesService.Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ThrowsAsync(new Exception($"Standard not found for course ID {request.CourseCode}"));

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"Standard not found for course ID {request.CourseCode}");
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenNoLearnerAgeRules_DoNotUpdateDraftApprenticeship(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        GetStandardsListItem standardResponse,
        UpdateDraftApprenticeshipCommand request,
        UpdateDraftApprenticeshipCommandHandler handler)
    {
        // Arrange
        standardResponse.ApprenticeshipType = "Apprenticeship";
        courseTypeRulesService.Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ThrowsAsync(new Exception("Learner age rules not found for apprenticeship type Standard"));

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Learner age rules not found for apprenticeship type Standard");
    }
}