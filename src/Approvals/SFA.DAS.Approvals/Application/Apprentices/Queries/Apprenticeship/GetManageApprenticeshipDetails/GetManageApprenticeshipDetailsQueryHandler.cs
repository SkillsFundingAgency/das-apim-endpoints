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
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.Interfaces;
using GetApprenticeshipKeyRequest = SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetApprenticeshipKey.GetApprenticeshipKeyRequest;
using GetPendingPriceChangeRequest = SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetPendingPriceChange.GetPendingPriceChangeRequest;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails
{
    public class GetManageApprenticeshipDetailsQueryHandler : IRequestHandler<GetManageApprenticeshipDetailsQuery, GetManageApprenticeshipDetailsQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IDeliveryModelService _deliveryModelService;
        private readonly ServiceParameters _serviceParameters;
        private readonly IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> _apprenticeshipsApiClient;
        private readonly ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> _collectionCalendarApiClient;

        public GetManageApprenticeshipDetailsQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IDeliveryModelService deliveryModelService, ServiceParameters serviceParameters, IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apprenticeshipsApiClient, ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient)
        {
            _apiClient = apiClient;
            _deliveryModelService = deliveryModelService;
            _serviceParameters = serviceParameters;
            _apprenticeshipsApiClient = apprenticeshipsApiClient;
            _collectionCalendarApiClient = collectionCalendarApiClient;
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

            var apprenticeshipKey = await _apprenticeshipsApiClient.GetWithResponseCode<Guid>(new GetApprenticeshipKeyRequest(request.ApprenticeshipId));

			var priceEpisodesResponseTask = _apiClient.GetWithResponseCode<GetPriceEpisodesResponse>(new GetPriceEpisodesRequest(apprenticeship.Id) );
            var apprenticeshipUpdatesResponseTask = _apiClient.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(new GetApprenticeshipUpdatesRequest(apprenticeship.Id, ApprenticeshipUpdateStatus.Pending));
            var apprenticeshipDataLockStatusResponseTask = _apiClient.GetWithResponseCode<GetDataLocksResponse>(new GetDataLocksRequest(apprenticeship.Id));
            var changeOfPartyRequestsResponseTask = _apiClient.GetWithResponseCode<GetChangeOfPartyRequestsResponse>(new GetChangeOfPartyRequestsRequest(apprenticeship.Id));
            var changeOfProviderChainResponseTask = _apiClient.GetWithResponseCode<GetChangeOfProviderChainResponse>(new GetChangeOfProviderChainRequest(apprenticeship.Id));
            var changeOfEmployerChainResponseTask = _apiClient.GetWithResponseCode<GetChangeOfEmployerChainResponse>(new GetChangeOfEmployerChainRequest(apprenticeship.Id));
            var overlappingTrainingDateResponseTask = _apiClient.GetWithResponseCode<GetOverlappingTrainingDateResponse>(new GetOverlappingTrainingDateRequest(apprenticeship.Id));
            var deliveryModelTask = _deliveryModelService.GetDeliveryModels(apprenticeship.ProviderId,
                apprenticeship.CourseCode, apprenticeship.AccountLegalEntityId, apprenticeship.ContinuationOfId);
            var pendingPriceChangeTask = _apprenticeshipsApiClient.GetWithResponseCode<GetPendingPriceChangeResponse>(new GetPendingPriceChangeRequest(apprenticeshipKey.Body));
            var canActualStartDateBeChangedTask = CanActualStartDateBeChanged(apprenticeship.ActualStartDate);
            var pendingStartDateChangeTask = _apprenticeshipsApiClient.GetWithResponseCode<GetPendingStartDateChangeApiResponse>(new GetPendingStartDateChangeRequest(apprenticeshipKey.Body));

            await Task.WhenAll(priceEpisodesResponseTask, 
                apprenticeshipUpdatesResponseTask,
                apprenticeshipDataLockStatusResponseTask,
                changeOfPartyRequestsResponseTask, 
                changeOfProviderChainResponseTask,
                changeOfEmployerChainResponseTask, 
                overlappingTrainingDateResponseTask, 
                deliveryModelTask,
                pendingPriceChangeTask,
                canActualStartDateBeChangedTask,
                pendingStartDateChangeTask);

            var priceEpisodesResponse = priceEpisodesResponseTask.Result;
            var apprenticeshipUpdatesResponse = apprenticeshipUpdatesResponseTask.Result;
            var apprenticeshipDataLockStatusResponse = apprenticeshipDataLockStatusResponseTask.Result; 
            var changeOfPartyRequestsResponse = changeOfPartyRequestsResponseTask.Result;
            var changeOfProviderChainResponse = changeOfProviderChainResponseTask.Result;
            var changeOfEmployerChainResponse = changeOfEmployerChainResponseTask.Result;
            var overlappingTrainingDateResponse = overlappingTrainingDateResponseTask.Result;
            var deliveryModel = deliveryModelTask.Result;
            var pendingPriceChangeResponse = pendingPriceChangeTask.Result;
            var canActualStartDateBeChanged = canActualStartDateBeChangedTask.Result;
            var pendingStartDateResponse = pendingStartDateChangeTask.Result;

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
                PendingPriceChange = ToResponse(pendingPriceChangeResponse.Body),
                CanActualStartDateBeChanged = canActualStartDateBeChanged,
                PendingStartDateChange = ToResponse(pendingStartDateResponse.Body)
            };
        }

        private PendingPriceChange ToResponse(GetPendingPriceChangeResponse pendingPriceChangeResponse)
        {
            if(pendingPriceChangeResponse == null || !pendingPriceChangeResponse.HasPendingPriceChange) return null;

            return new PendingPriceChange
            {
                Cost = pendingPriceChangeResponse.PendingPriceChange.PendingTotalPrice,
                EndPointAssessmentPrice = pendingPriceChangeResponse.PendingPriceChange.PendingAssessmentPrice,
                TrainingPrice = pendingPriceChangeResponse.PendingPriceChange.PendingTrainingPrice,
                ProviderApprovedDate = pendingPriceChangeResponse.PendingPriceChange.ProviderApprovedDate,
                EmployerApprovedDate = pendingPriceChangeResponse.PendingPriceChange.EmployerApprovedDate,
                Initiator = pendingPriceChangeResponse.PendingPriceChange.Initiator,
            };
        }
        private PendingStartDateChange ToResponse(GetPendingStartDateChangeApiResponse pendingStartDateChangeResponse)
        {
            if (pendingStartDateChangeResponse == null || !pendingStartDateChangeResponse.HasPendingStartDateChange) return null;

            return new PendingStartDateChange
            {
                PendingActualStartDate = pendingStartDateChangeResponse.PendingStartDateChange.PendingActualStartDate,
                ProviderApprovedDate = pendingStartDateChangeResponse.PendingStartDateChange.ProviderApprovedDate,
                EmployerApprovedDate = pendingStartDateChangeResponse.PendingStartDateChange.EmployerApprovedDate,
                Initiator = pendingStartDateChangeResponse.PendingStartDateChange.Initiator
            };
        }

        private async Task<bool?> CanActualStartDateBeChanged(DateTime? actualStartDate)
        {
            if(actualStartDate == null) return null;
            var currentAcademicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearsRequest(DateTime.Now));
            if (currentAcademicYear.StartDate <= actualStartDate) return true;
            var previousAcademicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearsRequest(DateTime.Now.AddYears(-1)));
            if (previousAcademicYear.StartDate <= actualStartDate && previousAcademicYear.HardCloseDate > DateTime.Now) return true;
            return false;
        }
    }
}