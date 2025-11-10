using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apprenticeships.Types;
using SFA.DAS.Approvals.Enums;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;
using GetApprenticeshipUpdatesResponse = SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetApprenticeshipUpdatesResponse;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails;

public class GetManageApprenticeshipDetailsQueryHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
    IDeliveryModelService deliveryModelService,
    ServiceParameters serviceParameters,
    ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient)
    : IRequestHandler<GetManageApprenticeshipDetailsQuery, GetManageApprenticeshipDetailsQueryResult>
{
    public async Task<GetManageApprenticeshipDetailsQueryResult> Handle(GetManageApprenticeshipDetailsQuery request, CancellationToken cancellationToken)
    {
        var apprenticeshipResponse = await apiClient.GetWithResponseCode<GetApprenticeshipResponse>(new GetApprenticeshipRequest(request.ApprenticeshipId));

        if (apprenticeshipResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        apprenticeshipResponse.EnsureSuccessStatusCode();
        
        var apprenticeship = apprenticeshipResponse.Body;

        if (apprenticeship == null)
        {
            throw new ResourceNotFoundException();
        }

        if (!apprenticeship.CheckParty(serviceParameters))
        {
            throw new UnauthorizedAccessException($"You do not permissions to access apprenticeship {request.ApprenticeshipId}");
        }

        var priceEpisodesResponseTask = apiClient.GetWithResponseCode<GetPriceEpisodesResponse>(new GetPriceEpisodesRequest(apprenticeship.Id));
        var apprenticeshipUpdatesResponseTask = apiClient.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(new GetApprenticeshipUpdatesRequest(apprenticeship.Id, ApprenticeshipUpdateStatus.Pending));
        var apprenticeshipDataLockStatusResponseTask = apiClient.GetWithResponseCode<GetDataLocksResponse>(new GetDataLocksRequest(apprenticeship.Id));
        var changeOfPartyRequestsResponseTask = apiClient.GetWithResponseCode<GetChangeOfPartyRequestsResponse>(new GetChangeOfPartyRequestsRequest(apprenticeship.Id));
        var changeOfProviderChainResponseTask = apiClient.GetWithResponseCode<GetChangeOfProviderChainResponse>(new GetChangeOfProviderChainRequest(apprenticeship.Id));
        var changeOfEmployerChainResponseTask = apiClient.GetWithResponseCode<GetChangeOfEmployerChainResponse>(new GetChangeOfEmployerChainRequest(apprenticeship.Id));
        var overlappingTrainingDateResponseTask = apiClient.GetWithResponseCode<GetOverlappingTrainingDateResponse>(new GetOverlappingTrainingDateRequest(apprenticeship.Id));
        var deliveryModelTask = deliveryModelService.GetDeliveryModels(apprenticeship.ProviderId, apprenticeship.CourseCode, apprenticeship.AccountLegalEntityId, apprenticeship.ContinuationOfId);
        var canActualStartDateBeChangedTask = CanActualStartDateBeChanged(apprenticeship.ActualStartDate);

        await Task.WhenAll(
            priceEpisodesResponseTask,
            apprenticeshipUpdatesResponseTask,
            apprenticeshipDataLockStatusResponseTask,
            changeOfPartyRequestsResponseTask,
            changeOfProviderChainResponseTask,
            changeOfEmployerChainResponseTask,
            overlappingTrainingDateResponseTask,
            deliveryModelTask,
            canActualStartDateBeChangedTask
        );

        var priceEpisodesResponse = priceEpisodesResponseTask.Result;
        var apprenticeshipUpdatesResponse = apprenticeshipUpdatesResponseTask.Result;
        var apprenticeshipDataLockStatusResponse = apprenticeshipDataLockStatusResponseTask.Result;
        var changeOfPartyRequestsResponse = changeOfPartyRequestsResponseTask.Result;
        var changeOfProviderChainResponse = changeOfProviderChainResponseTask.Result;
        var changeOfEmployerChainResponse = changeOfEmployerChainResponseTask.Result;
        var overlappingTrainingDateResponse = overlappingTrainingDateResponseTask.Result;
        var deliveryModel = deliveryModelTask.Result;
        var canActualStartDateBeChanged = canActualStartDateBeChangedTask.Result;

        var result = new GetManageApprenticeshipDetailsQueryResult();
        
        result.Apprenticeship = apprenticeship;
        result.PriceEpisodes = priceEpisodesResponse.Body?.PriceEpisodes;
        result.ApprenticeshipUpdates = apprenticeshipUpdatesResponse.Body?.ApprenticeshipUpdates;
        result.DataLocks = apprenticeshipDataLockStatusResponse.Body?.DataLocks;
        result.ChangeOfPartyRequests = changeOfPartyRequestsResponse.Body?.ChangeOfPartyRequests;
        result.ChangeOfProviderChain = changeOfProviderChainResponse.Body?.ChangeOfProviderChain;
        result.ChangeOfEmployerChain = changeOfEmployerChainResponse.Body?.ChangeOfEmployerChain;
        result.OverlappingTrainingDateRequest = overlappingTrainingDateResponse.Body?.OverlappingTrainingDateRequest;
        result.HasMultipleDeliveryModelOptions = deliveryModel?.Count > 1;
        result.CanActualStartDateBeChanged = canActualStartDateBeChanged;
        result.PendingPriceChange = null;
        result.PendingStartDateChange = null;
        result.PaymentsStatus = new PaymentsStatus { PaymentsFrozen = false };
        result.LearnerStatusDetails = new LearnerStatusDetails { LearnerStatus = LearnerStatus.None };

        return result;
    }

    private async Task<bool?> CanActualStartDateBeChanged(DateTime? actualStartDate)
    {
        if (actualStartDate == null)
        {
            return null;
        }

        var fundingQualifyingPeriodEnd = actualStartDate.Value.AddDays(Constants.QualifyingPeriod + 1).AddTicks(-1);
        if (fundingQualifyingPeriodEnd < DateTime.Now)
        {
            return false;
        }

        var currentAcademicYear = await collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByDateRequest(DateTime.Now));
        var isStartDateAfterStartOfCurrentAcademicYear = currentAcademicYear.StartDate <= actualStartDate;
        if (isStartDateAfterStartOfCurrentAcademicYear)
        {
            return true;
        }

        var previousAcademicYear = await collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByDateRequest(DateTime.Now.AddYears(-1)));
        if (!previousAcademicYear.HardCloseDate.HasValue)
        {
            throw new AcademicYearDataIncompleteException(PreviousOrCurrentAcademicYear.Previous);
        }

        var isStartDateInPreviousAcademicYear = previousAcademicYear.StartDate <= actualStartDate; 
        var isItR13R14PeriodOfPreviousAcademicYear = previousAcademicYear.HardCloseDate > DateTime.Now;
        if (isStartDateInPreviousAcademicYear && isItR13R14PeriodOfPreviousAcademicYear)
        {
            return true;
        }

        return false;
    }
}