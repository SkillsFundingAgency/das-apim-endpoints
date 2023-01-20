using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.ApprenticeshipDetails
{
    public class GetApprenticeshipDetailsQuery : IRequest<GetApprenticeshipDetailsQueryResult>
    {
        public long ApprenticeshipId { get; set; }
    }

    public class GetApprenticeshipDetailsQueryResult
    {
        public bool HasMultipleDeliveryModelOptions { get; set; }
    }

    public class GetApprenticeshipDetailsQueryHandler : IRequestHandler<GetApprenticeshipDetailsQuery, GetApprenticeshipDetailsQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IDeliveryModelService _deliveryModelService;
        private readonly ServiceParameters _serviceParameters;

        public GetApprenticeshipDetailsQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IDeliveryModelService deliveryModelService, ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _deliveryModelService = deliveryModelService;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetApprenticeshipDetailsQueryResult> Handle(GetApprenticeshipDetailsQuery request, CancellationToken cancellationToken)
        {
            var innerApiRequest = new GetApprenticeshipRequest(request.ApprenticeshipId);
            var innerApiResponse = await _apiClient.GetWithResponseCode<GetApprenticeshipResponse>(innerApiRequest);

            if (innerApiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            innerApiResponse.EnsureSuccessStatusCode();
            var apprenticeship = innerApiResponse.Body;

            if (!apprenticeship.CheckParty(_serviceParameters))
            {
                return null;
            }

            var deliveryModel = await _deliveryModelService.GetDeliveryModels(apprenticeship.ProviderId,
                apprenticeship.CourseCode, apprenticeship.AccountLegalEntityId, apprenticeship.ContinuationOfId);

            return new GetApprenticeshipDetailsQueryResult
            {
                HasMultipleDeliveryModelOptions = deliveryModel.Count > 1
            };
        }
    }
}
