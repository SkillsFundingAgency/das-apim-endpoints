using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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

        public GetEditDraftApprenticeshipQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IDeliveryModelService deliveryModelService)
        {
            _apiClient = apiClient;
            _deliveryModelService = deliveryModelService;
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

            if (!CheckParty(cohort, request))
            {
                return null;
            }

            var deliveryModels = await _deliveryModelService.GetDeliveryModels(cohort.ProviderId,
                apprenticeship.CourseCode, cohort.AccountLegalEntityId, apprenticeship.ContinuationOfId);

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
                StartDate = apprenticeship.StartDate,
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
                RecognisePriorLearning = apprenticeship.RecognisePriorLearning,
                DurationReducedBy = apprenticeship.DurationReducedBy,
                PriceReducedBy = apprenticeship.PriceReducedBy,
                RecognisingPriorLearningStillNeedsToBeConsidered = apprenticeship.RecognisingPriorLearningStillNeedsToBeConsidered
            };
        }

        private bool CheckParty(GetCohortResponse cohort, GetEditDraftApprenticeshipQuery query)
        {
            switch (query.Party)
            {
                case Party.Employer:
                {
                    if (cohort.AccountId != query.PartyId)
                    {
                        return false;
                    }

                    break;
                }
                case Party.Provider:
                {
                    if (cohort.ProviderId != query.PartyId)
                    {
                        return false;
                    }

                    break;
                }
                default:
                    return false;
            }

            return true;
        }
    }
}
