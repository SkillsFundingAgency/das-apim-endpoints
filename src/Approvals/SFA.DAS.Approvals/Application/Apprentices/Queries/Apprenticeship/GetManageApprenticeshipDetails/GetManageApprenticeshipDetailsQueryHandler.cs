using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apprenticeships.Types;
using SFA.DAS.Approvals.Enums;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetPendingPriceChange;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using GetApprenticeshipKeyRequest = SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetApprenticeshipKey.GetApprenticeshipKeyRequest;
using GetApprenticeshipUpdatesResponse = SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetApprenticeshipUpdatesResponse;
using GetPendingPriceChangeRequest = SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetPendingPriceChange.GetPendingPriceChangeRequest;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails;

public class GetManageApprenticeshipDetailsQueryHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
    IDeliveryModelService deliveryModelService,
    ServiceParameters serviceParameters,
    IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apprenticeshipsApiClient,
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

        var apprenticeshipKey = await apprenticeshipsApiClient.GetWithResponseCode<Guid>(new GetApprenticeshipKeyRequest(request.ApprenticeshipId));

        var priceEpisodesResponseTask = apiClient.GetWithResponseCode<GetPriceEpisodesResponse>(new GetPriceEpisodesRequest(apprenticeship.Id));
        var apprenticeshipUpdatesResponseTask = apiClient.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(new GetApprenticeshipUpdatesRequest(apprenticeship.Id, ApprenticeshipUpdateStatus.Pending));
        var apprenticeshipDataLockStatusResponseTask = apiClient.GetWithResponseCode<GetDataLocksResponse>(new GetDataLocksRequest(apprenticeship.Id));
        var changeOfPartyRequestsResponseTask = apiClient.GetWithResponseCode<GetChangeOfPartyRequestsResponse>(new GetChangeOfPartyRequestsRequest(apprenticeship.Id));
        var changeOfProviderChainResponseTask = apiClient.GetWithResponseCode<GetChangeOfProviderChainResponse>(new GetChangeOfProviderChainRequest(apprenticeship.Id));
        var changeOfEmployerChainResponseTask = apiClient.GetWithResponseCode<GetChangeOfEmployerChainResponse>(new GetChangeOfEmployerChainRequest(apprenticeship.Id));
        var overlappingTrainingDateResponseTask = apiClient.GetWithResponseCode<GetOverlappingTrainingDateResponse>(new GetOverlappingTrainingDateRequest(apprenticeship.Id));
        var deliveryModelTask = deliveryModelService.GetDeliveryModels(apprenticeship.ProviderId, apprenticeship.CourseCode, apprenticeship.AccountLegalEntityId, apprenticeship.ContinuationOfId);
        var pendingPriceChangeTask = apprenticeshipsApiClient.GetWithResponseCode<GetPendingPriceChangeResponse>(new GetPendingPriceChangeRequest(apprenticeshipKey.Body));
        var canActualStartDateBeChangedTask = CanActualStartDateBeChanged(apprenticeship.ActualStartDate);
        var pendingStartDateChangeTask = apprenticeshipsApiClient.GetWithResponseCode<GetPendingStartDateChangeApiResponse>(new GetPendingStartDateChangeRequest(apprenticeshipKey.Body));
        var paymentStatusTask = apprenticeshipsApiClient.GetWithResponseCode<GetPaymentStatusApiResponse>(new GetPaymentStatusRequest(apprenticeshipKey.Body));
        var learnerStatusTask = apprenticeshipsApiClient.GetWithResponseCode<GetLearnerStatusResponse>(new GetLearnerStatusRequest(apprenticeshipKey.Body));

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
            pendingStartDateChangeTask,
            paymentStatusTask,
            learnerStatusTask);

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
        var paymentStatusResponse = paymentStatusTask.Result;
        var learnerStatusResponse = learnerStatusTask.Result;

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
        result.PendingPriceChange = ToResponse(pendingPriceChangeResponse.Body);
        result.CanActualStartDateBeChanged = canActualStartDateBeChanged;
        result.PendingStartDateChange = ToResponse(pendingStartDateResponse.Body);
        result.PaymentsStatus = ToResponse(paymentStatusResponse.Body);
        result.LearnerStatusDetails = ToResponse(learnerStatusResponse.Body);

        return result;
    }

    private static PendingPriceChange ToResponse(GetPendingPriceChangeResponse pendingPriceChangeResponse)
    {
        if (pendingPriceChangeResponse == null || !pendingPriceChangeResponse.HasPendingPriceChange)
        {
            return null;
        }

        var pendingPriceChange = new PendingPriceChange();
           
        pendingPriceChange.Cost = pendingPriceChangeResponse.PendingPriceChange.PendingTotalPrice;
        pendingPriceChange.EndPointAssessmentPrice = pendingPriceChangeResponse.PendingPriceChange.PendingAssessmentPrice;
        pendingPriceChange.TrainingPrice = pendingPriceChangeResponse.PendingPriceChange.PendingTrainingPrice;
        pendingPriceChange.ProviderApprovedDate = pendingPriceChangeResponse.PendingPriceChange.ProviderApprovedDate;
        pendingPriceChange.EmployerApprovedDate = pendingPriceChangeResponse.PendingPriceChange.EmployerApprovedDate;
        pendingPriceChange.Initiator = pendingPriceChangeResponse.PendingPriceChange.Initiator;

        return pendingPriceChange;
    }

    private static PendingStartDateChange ToResponse(GetPendingStartDateChangeApiResponse pendingStartDateChangeResponse)
    {
        if (pendingStartDateChangeResponse == null || !pendingStartDateChangeResponse.HasPendingStartDateChange) return null;

        var pendingStartDateChange = new PendingStartDateChange();
        
        pendingStartDateChange.PendingActualStartDate = pendingStartDateChangeResponse.PendingStartDateChange.PendingActualStartDate;
        pendingStartDateChange.PendingPlannedEndDate = pendingStartDateChangeResponse.PendingStartDateChange.PendingPlannedEndDate;
        pendingStartDateChange.ProviderApprovedDate = pendingStartDateChangeResponse.PendingStartDateChange.ProviderApprovedDate;
        pendingStartDateChange.EmployerApprovedDate = pendingStartDateChangeResponse.PendingStartDateChange.EmployerApprovedDate;
        pendingStartDateChange.Initiator = pendingStartDateChangeResponse.PendingStartDateChange.Initiator;

        return pendingStartDateChange;
    }

    private PaymentsStatus ToResponse(GetPaymentStatusApiResponse source)
    {
        if (source == null) return new PaymentsStatus { PaymentsFrozen = false };

        return new PaymentsStatus
        {
            FrozenOn = source.FrozenOn,
            PaymentsFrozen = source.PaymentsFrozen,
            ReasonFrozen = source.ReasonFrozen
        };
    }

    private LearnerStatusDetails ToResponse(GetLearnerStatusResponse source)
    {
        if (source.LearnerStatus == null) return new LearnerStatusDetails{ LearnerStatus = LearnerStatus.None };

        return new LearnerStatusDetails
        {
            LearnerStatus = source.LearnerStatus.Value,
            WithdrawalChangedDate = source.WithdrawalChangedDate,
            WithdrawalReason = source.WithdrawalReason,
            LastCensusDateOfLearning = source.LastCensusDateOfLearning
        };
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