using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipDeliveryModel
{
    public class GetEditDraftApprenticeshipDeliveryModelQuery : IRequest<GetEditDraftApprenticeshipDeliveryModelQueryResult>
    {
        public long CohortId { get; set; }
        public long DraftApprenticeshipId { get; set; }
        public string CourseCode { get; set; }
    }

    public class GetEditDraftApprenticeshipDeliveryModelQueryResult
    {
        public string DeliveryModel { get; set; }
        public List<string> DeliveryModels { get; set; }
        public bool HasUnavailableDeliveryModel { get; set; }
        public string EmployerName { get; set; }
    }

    public class GetEditDraftApprenticeshipDeliveryModelQueryHandler : IRequestHandler<GetEditDraftApprenticeshipDeliveryModelQuery, GetEditDraftApprenticeshipDeliveryModelQueryResult>
    {
        private readonly IDeliveryModelService _deliveryModelService;
        private readonly ServiceParameters _serviceParameters;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public GetEditDraftApprenticeshipDeliveryModelQueryHandler(IDeliveryModelService deliveryModelService, ServiceParameters serviceParameters, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _deliveryModelService = deliveryModelService;
            _serviceParameters = serviceParameters;
            _apiClient = apiClient;
        }

        public async Task<GetEditDraftApprenticeshipDeliveryModelQueryResult> Handle(GetEditDraftApprenticeshipDeliveryModelQuery request, CancellationToken cancellationToken)
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

            return new GetEditDraftApprenticeshipDeliveryModelQueryResult
            {
                DeliveryModel = apprenticeship.DeliveryModel.ToString(),
                DeliveryModels = deliveryModels,
                HasUnavailableDeliveryModel = !deliveryModels.Contains(apprenticeship.DeliveryModel.ToString()),
                EmployerName = cohort.LegalEntityName
            };
        }
    }
}
