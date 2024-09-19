using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using MediatR;
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
using Apprenticeship = SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Apprenticeship;
using Episode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Episode;


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
                .Join(earningsData, a => a.Key, e => e.Key, (apprenticeship,earningsApprenticeship) => new { apprenticeship, earningsApprenticeship })
                .Select(model => new FM36Learner
            {
                ULN = long.Parse(model.apprenticeship.Uln),
                LearnRefNumber = EarningsFM36Constants.LearnRefNumber,
                PriceEpisodes = GetApprenticeshipPriceEpisodesForAcademicYearStarting(model.apprenticeship.Episodes, currentAcademicYear.StartDate)
                    .Join(model.earningsApprenticeship.Episodes, a => a.Episode.Key, b => b.Key, (episodePrice, earningsEpisode) => new{ episodePrice, earningsEpisode } )
                    .Select(priceEpisodeModel => new PriceEpisode
                    {
                        PriceEpisodeIdentifier = $"{EarningsFM36Constants.ProgType}-{priceEpisodeModel.episodePrice.Episode.TrainingCode.Trim()}-{priceEpisodeModel.episodePrice.Price.StartDate:dd/MM/yyyy}",
                        PriceEpisodeValues = new PriceEpisodeValues
                        {
                            EpisodeStartDate = priceEpisodeModel.episodePrice.Price.StartDate,
                            TNP1 = priceEpisodeModel.episodePrice.Price.TrainingPrice,
                            TNP2 = priceEpisodeModel.episodePrice.Price.EndPointAssessmentPrice,
                            TNP3 = EarningsFM36Constants.TNP3,
                            TNP4 = EarningsFM36Constants.TNP4,
                            PriceEpisodeActualEndDateIncEPA = EarningsFM36Constants.PriceEpisodeActualEndDateIncEPA,
                            PriceEpisode1618FUBalValue = EarningsFM36Constants.PriceEpisode1618FUBalValue,
                            PriceEpisodeApplic1618FrameworkUpliftCompElement = EarningsFM36Constants.PriceEpisodeApplic1618FrameworkUpliftCompElement,
                            PriceEpisode1618FrameworkUpliftTotPrevEarnings = EarningsFM36Constants.PriceEpisode1618FrameworkUpliftTotPrevEarnings,
                            PriceEpisode1618FrameworkUpliftRemainingAmount = EarningsFM36Constants.PriceEpisode1618FrameworkUpliftRemainingAmount,
                            PriceEpisode1618FUMonthInstValue = EarningsFM36Constants.PriceEpisode1618FUMonthInstValue,
                            PriceEpisode1618FUTotEarnings = EarningsFM36Constants.PriceEpisode1618FUTotEarnings,
                            PriceEpisodeUpperBandLimit = priceEpisodeModel.episodePrice.Price.FundingBandMaximum,
                            PriceEpisodePlannedEndDate = model.apprenticeship.PlannedEndDate,
                            PriceEpisodeActualEndDate = priceEpisodeModel.episodePrice.Price.EndDate,
                            PriceEpisodeTotalTNPPrice = priceEpisodeModel.episodePrice.Price.TotalPrice,
                            PriceEpisodeUpperLimitAdjustment = EarningsFM36Constants.PriceEpisodeUpperLimitAdjustment,
                            PriceEpisodePlannedInstalments = priceEpisodeModel.episodePrice.Price.StartDate.GetNumberOfIncludedCensusDatesUntil(model.apprenticeship.PlannedEndDate),
                            PriceEpisodeActualInstalments = model.earningsApprenticeship.Episodes
                            .SelectMany(x => x.Instalments)
                            .Count(x => x.AcademicYear == short.Parse(currentAcademicYear.AcademicYear)),
                            PriceEpisodeInstalmentsThisPeriod = model.earningsApprenticeship.Episodes
                            .SelectMany(x => x.Instalments)
                            .Any(x => x.AcademicYear == short.Parse(currentAcademicYear.AcademicYear) && x.DeliveryPeriod == request.CollectionPeriod) ? 1 : 0,
                            PriceEpisodeCompletionElement = priceEpisodeModel.earningsEpisode.CompletionPayment,
                            PriceEpisodePreviousEarnings = EarningsFM36Constants.PriceEpisodePreviousEarnings,
                            PriceEpisodeInstalmentValue = priceEpisodeModel.earningsEpisode.Instalments.FirstOrDefault()?.Amount ?? 0,
                            PriceEpisodeOnProgPayment = EarningsFM36Constants.PriceEpisodeOnProgPayment,
                            PriceEpisodeTotalEarnings = priceEpisodeModel.earningsEpisode.Instalments.Sum(x => x.Amount),
                            PriceEpisodeBalanceValue = EarningsFM36Constants.PriceEpisodeBalanceValue,
                            PriceEpisodeBalancePayment = EarningsFM36Constants.PriceEpisodeBalancePayment,
                            PriceEpisodeCompleted = priceEpisodeModel.episodePrice.Price.EndDate < DateTime.Now,
                            PriceEpisodeCompletionPayment = EarningsFM36Constants.PriceEpisodeCompletionPayment,
                            PriceEpisodeAimSeqNumber = EarningsFM36Constants.AimSeqNumber,
                            PriceEpisodeFirstDisadvantagePayment = EarningsFM36Constants.PriceEpisodeFirstDisadvantagePayment,
                            PriceEpisodeSecondDisadvantagePayment = EarningsFM36Constants.PriceEpisodeSecondDisadvantagePayment,
                            PriceEpisodeApplic1618FrameworkUpliftBalancing = EarningsFM36Constants.PriceEpisodeApplic1618FrameworkUpliftBalancing,
                            PriceEpisodeApplic1618FrameworkUpliftCompletionPayment = EarningsFM36Constants.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment,
                            PriceEpisodeApplic1618FrameworkUpliftOnProgPayment = EarningsFM36Constants.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment,
                            PriceEpisodeSecondProv1618Pay = EarningsFM36Constants.PriceEpisodeSecondProv1618Pay,
                            PriceEpisodeFirstEmp1618Pay = EarningsFM36Constants.PriceEpisodeFirstEmp1618Pay,
                            PriceEpisodeSecondEmp1618Pay = EarningsFM36Constants.PriceEpisodeSecondEmp1618Pay,
                            PriceEpisodeFirstProv1618Pay = EarningsFM36Constants.PriceEpisodeFirstProv1618Pay,
                            PriceEpisodeLSFCash = EarningsFM36Constants.PriceEpisodeLSFCash,
                            PriceEpisodeFundLineType = model.earningsApprenticeship.FundingLineType,
                            PriceEpisodeLevyNonPayInd = EarningsFM36Constants.PriceEpisodeLevyNonPayInd,
                            EpisodeEffectiveTNPStartDate = priceEpisodeModel.episodePrice.Price.StartDate,
                            PriceEpisodeFirstAdditionalPaymentThresholdDate = EarningsFM36Constants.PriceEpisodeFirstAdditionalPaymentThresholdDate,
                            PriceEpisodeSecondAdditionalPaymentThresholdDate = EarningsFM36Constants.PriceEpisodeSecondAdditionalPaymentThresholdDate,
                            PriceEpisodeContractType = EarningsFM36Constants.PriceEpisodeContractType,
                            PriceEpisodePreviousEarningsSameProvider = EarningsFM36Constants.PriceEpisodePreviousEarningsSameProvider,
                            PriceEpisodeTotProgFunding = priceEpisodeModel.earningsEpisode.OnProgramTotal,
                            PriceEpisodeProgFundIndMinCoInvest = priceEpisodeModel.earningsEpisode.OnProgramTotal * EarningsFM36Constants.CoInvestSfaMultiplier,
                            PriceEpisodeProgFundIndMaxEmpCont = priceEpisodeModel.earningsEpisode.OnProgramTotal * EarningsFM36Constants.CoInvestEmployerMultiplier,
                            PriceEpisodeTotalPMRs = EarningsFM36Constants.PriceEpisodeTotalPMRs,
                            PriceEpisodeCumulativePMRs = EarningsFM36Constants.PriceEpisodeCumulativePMRs,
                            PriceEpisodeCompExemCode = EarningsFM36Constants.PriceEpisodeCompExemCode,
                            PriceEpisodeLearnerAdditionalPaymentThresholdDate = EarningsFM36Constants.PriceEpisodeLearnerAdditionalPaymentThresholdDate,
                            PriceEpisodeRedStartDate = EarningsFM36Constants.PriceEpisodeRedStartDate,
                            PriceEpisodeRedStatusCode = EarningsFM36Constants.PriceEpisodeRedStatusCode,
                            PriceEpisodeLDAppIdent = $"{EarningsFM36Constants.ProgType}-{priceEpisodeModel.episodePrice.Episode.TrainingCode.Trim()}",
                            PriceEpisodeAugmentedBandLimitFactor = EarningsFM36Constants.PriceEpisodeAugmentedBandLimitFactor,
                            PriceEpisodeRemainingTNPAmount = priceEpisodeModel.episodePrice.Price.FundingBandMaximum
                                                             - GetPreviousEarnings(model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), request.CollectionPeriod),
                            PriceEpisodeRemainingAmountWithinUpperLimit = priceEpisodeModel.episodePrice.Price.FundingBandMaximum
                                                                          - GetPreviousEarnings(model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), request.CollectionPeriod),
                            PriceEpisodeCappedRemainingTNPAmount = priceEpisodeModel.episodePrice.Price.FundingBandMaximum
                                                                   - GetPreviousEarnings(model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), request.CollectionPeriod),
                            PriceEpisodeExpectedTotalMonthlyValue = priceEpisodeModel.episodePrice.Price.FundingBandMaximum
                                                                    - GetPreviousEarnings(model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), request.CollectionPeriod)
                                                                    - priceEpisodeModel.earningsEpisode.CompletionPayment,
                        },
                        PriceEpisodePeriodisedValues = new List<PriceEpisodePeriodisedValues>()
                        {
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeApplic1618FrameworkUpliftBalancing, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeBalancePayment, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeBalanceValue, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeCompletionPayment, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeFirstDisadvantagePayment, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeFirstEmp1618Pay, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeFirstProv1618Pay, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeLevyNonPayInd, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeLSFCash, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeSecondDisadvantagePayment, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeSecondEmp1618Pay, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeSecondProv1618Pay, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeLearnerAdditionalPayment, 0),
                            PriceEpisodePeriodisedValuesBuilder.BuildPriceEpisodeInstalmentsThisPeriodValues(model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear()),
                            PriceEpisodePeriodisedValuesBuilder.BuildInstallmentAmountValues(model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeOnProgPayment),
                            PriceEpisodePeriodisedValuesBuilder.BuildCoInvestmentValues(model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeProgFundIndMaxEmpCont, EarningsFM36Constants.CoInvestEmployerMultiplier),
                            PriceEpisodePeriodisedValuesBuilder.BuildCoInvestmentValues(model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeProgFundIndMinCoInvest, EarningsFM36Constants.CoInvestSfaMultiplier),
                            PriceEpisodePeriodisedValuesBuilder.BuildInstallmentAmountValues(model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeTotProgFunding),
                        }
                    }).ToList(),
                LearningDeliveries = new List<LearningDelivery>
                {
                    new LearningDelivery
                    {
                        AimSeqNumber = 1,
                        LearningDeliveryValues = new LearningDeliveryValues
                        {
                            ActualDaysIL = EarningsFM36Constants.ActualDaysIL,
                            AdjStartDate = model.apprenticeship.StartDate,
                            AgeAtProgStart = model.apprenticeship.AgeAtStartOfApprenticeship,
                            AppAdjLearnStartDate = model.apprenticeship.StartDate,
                            ApplicCompDate = EarningsFM36Constants.ApplicCompDate,
                            CombinedAdjProp = EarningsFM36Constants.CombinedAdjProp,
                            Completed = EarningsFM36Constants.Completed,
                            FundStart = EarningsFM36Constants.FundStart,
                            LDApplic1618FrameworkUpliftTotalActEarnings = EarningsFM36Constants.LDApplic1618FrameworkUpliftTotalActEarnings,
                            LearnAimRef = EarningsFM36Constants.LearnAimRef,
                            LearnStartDate = model.apprenticeship.StartDate,
                            LearnDel1618AtStart = model.apprenticeship.AgeAtStartOfApprenticeship < 19,
                            LearnDelAppAccDaysIL = ((model.apprenticeship.PlannedEndDate < currentAcademicYear.EndDate
                                ? model.apprenticeship.PlannedEndDate
                                : currentAcademicYear.EndDate) - model.apprenticeship.StartDate).Days,
                            LearnDelApplicDisadvAmount = EarningsFM36Constants.LearnDelApplicDisadvAmount,
                            LearnDelApplicEmp1618Incentive = EarningsFM36Constants.LearnDelApplicEmp1618Incentive,
                            LearnDelApplicProv1618FrameworkUplift = EarningsFM36Constants.LearnDelApplicProv1618FrameworkUplift,
                            LearnDelApplicProv1618Incentive = EarningsFM36Constants.LearnDelApplicProv1618Incentive,
                            LearnDelAppPrevAccDaysIL = 
                                ((model.apprenticeship.PlannedEndDate < currentAcademicYear.EndDate
                                    ? model.apprenticeship.PlannedEndDate
                                    : currentAcademicYear.EndDate)
                                - (model.apprenticeship.StartDate > currentAcademicYear.StartDate
                                    ? model.apprenticeship.StartDate
                                    : currentAcademicYear.StartDate)).Days,
                            LearnDelDisadAmount = EarningsFM36Constants.LearnDelDisadAmount,
                            LearnDelEligDisadvPayment = EarningsFM36Constants.LearnDelEligDisadvPayment,
                            LearnDelEmpIdFirstAdditionalPaymentThreshold = EarningsFM36Constants.LearnDelEmpIdFirstAdditionalPaymentThreshold,
                            LearnDelEmpIdSecondAdditionalPaymentThreshold = EarningsFM36Constants.LearnDelEmpIdSecondAdditionalPaymentThreshold,
                            LearnDelHistDaysThisApp = (currentAcademicYear.EndDate - model.apprenticeship.StartDate).Days,
                            LearnDelHistProgEarnings = model.earningsApprenticeship.Episodes
                                .SelectMany(episode => episode.Instalments)
                                .Sum(instalment => instalment.Amount),
                            LearnDelInitialFundLineType = model.earningsApprenticeship.FundingLineType,
                            LearnDelMathEng = EarningsFM36Constants.LearnDelMathEng,
                            LearnDelProgEarliestACT2Date = EarningsFM36Constants.LearnDelProgEarliestACT2Date,
                            LearnDelNonLevyProcured = EarningsFM36Constants.LearnDelNonLevyProcured,
                            MathEngAimValue = EarningsFM36Constants.MathEngAimValue,
                            OutstandNumOnProgInstalm = EarningsFM36Constants.OutstandNumOnProgInstalm,
                            PlannedNumOnProgInstalm = model.apprenticeship.StartDate.GetNumberOfIncludedCensusDatesUntil(model.apprenticeship.PlannedEndDate),
                            PlannedTotalDaysIL = (model.apprenticeship.PlannedEndDate - model.apprenticeship.StartDate).Days,
                            ProgType = EarningsFM36Constants.ProgType,
                            PwayCode = EarningsFM36Constants.PwayCode,
                            SecondIncentiveThresholdDate = EarningsFM36Constants.SecondIncentiveThresholdDate,
                            StdCode = int.TryParse(model.apprenticeship.Episodes.MinBy(x => x.Prices.Min(price => price.StartDate))?.TrainingCode, out int parsedTrainingCode) ? parsedTrainingCode : null,
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
                            LearningDeliveryPeriodisedValuesBuilder.BuildInstPerPeriodValues(model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear()),
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
                                model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimOnProgPayment),
                            LearningDeliveryPeriodisedValuesBuilder.BuildCoInvestmentValues(
                                model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimProgFundIndMaxEmpCont, EarningsFM36Constants.CoInvestEmployerMultiplier),
                            LearningDeliveryPeriodisedValuesBuilder.BuildCoInvestmentValues(
                                model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimProgFundIndMinCoInvest, EarningsFM36Constants.CoInvestSfaMultiplier),
                            LearningDeliveryPeriodisedValuesBuilder.BuildInstallmentAmountValues(
                                model.earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimTotProgFund)
                        },
                        LearningDeliveryPeriodisedTextValues = new List<LearningDeliveryPeriodisedTextValues>
                        {
                            LearningDeliveryPeriodisedTextValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.FundLineType, model.earningsApprenticeship.FundingLineType),
                            LearningDeliveryPeriodisedTextValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelContType, EarningsFM36Constants.LearnDelContType)
                        }
                    }
                }
                }).ToArray()
        };

        return result;
    }

    private decimal GetPreviousEarnings(SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship? apprenticeship, short academicYear, short collectionPeriod)
    {
        var previousYearEarnings = apprenticeship?
            .Episodes
            .SelectMany(x => x.Instalments)
            .Where(x => x.AcademicYear.IsEarlierThan(academicYear))
            .Sum(x => x.Amount);

        var previousPeriodEarnings = apprenticeship?
            .Episodes
            .SelectMany(x => x.Instalments)
            .Where(x =>
                x.AcademicYear == academicYear
                && x.DeliveryPeriod < collectionPeriod)
            .Sum(x => x.Amount);

        return previousYearEarnings.GetValueOrDefault() + previousPeriodEarnings.GetValueOrDefault();
    }

    private IEnumerable<(Episode Episode, EpisodePrice Price)> GetApprenticeshipPriceEpisodesForAcademicYearStarting(List<Episode> apprenticeshipEpisodes, DateTime academicYearStartDate)
    {
        foreach (var episodePrice in apprenticeshipEpisodes
                     .SelectMany(episode => episode.Prices.Select(price => (Episode: episode, Price: price))))
        {
            if (episodePrice.Price.StartDate < academicYearStartDate)
            {
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
                yield return episodePrice;
            }
        }
    }
}