﻿using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using static SFA.DAS.ToolsSupport.InnerApi.Responses.GetApprenticeshipPriceEpisodesResponse;

namespace SFA.DAS.ToolsSupport.Application.Queries;

public class GetApprenticeshipQueryHandler(IInternalApiClient<CommitmentsV2ApiConfiguration> client)
    : IRequestHandler<GetApprenticeshipQuery, GetApprenticeshipQueryResult?>
{
    public async Task<GetApprenticeshipQueryResult?> Handle(GetApprenticeshipQuery request, CancellationToken cancellationToken)
    {
        var apprenticeship = await client.Get<SupportApprenticeshipDetails?>(new GetApprovedApprenticeshipByIdRequest(request.Id));
        if(apprenticeship == null)
            return null;

        var changeOfProviderTask = client.Get<GetApprenticeshipChangeOfProviderChainResponse>(new GetApprenticeshipChangeOfProviderChainRequest(request.Id));
        var pendingUpdatesTask = client.Get<GetApprenticeshipPendingUpdatesResponse>(new GetApprenticeshipPendingUpdatesRequest(request.Id));
        var trainingDateTask = client.Get<GetApprenticeshipOverlappingTrainingDateResponse>(new GetApprenticeshipOverlappingTrainingDateRequest(request.Id));
        var priceEpisodeTask = client.Get<GetApprenticeshipPriceEpisodesResponse>(new GetApprenticeshipPriceEpisodeRequest(request.Id));

        await Task.WhenAll(changeOfProviderTask, pendingUpdatesTask, trainingDateTask, priceEpisodeTask);

        var changeOfProvider = await changeOfProviderTask;
        var pendingUpdates = await pendingUpdatesTask;
        var trainingDate = await trainingDateTask;
        var priceEpisode = await priceEpisodeTask;

        var (paymentStatusText, paymentStatusTagColour) = MapPaymentStatus(apprenticeship.PaymentStatus, apprenticeship.StartDate);

        return new GetApprenticeshipQueryResult
        {
            AgreementStatus = apprenticeship.AgreementStatus.GetDescription(),
            PaymentStatus = paymentStatusText,
            MadeRedundant = apprenticeship.MadeRedundant,
            CompletionDate = apprenticeship.PaymentStatus == PaymentStatus.Completed ? apprenticeship.CompletionDate : null,
            StopDate = apprenticeship.PaymentStatus == PaymentStatus.Withdrawn ? apprenticeship.StopDate : null,
            PauseDate = apprenticeship.PaymentStatus == PaymentStatus.Paused ? apprenticeship.PauseDate : null,
            Uln = apprenticeship.Uln,
            Email = apprenticeship.Email,
            TrainingProvider = apprenticeship.ProviderName,
            FirstName = apprenticeship.FirstName,
            LastName = apprenticeship.LastName,
            DateOfBirth = apprenticeship.DateOfBirth,
            CohortReference = apprenticeship.CohortReference,
            EmployerReference = apprenticeship.EmployerRef,
            LegalEntity = apprenticeship.EmployerName,
            UKPRN = apprenticeship.ProviderId,
            Trainingcourse = apprenticeship.CourseName,
            Version = apprenticeship.TrainingCourseVersionConfirmed ? apprenticeship.TrainingCourseVersion : null,
            Option = apprenticeship.TrainingCourseOption,
            ApprenticeshipCode = apprenticeship.CourseCode,
            ConfirmationStatusDescription = apprenticeship.ConfirmationStatus == null ? apprenticeship.ConfirmationStatus.ToString() : null,
            TrainingStartDate = apprenticeship.StartDate,
            TrainingEndDate = apprenticeship.EndDate,
            OverlappingTrainingDateRequestCreatedOn = MapToOverlappingTrainingDateRequest(trainingDate),
            TrainingCost = GetPrice(priceEpisode.PriceEpisodes, apprenticeship.Cost),
            ApprenticeshipUpdate = MapToApprenticeshipUpdate(pendingUpdates),
            ChangeOfProviderChain = changeOfProvider.ChangeOfProviderChain
        };
    }

    public static (string paymentStatusText, string paymentStatusTagColour) MapPaymentStatus(PaymentStatus paymentStatus, DateTime? startDate)
    {
        var isStartDateInFuture = startDate.HasValue && startDate.Value > new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        switch (paymentStatus)
        {
            case PaymentStatus.Active:
                return isStartDateInFuture ? ("Waiting to start", "") : ("Live", "blue");

            case PaymentStatus.Paused:
                return ("Paused", "grey");

            case PaymentStatus.Withdrawn:
                return ("Stopped", "red");

            case PaymentStatus.Completed:
                return ("Completed", "green");

            default:
                return (string.Empty, string.Empty);
        }
    }

    public DateTime? MapToOverlappingTrainingDateRequest(GetApprenticeshipOverlappingTrainingDateResponse overlappingTrainingDateResponse)
    {
        if (overlappingTrainingDateResponse?.OverlappingTrainingDateRequest == null)
            return null;

        var pendingRequest =
            overlappingTrainingDateResponse.OverlappingTrainingDateRequest.FirstOrDefault(x =>
                x.Status == OverlappingTrainingDateRequestStatus.Pending);

        if (pendingRequest == null)
        {
            return null;
        }

        return pendingRequest.CreatedOn;
    }

    public ApprenticeshipUpdate? MapToApprenticeshipUpdate(GetApprenticeshipPendingUpdatesResponse? pendingUpdateResponse)
    {
        var updateCount = pendingUpdateResponse?.ApprenticeshipUpdates?.Count;
        if (updateCount == null || updateCount == 0)
        {
            return null;
        }
        return pendingUpdateResponse.ApprenticeshipUpdates.ToList()[0];
    }

    public decimal? GetPrice(IEnumerable<PriceEpisode> priceEpisodes, decimal? cost)
    {
        if(priceEpisodes != null && priceEpisodes.Any())
            return GetPrice(priceEpisodes, DateTime.UtcNow);
        return cost;
    }

    private decimal GetPrice(IEnumerable<PriceEpisode> priceEpisodes, DateTime effectiveDate)
    {
        var episodes = priceEpisodes
            .OrderByDescending(x => x.FromDate)
            .ToList();

        var episode = episodes.FirstOrDefault(x =>
            x.FromDate <= effectiveDate && (x.ToDate == null || x.ToDate >= effectiveDate));

        return episode?.Cost ?? episodes.First().Cost;
    }
}