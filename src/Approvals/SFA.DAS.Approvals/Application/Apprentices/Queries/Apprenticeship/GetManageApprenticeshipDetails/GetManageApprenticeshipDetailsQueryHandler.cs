using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Enums;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetPendingPriceChange;
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
        private readonly IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> _apprenticeshipsApiClient;

        public GetManageApprenticeshipDetailsQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IDeliveryModelService deliveryModelService, ServiceParameters serviceParameters, IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apprenticeshipsApiClient)
        {
            _apiClient = apiClient;
            _deliveryModelService = deliveryModelService;
            _serviceParameters = serviceParameters;
            _apprenticeshipsApiClient = apprenticeshipsApiClient;
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

            var priceEpisodesResponseTask = _apiClient.GetWithResponseCode<GetPriceEpisodesResponse>(new GetPriceEpisodesRequest(apprenticeship.Id) );
            var apprenticeshipUpdatesResponseTask = _apiClient.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(new GetApprenticeshipUpdatesRequest(apprenticeship.Id, ApprenticeshipUpdateStatus.Pending));
            var apprenticeshipDataLockStatusResponseTask = _apiClient.GetWithResponseCode<GetDataLocksResponse>(new GetDataLocksRequest(apprenticeship.Id));
            var changeOfPartyRequestsResponseTask = _apiClient.GetWithResponseCode<GetChangeOfPartyRequestsResponse>(new GetChangeOfPartyRequestsRequest(apprenticeship.Id));
            var changeOfProviderChainResponseTask = _apiClient.GetWithResponseCode<GetChangeOfProviderChainResponse>(new GetChangeOfProviderChainRequest(apprenticeship.Id));
            var changeOfEmployerChainResponseTask = _apiClient.GetWithResponseCode<GetChangeOfEmployerChainResponse>(new GetChangeOfEmployerChainRequest(apprenticeship.Id));
            var overlappingTrainingDateResponseTask = _apiClient.GetWithResponseCode<GetOverlappingTrainingDateResponse>(new GetOverlappingTrainingDateRequest(apprenticeship.Id));
            var deliveryModelTask = _deliveryModelService.GetDeliveryModels(apprenticeship.ProviderId,
                apprenticeship.CourseCode, apprenticeship.AccountLegalEntityId, apprenticeship.ContinuationOfId);
            var pendingPriceChangeTask = _apprenticeshipsApiClient.GetWithResponseCode<GetPendingPriceChangeResponse>(new GetPendingPriceChangeRequest(apprenticeship.Id));

            await Task.WhenAll(priceEpisodesResponseTask, 
                apprenticeshipUpdatesResponseTask,
                apprenticeshipDataLockStatusResponseTask,
                changeOfPartyRequestsResponseTask, 
                changeOfProviderChainResponseTask,
                changeOfEmployerChainResponseTask, 
                overlappingTrainingDateResponseTask, 
                deliveryModelTask,
                pendingPriceChangeTask);

            var priceEpisodesResponse = priceEpisodesResponseTask.Result;
            var apprenticeshipUpdatesResponse = apprenticeshipUpdatesResponseTask.Result;
            var apprenticeshipDataLockStatusResponse = apprenticeshipDataLockStatusResponseTask.Result; 
            var changeOfPartyRequestsResponse = changeOfPartyRequestsResponseTask.Result;
            var changeOfProviderChainResponse = changeOfProviderChainResponseTask.Result;
            var changeOfEmployerChainResponse = changeOfEmployerChainResponseTask.Result;
            var overlappingTrainingDateResponse = overlappingTrainingDateResponseTask.Result;
            var deliveryModel = deliveryModelTask.Result;
            var pendingPriceChangeResponse = pendingPriceChangeTask.Result;

            return new GetManageApprenticeshipDetailsQueryResult
            {
                Apprenticeship = apprenticeship,
                PriceEpisodes = priceEpisodesResponse.Body.PriceEpisodes,
                ApprenticeshipUpdates = apprenticeshipUpdatesResponse.Body.ApprenticeshipUpdates,
                DataLocks = apprenticeshipDataLockStatusResponse.Body.DataLocks,
                ChangeOfPartyRequests = changeOfPartyRequestsResponse.Body.ChangeOfPartyRequests,
                ChangeOfProviderChain = changeOfProviderChainResponse.Body.ChangeOfProviderChain,
                ChangeOfEmployerChain = changeOfEmployerChainResponse.Body.ChangeOfEmployerChain,
                OverlappingTrainingDateRequest = overlappingTrainingDateResponse.Body.OverlappingTrainingDateRequest,
                HasMultipleDeliveryModelOptions = deliveryModel.Count > 1,
                PendingPriceChange = ToResponse(pendingPriceChangeResponse.Body)
            };
        }

        private PendingPriceChange ToResponse(GetPendingPriceChangeResponse pendingPriceChangeResponse)
        {
            if(!pendingPriceChangeResponse.HasPendingPriceChange) return null;

            return new PendingPriceChange
            {
                Cost = pendingPriceChangeResponse.PendingPriceChange.PendingTotalPrice,
                EndPointAssessmentPrice = pendingPriceChangeResponse.PendingPriceChange.PendingAssessmentPrice,
                TrainingPrice = pendingPriceChangeResponse.PendingPriceChange.PendingTrainingPrice
            };
        }
    }
}