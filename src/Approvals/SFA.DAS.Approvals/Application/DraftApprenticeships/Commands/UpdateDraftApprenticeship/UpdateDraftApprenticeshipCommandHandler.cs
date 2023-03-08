using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.UpdateDraftApprenticeship
{
    public class UpdateDraftApprenticeshipCommandHandler : IRequestHandler<UpdateDraftApprenticeshipCommand, UpdateDraftApprenticeshipResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        
        public UpdateDraftApprenticeshipCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<UpdateDraftApprenticeshipResult> Handle(UpdateDraftApprenticeshipCommand request, CancellationToken cancellationToken)
        {
            var updateDraftApprenticeshipRequest = new UpdateDraftApprenticeshipRequest
            {
                ActualStartDate = request.ActualStartDate,
                Cost = request.Cost,
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
                ReservationId = request.ReservationId,
                StartDate = request.StartDate,
                Uln = request.Uln,
                UserInfo = request.UserInfo,
                CourseOption = request.CourseOption,
                Reference = request.Reference
            };
            var response = await _apiClient.PutWithResponseCode<UpdateDraftApprenticeshipResponse>(new PutUpdateDraftApprenticeshipRequest(request.CohortId, request.ApprenticeshipId, updateDraftApprenticeshipRequest));
            response.EnsureSuccessStatusCode();

            return new UpdateDraftApprenticeshipResult
            {
                ApprenticeshipId = response.Body.ApprenticeshipId,
                Id = response.Body.Id
            };
        }
    }
}