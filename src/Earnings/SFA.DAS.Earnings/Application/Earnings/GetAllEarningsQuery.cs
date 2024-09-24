using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.Earnings.Application.Extensions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Reflection;
using Apprenticeship = SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Apprenticeship;
using Episode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Episode;
using EarningsApprenticeship = SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship;
using EarningsEpisode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.Episode;
using Azure.Core;
using SFA.DAS.Apprenticeships.Types;

namespace SFA.DAS.Earnings.Application.Earnings;

public class GetAllEarningsQuery : IRequest<GetAllEarningsQueryResult>
{
    public long Ukprn { get; set; }
    public byte CollectionPeriod { get; set; }
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

        var apprenticeshipsData = await _apprenticeshipsApiClient.Get<GetApprenticeshipsResponse>(new GetApprenticeshipsRequest { Ukprn = request.Ukprn });
        var earningsData = await _earningsApiClient.Get<GetFm36DataResponse>(new GetFm36DataRequest(request.Ukprn));
        var currentAcademicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearsRequest(DateTime.Now));

        _logger.LogInformation("Found {apprenticeshipsCount} apprenticeships, {earningsApprenticeshipsCount} earnings apprenticeships, for provider {ukprn}", apprenticeshipsData.Apprenticeships.Count, earningsData.Count, request.Ukprn);

        var result = new GetAllEarningsQueryResult
        {

            FM36Learners = apprenticeshipsData.Apprenticeships
                .Join(earningsData, a => a.Key, e => e.Key, (apprenticeship,earningsApprenticeship) => new JoinedEarningsApprenticeship (apprenticeship, earningsApprenticeship))
                .Select(model => {
                    var priceEpisodesForAcademicYear = GetApprenticeshipPriceEpisodesForAcademicYearStarting(model.Apprenticeship.Episodes, currentAcademicYear.StartDate).ToList();
                    return new FM36Learner
                    {
                        ULN = long.Parse(model.Apprenticeship.Uln),
                        LearnRefNumber = EarningsFM36Constants.LearnRefNumber,
                        EarningsPlatform = SimplificationEarningsPlatform,
                        PriceEpisodes = priceEpisodesForAcademicYear
                            .Join(model.EarningsApprenticeship.Episodes, a => a.Episode.Key, b => b.Key, (episodePrice, earningsEpisode) => new JoinedPriceEpisodeModel(episodePrice.Episode, episodePrice.Price, earningsEpisode ) )
                            .Select(priceEpisodeModel => new PriceEpisode
                            {
                                PriceEpisodeIdentifier = $"{EarningsFM36Constants.ProgType}-{priceEpisodeModel.ApprenticeshipEpisode.TrainingCode.Trim()}-{priceEpisodeModel.ApprenticeshipEpisodePrice.StartDate:dd/MM/yyyy}",
                                PriceEpisodeValues = model.GetPriceEpisodeValues(priceEpisodeModel, priceEpisodesForAcademicYear, currentAcademicYear, request.CollectionPeriod),
                                PriceEpisodePeriodisedValues = model.GetPriceEpisodePeriodisedValues(currentAcademicYear)
                            }).ToList(),
                        LearningDeliveries = new List<LearningDelivery>
                        {
                            new LearningDelivery
                            {
                                AimSeqNumber = 1,
                                LearningDeliveryValues = model.GetLearningDelivery(currentAcademicYear),
                                LearningDeliveryPeriodisedValues = model.GetLearningDeliveryPeriodisedValues(currentAcademicYear),
                                LearningDeliveryPeriodisedTextValues = model.GetLearningDeliveryPeriodisedTextValues()
                            }
                        }
                    };
                }).ToArray()
        };

        return result;
    }

    private static IEnumerable<(Episode Episode, EpisodePrice Price)> GetApprenticeshipPriceEpisodesForAcademicYearStarting(List<Episode> apprenticeshipEpisodes, DateTime academicYearStartDate)
    {
        foreach (var episodePrice in apprenticeshipEpisodes
                     .SelectMany(episode => episode.Prices.Select(price => (Episode: episode, Price: price))))
        {
            if (episodePrice.Price.StartDate < academicYearStartDate)
            {
                //Price started before the current academic year, and so we need to create a price episode from the start of the current AY to satisfy the way the fm36 expects PriceEpisodes to be split by AY
                var price = new EpisodePrice
                {
                    StartDate = academicYearStartDate,
                    EndDate = episodePrice.Price.EndDate,
                    EndPointAssessmentPrice = episodePrice.Price.EndPointAssessmentPrice,
                    FundingBandMaximum = episodePrice.Price.FundingBandMaximum,
                    TotalPrice = episodePrice.Price.TotalPrice,
                    TrainingPrice = episodePrice.Price.TrainingPrice
                };

                yield return new ValueTuple<Episode, EpisodePrice>(episodePrice.Episode, price);
            }
            else
            {
                //Otherwise the price started during this AY so is valid to be included in the fm36 as is
                yield return episodePrice;
            }
        }
    }

}