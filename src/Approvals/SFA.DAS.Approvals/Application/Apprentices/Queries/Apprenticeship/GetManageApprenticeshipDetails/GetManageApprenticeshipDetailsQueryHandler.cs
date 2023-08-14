using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Enums;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails
{
    public class GetManageApprenticeshipDetailsQueryHandler : IRequestHandler<GetManageApprenticeshipDetailsQuery, GetManageApprenticeshipDetailsQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IDeliveryModelService _deliveryModelService;
        private readonly ServiceParameters _serviceParameters;

        public GetManageApprenticeshipDetailsQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IDeliveryModelService deliveryModelService, ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _deliveryModelService = deliveryModelService;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetManageApprenticeshipDetailsQueryResult> Handle(GetManageApprenticeshipDetailsQuery request, CancellationToken cancellationToken)
        {
            var apprenticeshipResponse = await _apiClient.GetWithResponseCode<GetApprenticeshipResponse>(new GetApprenticeshipRequest(request.ApprenticeshipId));

            if (apprenticeshipResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            apprenticeshipResponse.EnsureSuccessStatusCode();
            var apprenticeship = apprenticeshipResponse.Body;

            if (!apprenticeship.CheckParty(_serviceParameters))
            {
                throw new UnauthorizedAccessException(
                    $"You do not permissions to access apprenticeship {request.ApprenticeshipId}");
            }

            var priceEpisodesResponse = await _apiClient.GetWithResponseCode<GetPriceEpisodesResponse>(new GetPriceEpisodesRequest(apprenticeship.Id));
            var apprenticeshipUpdatesResponse = await _apiClient.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(new GetApprenticeshipUpdatesRequest(apprenticeship.Id, ApprenticeshipUpdateStatus.Pending));
            //var apprenticeshipDataLockStatusResponse = await _apiClient.GetWithResponseCode<GetApprenticeshipDataLockStatusResponse>(new GetApprenticeshipDataLockStatusRequest(apprenticeship.Id)); 
            //var changeOfPartyResponse = await _apiClient.GetWithResponseCode<GetApprenticeshipChangeOfPartyResponse>(new GetApprenticeshipChangeOfPartyRequest(apprenticeship.Id));
            //var changeOfProviderChainResponse = await _apiClient.GetWithResponseCode<GetApprenticeshipChangeOfProviderChainResponse>(new GetApprenticeshipChangeOfProviderChainRequest(apprenticeship.Id)); 
            // ????? different var overlappingTrainingDateResponse = await _apiClient.GetWithResponseCode<GetOverlapRequestResponse>(new GetOverlapRequestRequest(apprenticeship.Id));

            var deliveryModel = await _deliveryModelService.GetDeliveryModels(apprenticeship.ProviderId,
                apprenticeship.CourseCode, apprenticeship.AccountLegalEntityId, apprenticeship.ContinuationOfId);

            return new GetManageApprenticeshipDetailsQueryResult
            {
                Apprenticeship = apprenticeship,
                PriceEpisodes = priceEpisodesResponse.Body.PriceEpisodes,
                ApprenticeshipUpdates = apprenticeshipUpdatesResponse.Body.ApprenticeshipUpdates,
                HasMultipleDeliveryModelOptions = deliveryModel.Count > 1
            };
        }
    }
}