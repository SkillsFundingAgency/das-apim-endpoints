using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetViewDraftApprenticeship
{
    public class GetViewDraftApprenticeshipQueryHandler : IRequestHandler<GetViewDraftApprenticeshipQuery, GetViewDraftApprenticeshipQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public GetViewDraftApprenticeshipQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetViewDraftApprenticeshipQueryResult> Handle(GetViewDraftApprenticeshipQuery request, CancellationToken cancellationToken)
        {
            var innerApiRequest = new GetDraftApprenticeshipRequest(request.CohortId, request.DraftApprenticeshipId);

            var innerApiResponse = await _apiClient.GetWithResponseCode<GetDraftApprenticeshipResponse>(innerApiRequest);

            if (innerApiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            innerApiResponse.EnsureSuccessStatusCode();

            var apprenticeship = innerApiResponse.Body;

            return new GetViewDraftApprenticeshipQueryResult
            {
                Id = apprenticeship.Id,
                FirstName = apprenticeship.FirstName,
                LastName = apprenticeship.LastName,
                Email = apprenticeship.Email,
                Uln = apprenticeship.Uln,
                CourseCode = apprenticeship.CourseCode,
                DeliveryModel = apprenticeship.DeliveryModel,
                TrainingCourseName = apprenticeship.TrainingCourseName,
                TrainingCourseVersion = apprenticeship.TrainingCourseVersion,
                TrainingCourseOption = apprenticeship.TrainingCourseOption,
                TrainingCourseVersionConfirmed = apprenticeship.TrainingCourseVersionConfirmed,
                StandardUId = apprenticeship.StandardUId,
                Cost = apprenticeship.Cost,
                StartDate = apprenticeship.StartDate,
                ActualStartDate = apprenticeship.ActualStartDate,
                EndDate = apprenticeship.EndDate,
                DateOfBirth = apprenticeship.DateOfBirth,
                Reference = apprenticeship.Reference,
                EmployerReference = apprenticeship.EmployerReference,
                ProviderReference = apprenticeship.ProviderReference,
                ReservationId = apprenticeship.ReservationId,
                IsContinuation = apprenticeship.IsContinuation,
                ContinuationOfId = apprenticeship.ContinuationOfId,
                OriginalStartDate = apprenticeship.OriginalStartDate,
                HasStandardOptions = apprenticeship.HasStandardOptions,
                EmploymentPrice = apprenticeship.EmploymentPrice,
                EmploymentEndDate = apprenticeship.EmploymentEndDate,
                RecognisePriorLearning = apprenticeship.RecognisePriorLearning,
                DurationReducedBy = apprenticeship.DurationReducedBy,
                PriceReducedBy = apprenticeship.PriceReducedBy,
                RecognisingPriorLearningStillNeedsToBeConsidered = apprenticeship.RecognisingPriorLearningStillNeedsToBeConsidered,
                RecognisingPriorLearningExtendedStillNeedsToBeConsidered = apprenticeship.RecognisingPriorLearningExtendedStillNeedsToBeConsidered,
                IsOnFlexiPaymentPilot = apprenticeship.IsOnFlexiPaymentPilot,
                EmailAddressConfirmed = apprenticeship.EmailAddressConfirmed,
                DurationReducedByHours = apprenticeship.DurationReducedByHours,
                IsDurationReducedByRpl = apprenticeship.IsDurationReducedByRpl,
                TrainingTotalHours = apprenticeship.TrainingTotalHours,
            };
        }
    }
}
