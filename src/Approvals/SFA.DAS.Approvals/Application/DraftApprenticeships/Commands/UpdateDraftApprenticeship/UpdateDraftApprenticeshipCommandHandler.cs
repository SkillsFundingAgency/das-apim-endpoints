using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.UpdateDraftApprenticeship;

public class UpdateDraftApprenticeshipCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
    ICourseTypeRulesService courseTypeRulesService)
    : IRequestHandler<UpdateDraftApprenticeshipCommand, Unit>
{
    public async Task<Unit> Handle(UpdateDraftApprenticeshipCommand request, CancellationToken cancellationToken)
    {
        var courseTypeRules = await courseTypeRulesService.GetCourseTypeRulesAsync(request.CourseCode);
            
        var updateDraftApprenticeshipRequest = new UpdateDraftApprenticeshipRequest
        {
            ActualStartDate = request.ActualStartDate,
            Cost = request.Cost,
            TrainingPrice = request.TrainingPrice,
            EndPointAssessmentPrice = request.EndPointAssessmentPrice,
            CourseCode = request.CourseCode,
            DateOfBirth = request.DateOfBirth,
            DeliveryModel = request.DeliveryModel,
            Email = request.Email,
            EmploymentEndDate = request.EmploymentEndDate,
            EmploymentPrice = request.EmploymentPrice,
            EndDate = request.EndDate,
            FirstName = request.FirstName,
            IgnoreStartDateOverlap = request.IgnoreStartDateOverlap,
            IsOnFlexiPaymentPilot = request.IsOnFlexiPaymentPilot,
            LastName = request.LastName,
            CourseOption = request.CourseOption,
            Reference = request.Reference,
            ReservationId = request.ReservationId,
            StartDate = request.StartDate,
            Uln = request.Uln,
            UserInfo = request.UserInfo,
            RequestingParty = request.RequestingParty,
            MinimumAgeAtApprenticeshipStart = courseTypeRules.LearnerAgeRules.MinimumAge,
            MaximumAgeAtApprenticeshipStart = courseTypeRules.LearnerAgeRules.MaximumAge
        };

        await commitmentsApiClient.PutWithResponseCode<NullResponse>(
            new PutUpdateDraftApprenticeshipRequest(request.CohortId, request.ApprenticeshipId, updateDraftApprenticeshipRequest));

        return Unit.Value;
    }
}