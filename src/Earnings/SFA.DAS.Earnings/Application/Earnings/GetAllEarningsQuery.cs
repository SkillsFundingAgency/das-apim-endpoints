﻿using System.Reflection.Metadata.Ecma335;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Earnings.Helpers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Earnings.Application.Earnings;

public class GetAllEarningsQuery : IRequest<GetAllEarningsQueryResult>
{
    public GetAllEarningsQuery(long ukprn, int collectionYear, byte collectionPeriod)
    {
        Ukprn = ukprn;
        CollectionYear = collectionYear;
        CollectionPeriod = collectionPeriod;
    }

    public long Ukprn { get; }
    public int CollectionYear { get; }
    public byte CollectionPeriod { get; }
}

public class GetAllEarningsQueryResult
{
    public FM36Learner[] FM36Learners { get; set; }
}

public class GetAllEarningsQueryHandler : IRequestHandler<GetAllEarningsQuery, GetAllEarningsQueryResult>
{
    const int SimplificationEarningsPlatform = 2;

    private readonly IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> _apprenticeshipsApiClient;
    private readonly IEarningsApiClient<EarningsApiConfiguration> _earningsApiClient;
    private readonly ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> _collectionCalendarApiClient;
    private readonly ILogger<GetAllEarningsQueryHandler> _logger;

    public GetAllEarningsQueryHandler(IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apprenticeshipsApiClient,
        IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
        ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient,
        ILogger<GetAllEarningsQueryHandler> logger)
    {
        _apprenticeshipsApiClient = apprenticeshipsApiClient;
        _earningsApiClient = earningsApiClient;
        _collectionCalendarApiClient = collectionCalendarApiClient;
        _logger = logger;
    }

    public async Task<GetAllEarningsQueryResult> Handle(GetAllEarningsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllEarningsQuery for provider {ukprn}", request.Ukprn);

        _apprenticeshipsApiClient.GenerateServiceToken("Earnings");
        
        var apprenticeshipsDataTask = _apprenticeshipsApiClient.Get<GetApprenticeshipsResponse>(new GetApprenticeshipsRequest { Ukprn = request.Ukprn });
        var earningsDataTask = _earningsApiClient.Get<GetFm36DataResponse>(new GetFm36DataRequest(request.Ukprn));
        var currentAcademicYearTask = _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByYearRequest(request.CollectionYear));

        await Task.WhenAll(apprenticeshipsDataTask, earningsDataTask, currentAcademicYearTask);

        var apprenticeshipsData = apprenticeshipsDataTask.Result;
        var earningsData = earningsDataTask.Result;
        var currentAcademicYear = currentAcademicYearTask.Result;

        if (currentAcademicYear == null)
        {
            _logger.LogWarning($"Collection Calendar Api failed to return data on requested collection year {request.CollectionYear}. A calculated fall-back will be used instead.");
            currentAcademicYear = AcademicYearFallbackHelper.GetFallbackAcademicYearResponse(request.CollectionYear);
        }

        if (!IsDataReturnedValid(request.Ukprn, apprenticeshipsData, earningsData))
            return new GetAllEarningsQueryResult { FM36Learners = [] };

        _logger.LogInformation("Found {apprenticeshipsCount} apprenticeships, {earningsApprenticeshipsCount} earnings apprenticeships, for provider {ukprn}", apprenticeshipsData.Apprenticeships.Count, earningsData.Count, request.Ukprn);
        _logger.LogInformation($"Academic year {currentAcademicYear.AcademicYear}: {currentAcademicYear.StartDate:yyyy-MM-dd} - {currentAcademicYear.EndDate:yyyy-MM-dd}");

        var joinedApprenticeships = new List<JoinedEarningsApprenticeship>();

        foreach (var apprenticeship in apprenticeshipsData.Apprenticeships)
        {
            // Find matching entries in earningsData
            var matchingEarnings = earningsData.FirstOrDefault(e => e.Key == apprenticeship.Key);

            if (matchingEarnings != null)
            {
                _logger.LogInformation($"Processing apprenticeship with key: {apprenticeship.Key}");
                joinedApprenticeships.Add(new JoinedEarningsApprenticeship(apprenticeship, matchingEarnings, currentAcademicYear.GetShortAcademicYear()));
            }
        }

        var result = new GetAllEarningsQueryResult
        {
            FM36Learners = joinedApprenticeships
            .Select(joinedApprenticeship =>
            {
                var priceEpisodesForAcademicYear = GetPriceEpisodesByFm36StartAndFinishPeriods(joinedApprenticeship.Episodes, currentAcademicYear)
                    .ToList();

                //If there are no price episodes for the requested academic year, create one, using the values of the first actual episode
                if (priceEpisodesForAcademicYear.Count == 0)
                {
                    priceEpisodesForAcademicYear =
                    [
                        new JoinedPriceEpisode(joinedApprenticeship.Episodes.First(), currentAcademicYear.StartDate, currentAcademicYear.EndDate, currentAcademicYear.GetShortAcademicYear(), true)
                    ];
                }

                return new FM36Learner
                {
                    ULN = long.Parse(joinedApprenticeship.Uln),
                    LearnRefNumber = EarningsFM36Constants.LearnRefNumber,
                    EarningsPlatform = SimplificationEarningsPlatform,
                    PriceEpisodes = GetPriceEpisodes(joinedApprenticeship, priceEpisodesForAcademicYear, currentAcademicYear, request.CollectionPeriod),
                    LearningDeliveries =
                    [
                        new LearningDelivery
                        {
                            AimSeqNumber = 1,
                            LearningDeliveryValues = joinedApprenticeship.GetLearningDelivery(currentAcademicYear),
                            LearningDeliveryPeriodisedValues =
                                joinedApprenticeship.GetLearningDeliveryPeriodisedValues(currentAcademicYear),
                            LearningDeliveryPeriodisedTextValues =
                                joinedApprenticeship.GetLearningDeliveryPeriodisedTextValues()
                        }
                    ],
                    HistoricEarningOutputValues = new List<HistoricEarningOutputValues>()
                };
            })
            .ToArray()
        };

        return result;
    }

    private List<PriceEpisode> GetPriceEpisodes(
        JoinedEarningsApprenticeship joinedEarningsApprenticeship,
        List<JoinedPriceEpisode> priceEpisodesForAcademicYear,
        GetAcademicYearsResponse currentAcademicYear,
           byte collectionPeriod)
    {
        var lastPriceEpisode = priceEpisodesForAcademicYear.MaxBy(x => x.EndDate);

        return priceEpisodesForAcademicYear
            .Select(priceEpisodeModel => {

                var hasSubsequentPriceEpisodes = lastPriceEpisode != null && priceEpisodeModel.EndDate != lastPriceEpisode.EndDate;
                return new PriceEpisode
                {
                    PriceEpisodeIdentifier = $"{EarningsFM36Constants.ProgType}-{priceEpisodeModel.TrainingCode.Trim()}-{priceEpisodeModel.StartDate:dd/MM/yyyy}",

                    PriceEpisodeValues = joinedEarningsApprenticeship.GetPriceEpisodeValues(priceEpisodeModel, currentAcademicYear, collectionPeriod, hasSubsequentPriceEpisodes),
                    PriceEpisodePeriodisedValues = joinedEarningsApprenticeship.GetPriceEpisodePeriodisedValues(priceEpisodeModel, currentAcademicYear)
                };

            }).ToList();
    }

    private bool IsDataReturnedValid(long ukprn, GetApprenticeshipsResponse apprenticeshipsData, GetFm36DataResponse earningsData)
    {
        if(apprenticeshipsData == null || apprenticeshipsData.Apprenticeships == null || !apprenticeshipsData.Apprenticeships.Any())
        {
            _logger.LogWarning("No apprenticeships data returned for {ukprn}", ukprn);
            return false;
        }

        if(earningsData == null || !earningsData.Any())
        {
            _logger.LogWarning("No earnings data returned for {ukprn}", ukprn);
            return false;
        }

        return true;
    }

    // The fm36 expects PriceEpisodes to be split by academic year, so we need to split the price episodes by the academic year start and finish periods
    // the exception is where there is a price change within the academic year, in which case we need to split the price episodes by the start and finish periods of the price change
    private static IEnumerable<JoinedPriceEpisode> GetPriceEpisodesByFm36StartAndFinishPeriods(List<JoinedPriceEpisode> priceEpisodes, GetAcademicYearsResponse academicYearDetails)
    {
        foreach (var episodePrice in priceEpisodes)
        {
            if (episodePrice.StartDate <= academicYearDetails.StartDate && episodePrice.EndDate >= academicYearDetails.StartDate)
            {
                // Some of the later instalments are in the current academic year, capture these as a price episode
                yield return new JoinedPriceEpisode(episodePrice, academicYearDetails.StartDate, episodePrice.EndDate, academicYearDetails.GetShortAcademicYear(), false);
            }
            else if(episodePrice.StartDate < academicYearDetails.EndDate && episodePrice.EndDate > academicYearDetails.EndDate)
            {
                // Some of the earlier instalments are in the current academic year, capture these as a price episode
                yield return new JoinedPriceEpisode(episodePrice, episodePrice.StartDate, academicYearDetails.EndDate, academicYearDetails.GetShortAcademicYear(), true);
            }
            else if (episodePrice.StartDate > academicYearDetails.EndDate || episodePrice.EndDate < academicYearDetails.StartDate)
            {
                // no part of the price episode is in the current academic year, do not return it
            }
            else
            {
                // start and end dates are within the current academic year, so capture the price episode as is
                yield return episodePrice;
            }
        }
    }
}