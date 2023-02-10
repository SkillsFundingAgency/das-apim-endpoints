using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeship
{
    public class GetEditDraftApprenticeshipQueryHandler : IRequestHandler<GetEditDraftApprenticeshipQuery, GetEditDraftApprenticeshipQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IDeliveryModelService _deliveryModelService;
        private readonly ServiceParameters _serviceParameters;

        public GetEditDraftApprenticeshipQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IDeliveryModelService deliveryModelService, ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _deliveryModelService = deliveryModelService;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetEditDraftApprenticeshipQueryResult> Handle(GetEditDraftApprenticeshipQuery request, CancellationToken cancellationToken)
        {
            var innerApiRequest = new GetDraftApprenticeshipRequest(request.CohortId, request.DraftApprenticeshipId);
            var cohortRequest = new GetCohortRequest(request.CohortId);

            var innerApiResponseTask = _apiClient.GetWithResponseCode<GetDraftApprenticeshipResponse>(innerApiRequest);
            var cohortResponseTask = _apiClient.GetWithResponseCode<GetCohortResponse>(cohortRequest);

            await Task.WhenAll(innerApiResponseTask, cohortResponseTask);

            if (innerApiResponseTask.Result.StatusCode == HttpStatusCode.NotFound || cohortResponseTask.Result.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            innerApiResponseTask.Result.EnsureSuccessStatusCode();
            cohortResponseTask.Result.EnsureSuccessStatusCode();

            var apprenticeship = innerApiResponseTask.Result.Body;
            var cohort = cohortResponseTask.Result.Body;

            if (!cohort.CheckParty(_serviceParameters))
            {
                return null;
            }

            var courseCode = !string.IsNullOrWhiteSpace(request.CourseCode)
                ? request.CourseCode
                : apprenticeship.CourseCode;

            var deliveryModels = await _deliveryModelService.GetDeliveryModels(cohort.ProviderId,
                courseCode, cohort.AccountLegalEntityId, apprenticeship.ContinuationOfId);

            return new GetEditDraftApprenticeshipQueryResult
            {
                FirstName = apprenticeship.FirstName,
                LastName = apprenticeship.LastName,
                DateOfBirth = apprenticeship.DateOfBirth,
                ReservationId = apprenticeship.ReservationId,
                Email = apprenticeship.Email,
                Uln = apprenticeship.Uln,
                DeliveryModel = apprenticeship.DeliveryModel,
                CourseCode = apprenticeship.CourseCode,
                StandardUId = apprenticeship.StandardUId,
                CourseName = apprenticeship.TrainingCourseName,
                HasStandardOptions = apprenticeship.HasStandardOptions,
                TrainingCourseOption = apprenticeship.TrainingCourseOption,
                StartDate = apprenticeship.StartDate,
                ActualStartDate =  apprenticeship.ActualStartDate,
                EndDate = apprenticeship.EndDate,
                Cost = apprenticeship.Cost,
                EmploymentPrice = apprenticeship.EmploymentPrice,
                EmploymentEndDate = apprenticeship.EmploymentEndDate,
                EmployerReference = apprenticeship.EmployerReference,
                ProviderReference = apprenticeship.ProviderReference,
                ProviderId = cohort.ProviderId,
                ProviderName = cohort.ProviderName,
                LegalEntityName = cohort.LegalEntityName,
                AccountLegalEntityId = cohort.AccountLegalEntityId,
                IsContinuation = apprenticeship.IsContinuation,
                HasMultipleDeliveryModelOptions = deliveryModels.Count > 1,
                HasUnavailableDeliveryModel = !deliveryModels.Contains(apprenticeship.DeliveryModel.ToString()),
                RecognisePriorLearning = apprenticeship.RecognisePriorLearning,
                DurationReducedBy = apprenticeship.DurationReducedBy,
                PriceReducedBy = apprenticeship.PriceReducedBy,
                RecognisingPriorLearningStillNeedsToBeConsidered = apprenticeship.RecognisingPriorLearningStillNeedsToBeConsidered,
                IsOnFlexiPaymentPilot = apprenticeship.IsOnFlexiPaymentPilot,
                EmailAddressConfirmed = apprenticeship.EmailAddressConfirmed
            };
        }
    }
}
