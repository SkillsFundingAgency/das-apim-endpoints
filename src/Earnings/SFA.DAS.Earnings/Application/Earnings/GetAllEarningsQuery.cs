using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using MediatR;
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


namespace SFA.DAS.Earnings.Application.Earnings;

public class GetAllEarningsQuery : IRequest<GetAllEarningsQueryResult>
{
    public long Ukprn { get; set; }
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

    public GetAllEarningsQueryHandler(IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apprenticeshipsApiClient,
        IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
        ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient)
    {
        _apprenticeshipsApiClient = apprenticeshipsApiClient;
        _earningsApiClient = earningsApiClient;
        _collectionCalendarApiClient = collectionCalendarApiClient;
    }

    public async Task<GetAllEarningsQueryResult> Handle(GetAllEarningsQuery request, CancellationToken cancellationToken)
    {
        var apprenticeshipsData = await _apprenticeshipsApiClient.Get<GetApprenticeshipsResponse>(new GetApprenticeshipsRequest { Ukprn = request.Ukprn });
        var earningsData = await _earningsApiClient.Get<GetFm36DataResponse>(new GetFm36DataRequest(request.Ukprn));
        var currentAcademicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearsRequest(DateTime.Now));
        
        var result = new GetAllEarningsQueryResult
        {
            FM36Learners = apprenticeshipsData.Apprenticeships
                .Join(earningsData, a => a.Key, e => e.Key, (apprenticeship,earningsApprenticeship) => new { apprenticeship, earningsApprenticeship })
                .Select(model => new FM36Learner
            {
                ULN = long.Parse(model.apprenticeship.Uln),
                LearnRefNumber = EarningsFM36Constants.LearnRefNumber,
                PriceEpisodes = model.apprenticeship.Episodes.SelectMany(episode => episode.Prices, (episode, price) => new PriceEpisode
                {
                    PriceEpisodeIdentifier = $"25-{episode.TrainingCode}-{price.StartDate:dd/MM/yyyy}",
                    PriceEpisodeValues = new PriceEpisodeValues
                    {
                        EpisodeStartDate = price.StartDate
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
}