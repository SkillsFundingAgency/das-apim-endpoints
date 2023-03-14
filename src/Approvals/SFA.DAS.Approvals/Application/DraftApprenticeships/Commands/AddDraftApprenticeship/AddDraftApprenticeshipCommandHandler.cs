using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddDraftApprenticeship
{
    public class AddDraftApprenticeshipCommandHandler : IRequestHandler<AddDraftApprenticeshipCommand, AddDraftApprenticeshipResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        
        public AddDraftApprenticeshipCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<AddDraftApprenticeshipResult> Handle(AddDraftApprenticeshipCommand request, CancellationToken cancellationToken)
        {
            var addDraftApprenticeshipRequest = new AddDraftApprenticeshipRequest
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
                OriginatorReference = request.OriginatorReference,
                ProviderId = request.ProviderId,
                ReservationId = request.ReservationId,
                StartDate = request.StartDate,
                Uln = request.Uln,
                UserInfo = request.UserInfo,
                UserId = request.UserId
            };
            var response = await _apiClient.PostWithResponseCode<AddDraftApprenticeshipResponse>(new PostAddDraftApprenticeshipRequest(request.CohortId, addDraftApprenticeshipRequest));

            return new AddDraftApprenticeshipResult
            {
                DraftApprenticeshipId = response.Body.DraftApprenticeshipId
            };
        }
    }
}