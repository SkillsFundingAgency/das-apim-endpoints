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
                            LearningDeliveryValues = new LearningDeliveryValues
                            {
                                ActualDaysIL = EarningsFM36Constants.ActualDaysIL,
                                AdjStartDate = model.Apprenticeship.StartDate,
                                AgeAtProgStart = model.Apprenticeship.AgeAtStartOfApprenticeship,
                                AppAdjLearnStartDate = model.Apprenticeship.StartDate,
                                AppAdjLearnStartDateMatchPathway = model.Apprenticeship.StartDate,
                                ApplicCompDate = EarningsFM36Constants.ApplicCompDate,
                                CombinedAdjProp = EarningsFM36Constants.CombinedAdjProp,
                                Completed = EarningsFM36Constants.Completed,
                                FundStart = EarningsFM36Constants.FundStart,
                                LDApplic1618FrameworkUpliftTotalActEarnings = EarningsFM36Constants.LDApplic1618FrameworkUpliftTotalActEarnings,
                                LearnAimRef = EarningsFM36Constants.LearnAimRef,
                                LearnStartDate = model.Apprenticeship.StartDate,
                                LearnDel1618AtStart = model.Apprenticeship.AgeAtStartOfApprenticeship < 19,
                                LearnDelAppAccDaysIL = 1 + ((model.Apprenticeship.PlannedEndDate < currentAcademicYear.EndDate
                                    ? model.Apprenticeship.PlannedEndDate
                                    : currentAcademicYear.EndDate) - model.Apprenticeship.StartDate).Days,
                                LearnDelApplicDisadvAmount = EarningsFM36Constants.LearnDelApplicDisadvAmount,
                                LearnDelApplicEmp1618Incentive = EarningsFM36Constants.LearnDelApplicEmp1618Incentive,
                                LearnDelApplicProv1618FrameworkUplift = EarningsFM36Constants.LearnDelApplicProv1618FrameworkUplift,
                                LearnDelApplicProv1618Incentive = EarningsFM36Constants.LearnDelApplicProv1618Incentive,
                                LearnDelAppPrevAccDaysIL = 
                                    1 + ((model.Apprenticeship.PlannedEndDate < currentAcademicYear.EndDate
                                        ? model.Apprenticeship.PlannedEndDate
                                        : currentAcademicYear.EndDate)
                                    - (model.Apprenticeship.StartDate > currentAcademicYear.StartDate
                                        ? model.Apprenticeship.StartDate
                                        : currentAcademicYear.StartDate)).Days,
                                LearnDelDisadAmount = EarningsFM36Constants.LearnDelDisadAmount,
                                LearnDelEligDisadvPayment = EarningsFM36Constants.LearnDelEligDisadvPayment,
                                LearnDelEmpIdFirstAdditionalPaymentThreshold = EarningsFM36Constants.LearnDelEmpIdFirstAdditionalPaymentThreshold,
                                LearnDelEmpIdSecondAdditionalPaymentThreshold = EarningsFM36Constants.LearnDelEmpIdSecondAdditionalPaymentThreshold,
                                LearnDelHistDaysThisApp = (currentAcademicYear.EndDate - model.Apprenticeship.StartDate).Days,
                                LearnDelHistProgEarnings = model.EarningsApprenticeship.Episodes
                                    .SelectMany(episode => episode.Instalments)
                                    .Sum(instalment => instalment.Amount),
                                LearnDelInitialFundLineType = model.EarningsApprenticeship.FundingLineType,
                                LearnDelMathEng = EarningsFM36Constants.LearnDelMathEng,
                                LearnDelProgEarliestACT2Date = EarningsFM36Constants.LearnDelProgEarliestACT2Date,
                                LearnDelNonLevyProcured = EarningsFM36Constants.LearnDelNonLevyProcured,
                                MathEngAimValue = EarningsFM36Constants.MathEngAimValue,
                                OutstandNumOnProgInstalm = EarningsFM36Constants.OutstandNumOnProgInstalm,
                                PlannedNumOnProgInstalm = model.Apprenticeship.StartDate.GetNumberOfIncludedCensusDatesUntil(model.Apprenticeship.PlannedEndDate),
                                PlannedTotalDaysIL = 1 + (model.Apprenticeship.PlannedEndDate - model.Apprenticeship.StartDate).Days,
                                ProgType = EarningsFM36Constants.ProgType,
                                PwayCode = EarningsFM36Constants.PwayCode,
                                SecondIncentiveThresholdDate = EarningsFM36Constants.SecondIncentiveThresholdDate,
                                StdCode = int.TryParse(model.Apprenticeship.Episodes.MinBy(x => x.Prices.Min(price => price.StartDate))?.TrainingCode, out int parsedTrainingCode) ? parsedTrainingCode : null,
                                ThresholdDays = EarningsFM36Constants.ThresholdDays,
                                LearnDelApplicCareLeaverIncentive = EarningsFM36Constants.LearnDelApplicCareLeaverIncentive,
                                LearnDelHistDaysCareLeavers = EarningsFM36Constants.LearnDelHistDaysCareLeavers,
                                LearnDelAccDaysILCareLeavers = EarningsFM36Constants.LearnDelAccDaysILCareLeavers,
                                LearnDelPrevAccDaysILCareLeavers = EarningsFM36Constants.LearnDelPrevAccDaysILCareLeavers,
                                LearnDelLearnerAddPayThresholdDate = EarningsFM36Constants.LearnDelLearnerAddPayThresholdDate,
                                LearnDelRedCode = EarningsFM36Constants.LearnDelRedCode,
                                LearnDelRedStartDate = EarningsFM36Constants.LearnDelRedStartDate
                            },
                            LearningDeliveryPeriodisedValues = new List<LearningDeliveryPeriodisedValues>
                            {
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.DisadvFirstPayment, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.DisadvSecondPayment, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildInstPerPeriodValues(model.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear()),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LDApplic1618FrameworkUpliftBalancingPayment, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LDApplic1618FrameworkUpliftCompletionPayment, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LDApplic1618FrameworkUpliftOnProgPayment, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelFirstEmp1618Pay, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelFirstProv1618Pay, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelLearnAddPayment, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelLevyNonPayInd, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelSecondEmp1618Pay, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelSecondProv1618Pay, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelSEMContWaiver, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelESFAContribPct, 0.95m),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnSuppFund, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnSuppFundCash, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.MathEngBalPayment, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.MathEngOnProgPayment, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimBalPayment, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimCompletionPayment, 0),
                                LearningDeliveryPeriodisedValuesBuilder.BuildInstallmentAmountValues(
                                    model.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimOnProgPayment),
                                LearningDeliveryPeriodisedValuesBuilder.BuildCoInvestmentValues(
                                    model.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimProgFundIndMaxEmpCont, EarningsFM36Constants.CoInvestEmployerMultiplier),
                                LearningDeliveryPeriodisedValuesBuilder.BuildCoInvestmentValues(
                                    model.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimProgFundIndMinCoInvest, EarningsFM36Constants.CoInvestSfaMultiplier),
                                LearningDeliveryPeriodisedValuesBuilder.BuildInstallmentAmountValues(
                                    model.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimTotProgFund)
                            },
                            LearningDeliveryPeriodisedTextValues = new List<LearningDeliveryPeriodisedTextValues>
                            {
                                LearningDeliveryPeriodisedTextValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.FundLineType, model.EarningsApprenticeship.FundingLineType),
                                LearningDeliveryPeriodisedTextValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelContType, EarningsFM36Constants.LearnDelContType)
                            }
                        }
                    }
                    };}).ToArray()
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