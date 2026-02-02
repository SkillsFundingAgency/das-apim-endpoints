using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using SFA.DAS.LearnerData.Application.Fm36.LearningDeliveryHelper;
using SFA.DAS.LearnerData.Application.Fm36.PriceEpisodeHelper;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;

namespace SFA.DAS.LearnerData.Application.Fm36.Common;

internal static class JoinedDataModelsExtensions
{
    internal static List<PriceEpisodePeriodisedValues> GetPriceEpisodePeriodisedValues(this JoinedLearnerData joinedLearnerData, JoinedPriceEpisode joinedPriceEpisode, GetAcademicYearsResponse currentAcademicYear)
    {
        var periodisedValues = new List<PriceEpisodePeriodisedValues>();

        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeApplic1618FrameworkUpliftBalancing, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment, 0);
        periodisedValues.AddInstallmentAmountValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeBalancePayment, joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear(), InstalmentType.Balancing);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeBalanceValue, 0);
        periodisedValues.AddInstallmentAmountValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeCompletionPayment, joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear(), InstalmentType.Completion);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeFirstDisadvantagePayment, 0);
        periodisedValues.AddNthIncentivePaymentValues(
                joinedLearnerData,
                joinedPriceEpisode,
                currentAcademicYear.GetShortAcademicYear(),
                EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeFirstEmp1618Pay,
                EarningsFM36Constants.AdditionalPaymentsTypes.EmployerIncentive,
                1);
        periodisedValues.AddNthIncentivePaymentValues(
                joinedLearnerData,
                joinedPriceEpisode,
                currentAcademicYear.GetShortAcademicYear(),
                EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeFirstProv1618Pay,
                EarningsFM36Constants.AdditionalPaymentsTypes.ProviderIncentive,
                1);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeLevyNonPayInd, 0);
        periodisedValues.AddAdditionalPaymentPerPeriodValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeLSFCash, joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.AdditionalPaymentsTypes.LearningSupport);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeSecondDisadvantagePayment, 0);
        periodisedValues.AddNthIncentivePaymentValues(
                joinedLearnerData,
                joinedPriceEpisode,
                currentAcademicYear.GetShortAcademicYear(),
                EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeSecondEmp1618Pay,
                EarningsFM36Constants.AdditionalPaymentsTypes.EmployerIncentive,
                2);
        periodisedValues.AddNthIncentivePaymentValues(
                joinedLearnerData,
                joinedPriceEpisode,
                currentAcademicYear.GetShortAcademicYear(),
                EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeSecondProv1618Pay,
                EarningsFM36Constants.AdditionalPaymentsTypes.ProviderIncentive,
                2);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeLearnerAdditionalPayment, 0);

        periodisedValues.AddPriceEpisodeInstalmentsThisPeriodValues(joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear());

        periodisedValues.AddInstallmentAmountValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeOnProgPayment, joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear());
        periodisedValues.AddCoInvestmentValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeProgFundIndMaxEmpCont, joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.CoInvestEmployerMultiplier);
        periodisedValues.AddCoInvestmentValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeProgFundIndMinCoInvest, joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.CoInvestSfaMultiplier);
        periodisedValues.AddInstallmentAmountValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeTotProgFunding, joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear());
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeESFAContribPct, EarningsFM36Constants.CoInvestSfaMultiplier);

        return periodisedValues;
    }

    internal static PriceEpisodeValues GetPriceEpisodeValues(
        this JoinedLearnerData joinedLearnerData,
        JoinedPriceEpisode joinedPriceEpisode,
        GetAcademicYearsResponse currentAcademicYear,
        byte collectionPeriod,
        bool hasSubsequentPriceEpisodes)
    {
        var previousEarnings = GetPreviousEarnings(joinedLearnerData, currentAcademicYear.GetShortAcademicYear(), collectionPeriod);

        //Total earnings are for the entire episode, irrespective of academic year
        var totalEpisodeEarnings = joinedLearnerData.Episodes
            .Single(x => x.EpisodePriceKey == joinedPriceEpisode.EpisodePriceKey)
            .Instalments.Sum(x => x.Amount);

        return new PriceEpisodeValues
        {
            EpisodeStartDate = joinedPriceEpisode.StartDate.LatestOf(currentAcademicYear.StartDate),

            TNP1 = joinedPriceEpisode.TrainingPrice,
            TNP2 = joinedPriceEpisode.EndPointAssessmentPrice,
            TNP3 = EarningsFM36Constants.TNP3,
            TNP4 = EarningsFM36Constants.TNP4,

            PriceEpisodeActualEndDateIncEPA = joinedLearnerData.GetPriceEpisodeActualEndDateIncEPA(joinedPriceEpisode, hasSubsequentPriceEpisodes),



            PriceEpisode1618FUBalValue = EarningsFM36Constants.PriceEpisode1618FUBalValue,
            PriceEpisodeApplic1618FrameworkUpliftCompElement = EarningsFM36Constants.PriceEpisodeApplic1618FrameworkUpliftCompElement,
            PriceEpisode1618FrameworkUpliftTotPrevEarnings = EarningsFM36Constants.PriceEpisode1618FrameworkUpliftTotPrevEarnings,
            PriceEpisode1618FrameworkUpliftRemainingAmount = EarningsFM36Constants.PriceEpisode1618FrameworkUpliftRemainingAmount,
            PriceEpisode1618FUMonthInstValue = EarningsFM36Constants.PriceEpisode1618FUMonthInstValue,
            PriceEpisode1618FUTotEarnings = EarningsFM36Constants.PriceEpisode1618FUTotEarnings,

            PriceEpisodeUpperBandLimit = joinedPriceEpisode.FundingBandMaximum,
            PriceEpisodePlannedEndDate = joinedLearnerData.PlannedEndDate,


            PriceEpisodeActualEndDate = joinedLearnerData.GetActualEndDate(joinedPriceEpisode, hasSubsequentPriceEpisodes),


            PriceEpisodeTotalTNPPrice = joinedPriceEpisode.TotalPrice,
            PriceEpisodeUpperLimitAdjustment = EarningsFM36Constants.PriceEpisodeUpperLimitAdjustment,

            PriceEpisodePlannedInstalments = joinedPriceEpisode.StartDate.GetNumberOfIncludedCensusDatesUntil(joinedLearnerData.PlannedEndDate),
            PriceEpisodeActualInstalments = joinedLearnerData.GetPriceEpisodeActualInstalments(currentAcademicYear, hasSubsequentPriceEpisodes),
            PriceEpisodeInstalmentsThisPeriod = joinedLearnerData.GetPriceEpisodeInstalmentsThisPeriod(joinedPriceEpisode, currentAcademicYear, collectionPeriod),

            PriceEpisodeCompletionElement = joinedPriceEpisode.CompletionPayment,
            PriceEpisodePreviousEarnings = EarningsFM36Constants.PriceEpisodePreviousEarnings,
            PriceEpisodeInstalmentValue = joinedPriceEpisode.Instalments.FirstOrDefault()?.Amount ?? 0,
            PriceEpisodeOnProgPayment = EarningsFM36Constants.PriceEpisodeOnProgPayment,
            PriceEpisodeTotalEarnings = totalEpisodeEarnings,
            PriceEpisodeBalanceValue = EarningsFM36Constants.PriceEpisodeBalanceValue,
            PriceEpisodeBalancePayment = EarningsFM36Constants.PriceEpisodeBalancePayment,
            PriceEpisodeCompleted = joinedPriceEpisode.EndDate < DateTime.Now,
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
            PriceEpisodeFundLineType = joinedLearnerData.FundingLineType,
            PriceEpisodeLevyNonPayInd = EarningsFM36Constants.PriceEpisodeLevyNonPayInd,
            EpisodeEffectiveTNPStartDate = joinedPriceEpisode.StartDate,
            PriceEpisodeFirstAdditionalPaymentThresholdDate = EarningsFM36Constants.PriceEpisodeFirstAdditionalPaymentThresholdDate,
            PriceEpisodeSecondAdditionalPaymentThresholdDate = EarningsFM36Constants.PriceEpisodeSecondAdditionalPaymentThresholdDate,
            PriceEpisodeContractType = EarningsFM36Constants.PriceEpisodeContractType,
            PriceEpisodePreviousEarningsSameProvider = EarningsFM36Constants.PriceEpisodePreviousEarningsSameProvider,
            PriceEpisodeTotProgFunding = joinedPriceEpisode.OnProgramTotal,
            PriceEpisodeProgFundIndMinCoInvest = joinedPriceEpisode.OnProgramTotal * EarningsFM36Constants.CoInvestSfaMultiplier,
            PriceEpisodeProgFundIndMaxEmpCont = joinedPriceEpisode.OnProgramTotal * EarningsFM36Constants.CoInvestEmployerMultiplier,
            PriceEpisodeTotalPMRs = EarningsFM36Constants.PriceEpisodeTotalPMRs,
            PriceEpisodeCumulativePMRs = EarningsFM36Constants.PriceEpisodeCumulativePMRs,
            PriceEpisodeCompExemCode = EarningsFM36Constants.PriceEpisodeCompExemCode,
            PriceEpisodeLearnerAdditionalPaymentThresholdDate = EarningsFM36Constants.PriceEpisodeLearnerAdditionalPaymentThresholdDate,
            PriceEpisodeRedStartDate = EarningsFM36Constants.PriceEpisodeRedStartDate,
            PriceEpisodeRedStatusCode = EarningsFM36Constants.PriceEpisodeRedStatusCode,
            PriceEpisodeLDAppIdent = $"{EarningsFM36Constants.ProgType}-{joinedPriceEpisode.TrainingCode.Trim()}",
            PriceEpisodeAugmentedBandLimitFactor = EarningsFM36Constants.PriceEpisodeAugmentedBandLimitFactor,
            PriceEpisodeRemainingTNPAmount = joinedPriceEpisode.FundingBandMaximum - previousEarnings,
            PriceEpisodeRemainingAmountWithinUpperLimit = joinedPriceEpisode.FundingBandMaximum - previousEarnings,
            PriceEpisodeCappedRemainingTNPAmount = joinedPriceEpisode.FundingBandMaximum - previousEarnings,
            PriceEpisodeExpectedTotalMonthlyValue = joinedPriceEpisode.FundingBandMaximum
            - GetPreviousEarnings(joinedLearnerData, currentAcademicYear.GetShortAcademicYear(), collectionPeriod)
                                        - joinedPriceEpisode.CompletionPayment,
        };
    }

    private static DateTime? GetPriceEpisodeActualEndDateIncEPA(this JoinedLearnerData joinedLearnerData, JoinedPriceEpisode currentPriceEpisode, bool hasSubsequentPriceEpisodes)
    {
        if (hasSubsequentPriceEpisodes)
        {
            var nextPEStartDate = joinedLearnerData.GetNextPriceEpisode(currentPriceEpisode)?.StartDate;
            return joinedLearnerData.CompletionDate.EarliestOf(nextPEStartDate);
        }

        return joinedLearnerData.CompletionDate;

    }

    private static DateTime? GetActualEndDate(this JoinedLearnerData joinedLearnerData, JoinedPriceEpisode currentPriceEpisode, bool hasSubsequentPriceEpisodes)
    {
        if (hasSubsequentPriceEpisodes)
        {
            var dayBeforeNextPEStartDate = joinedLearnerData.GetNextPriceEpisode(currentPriceEpisode)?.StartDate.AddDays(-1);
            return currentPriceEpisode.ActualEndDate.EarliestOf(dayBeforeNextPEStartDate);
        }

        return currentPriceEpisode.ActualEndDate;

    }

    internal static LearningDeliveryValues GetLearningDelivery(
        this JoinedLearnerData joinedLearnerData,
        JoinedLearningDelivery joinedLearningDelivery,
        GetAcademicYearsResponse currentAcademicYear)
    {
        var daysInLearning = joinedLearnerData.DaysInLearning();
        var firstAdditionalPaymentDate = joinedLearnerData.Episodes
            .SelectMany(x => x.AdditionalPayments.Where(p => p.IsIncentive()))
            .MinBy(x => x.DueDate)?.DueDate;
        var secondAdditionalPaymentDate = joinedLearnerData.Episodes
            .SelectMany(x => x.AdditionalPayments.Where(p => p.IsIncentive()))
            .DistinctBy(x => x.DueDate)
            .OrderBy(x => x.DueDate)
            .Skip(1)
            .FirstOrDefault()?.DueDate;
        return new LearningDeliveryValues
        {
            ActualDaysIL = daysInLearning,
            AdjStartDate = joinedLearnerData.StartDate,
            AgeAtProgStart = joinedLearnerData.AgeAtStartOfApprenticeship,
            AppAdjLearnStartDate = joinedLearnerData.StartDate,
            AppAdjLearnStartDateMatchPathway = joinedLearnerData.StartDate,
            ApplicCompDate = EarningsFM36Constants.ApplicCompDate,
            CombinedAdjProp = EarningsFM36Constants.CombinedAdjProp,
            Completed = EarningsFM36Constants.Completed,
            FundStart = joinedLearnerData.FundingStart(),
            LDApplic1618FrameworkUpliftTotalActEarnings = EarningsFM36Constants.LDApplic1618FrameworkUpliftTotalActEarnings,
            LearnAimRef = joinedLearningDelivery.LearnAimRef,
            LearnStartDate = joinedLearnerData.StartDate,
            LearnDel1618AtStart = joinedLearnerData.Episodes.Any(episode =>
                episode.AdditionalPayments.Any(additionalPayment =>
                    additionalPayment.AdditionalPaymentType
                        is EarningsFM36Constants.AdditionalPaymentsTypes.EmployerIncentive
                        or EarningsFM36Constants.AdditionalPaymentsTypes.ProviderIncentive)),
            LearnDelAppAccDaysIL = 1 + ((joinedLearnerData.PlannedEndDate < currentAcademicYear.EndDate
                    ? joinedLearnerData.PlannedEndDate
                    : currentAcademicYear.EndDate) - joinedLearnerData.StartDate).Days,

            LearnDelApplicDisadvAmount = EarningsFM36Constants.LearnDelApplicDisadvAmount,
            LearnDelApplicEmp1618Incentive = joinedLearnerData.Episodes.SelectMany(x => x.AdditionalPayments).Where(x => x.AdditionalPaymentType == "EmployerIncentive").Sum(x => x.Amount),
            LearnDelApplicProv1618FrameworkUplift = EarningsFM36Constants.LearnDelApplicProv1618FrameworkUplift,
            LearnDelApplicProv1618Incentive = joinedLearnerData.Episodes.SelectMany(x => x.AdditionalPayments).Where(x => x.AdditionalPaymentType == "ProviderIncentive").Sum(x => x.Amount),
            LearnDelAppPrevAccDaysIL = GetLearnDelAppPrevAccDaysIL(joinedLearnerData, currentAcademicYear),
            LearnDelDisadAmount = EarningsFM36Constants.LearnDelDisadAmount,
            LearnDelEligDisadvPayment = EarningsFM36Constants.LearnDelEligDisadvPayment,
            LearnDelEmpIdFirstAdditionalPaymentThreshold = EarningsFM36Constants.LearnDelEmpIdFirstAdditionalPaymentThreshold,
            LearnDelEmpIdSecondAdditionalPaymentThreshold = EarningsFM36Constants.LearnDelEmpIdSecondAdditionalPaymentThreshold,
            LearnDelHistDaysThisApp = 1 + (currentAcademicYear.EndDate - joinedLearnerData.StartDate).Days,
            LearnDelHistProgEarnings = GetLearnDelHistProgEarnings(joinedLearnerData, currentAcademicYear),
            LearnDelInitialFundLineType = joinedLearnerData.FundingLineType,
            LearnDelMathEng = EarningsFM36Constants.LearnDelMathEng,
            LearnDelProgEarliestACT2Date = EarningsFM36Constants.LearnDelProgEarliestACT2Date,
            LearnDelNonLevyProcured = EarningsFM36Constants.LearnDelNonLevyProcured,
            MathEngAimValue = EarningsFM36Constants.MathEngAimValue,
            OutstandNumOnProgInstalm = EarningsFM36Constants.OutstandNumOnProgInstalm,
            PlannedNumOnProgInstalm = joinedLearnerData.StartDate.GetNumberOfIncludedCensusDatesUntil(joinedLearnerData.PlannedEndDate),
            PlannedTotalDaysIL = joinedLearnerData.PlannedDuration(),
            ProgType = EarningsFM36Constants.ProgType,
            PwayCode = EarningsFM36Constants.PwayCode,
            SecondIncentiveThresholdDate = secondAdditionalPaymentDate >= joinedLearnerData.StartDate && secondAdditionalPaymentDate <= joinedLearnerData.PlannedEndDate ? secondAdditionalPaymentDate : null,
            StdCode = int.TryParse(joinedLearnerData.Episodes.MinBy(x => x.StartDate)?.TrainingCode, out int parsedTrainingCode) ? parsedTrainingCode : null,
            ThresholdDays = joinedLearnerData.QualifyingPeriod(),
            LearnDelApplicCareLeaverIncentive = EarningsFM36Constants.LearnDelApplicCareLeaverIncentive,
            LearnDelHistDaysCareLeavers = EarningsFM36Constants.LearnDelHistDaysCareLeavers,
            LearnDelAccDaysILCareLeavers = EarningsFM36Constants.LearnDelAccDaysILCareLeavers,
            LearnDelPrevAccDaysILCareLeavers = EarningsFM36Constants.LearnDelPrevAccDaysILCareLeavers,
            LearnDelLearnerAddPayThresholdDate = EarningsFM36Constants.LearnDelLearnerAddPayThresholdDate,
            LearnDelRedCode = EarningsFM36Constants.LearnDelRedCode,
            LearnDelRedStartDate = EarningsFM36Constants.LearnDelRedStartDate,
            FirstIncentiveThresholdDate = firstAdditionalPaymentDate >= joinedLearnerData.StartDate && firstAdditionalPaymentDate <= joinedLearnerData.PlannedEndDate ? firstAdditionalPaymentDate : null
        };
    }

    internal static List<LearningDeliveryPeriodisedValues> GetLearningDeliveryPeriodisedValues(
        this JoinedLearningDelivery joinedLearningDelivery,
        GetAcademicYearsResponse currentAcademicYear)
    {
        var periodisedValues = new List<LearningDeliveryPeriodisedValues>();

        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.DisadvFirstPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.DisadvSecondPayment, 0);
        periodisedValues.AddInstPerPeriodValues(joinedLearningDelivery, currentAcademicYear.GetShortAcademicYear());
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LDApplic1618FrameworkUpliftBalancingPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LDApplic1618FrameworkUpliftCompletionPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LDApplic1618FrameworkUpliftOnProgPayment, 0);
        periodisedValues.AddNthIncentivePaymentValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelFirstEmp1618Pay, joinedLearningDelivery, currentAcademicYear.GetShortAcademicYear(), "EmployerIncentive", 1);
        periodisedValues.AddNthIncentivePaymentValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelFirstProv1618Pay, joinedLearningDelivery, currentAcademicYear.GetShortAcademicYear(), "ProviderIncentive", 1);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelLearnAddPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelLevyNonPayInd, 0);
        periodisedValues.AddNthIncentivePaymentValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelSecondEmp1618Pay, joinedLearningDelivery, currentAcademicYear.GetShortAcademicYear(), "EmployerIncentive", 2);
        periodisedValues.AddNthIncentivePaymentValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelSecondProv1618Pay, joinedLearningDelivery, currentAcademicYear.GetShortAcademicYear(), "ProviderIncentive", 2);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelSEMContWaiver, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelESFAContribPct, 0.95m);
        periodisedValues.AddAdditionalPaymentPerPeriodIndicators(EarningsFM36Constants.PeriodisedAttributes.LearnSuppFund, joinedLearningDelivery, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.AdditionalPaymentsTypes.LearningSupport);
        periodisedValues.AddAdditionalPaymentPerPeriodValues(EarningsFM36Constants.PeriodisedAttributes.LearnSuppFundCash, joinedLearningDelivery, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.AdditionalPaymentsTypes.LearningSupport);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.MathEngBalPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.MathEngOnProgPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimBalPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimCompletionPayment, 0);
        periodisedValues.AddInstallmentAmountValues(EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimOnProgPayment, joinedLearningDelivery, currentAcademicYear.GetShortAcademicYear());
        periodisedValues.AddCoInvestmentValues(EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimProgFundIndMaxEmpCont, joinedLearningDelivery, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.CoInvestEmployerMultiplier);
        periodisedValues.AddCoInvestmentValues(EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimProgFundIndMinCoInvest, joinedLearningDelivery, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.CoInvestSfaMultiplier);
        periodisedValues.AddInstallmentAmountValues(EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimTotProgFund, joinedLearningDelivery, currentAcademicYear.GetShortAcademicYear());

        return periodisedValues;
    }

    internal static List<LearningDeliveryPeriodisedTextValues> GetLearningDeliveryPeriodisedTextValues(this JoinedLearnerData joinedLearnerData)
    {
        return new List<LearningDeliveryPeriodisedTextValues>
            {
                LearningDeliveryPeriodisedTextValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.FundLineType, joinedLearnerData.FundingLineType),
                LearningDeliveryPeriodisedTextValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelContType, EarningsFM36Constants.LearnDelContType)
            };
    }

    /// <summary>
    /// Currently only returns days in learning for withdrawn apprenticeship, in future this will need to be expanded to include completed apprenticeships
    /// </summary>
    private static int DaysInLearning(this JoinedLearnerData joinedLearnerData)
    {
        if (joinedLearnerData.WithdrawnDate.HasValue)
        {
            return 1 + (joinedLearnerData.WithdrawnDate.Value - joinedLearnerData.StartDate).Days;
        }
        return 0;// Default to zero if still in learning
    }

    private static bool FundingStart(this JoinedLearnerData joinedLearnerData)
    {
        var daysInLearning = joinedLearnerData.DaysInLearning();

        if (daysInLearning == 0)
        {
            // if we dont have a days in learning value, then we assume the funding start is true. If later the days in learning
            // does not meet the qualifying period, then the funding start will be false and payments will be clawed back
            return true;
        }

        var qualifyingPeriod = joinedLearnerData.QualifyingPeriod();

        return daysInLearning >= qualifyingPeriod;
    }

    private static int QualifyingPeriod(this JoinedLearnerData joinedLearnerData)
    {
        var plannedDuration = joinedLearnerData.PlannedDuration();

        switch (plannedDuration)
        {
            case < 14: return 1;
            case <= 167: return 14;
            default: return 42;
        }
    }

    private static int PlannedDuration(this JoinedLearnerData joinedLearnerData)
    {
        return 1 + (joinedLearnerData.PlannedEndDate - joinedLearnerData.StartDate).Days;
    }

    private static decimal GetPreviousEarnings(JoinedLearnerData? joinedLearnerData, short academicYear, short collectionPeriod)
    {
        var previousYearEarnings = joinedLearnerData?
            .Episodes
            .SelectMany(x => x.Instalments)
            .Where(x => x.AcademicYear.IsEarlierThan(academicYear))
            .Sum(x => x.Amount);

        var previousPeriodEarnings = joinedLearnerData?
            .Episodes
            .SelectMany(x => x.Instalments)
            .Where(x =>
                x.AcademicYear == academicYear
                && x.DeliveryPeriod < collectionPeriod)
            .Sum(x => x.Amount);

        return previousYearEarnings.GetValueOrDefault() + previousPeriodEarnings.GetValueOrDefault();
    }

    private static int? GetPriceEpisodeActualInstalments(
        this JoinedLearnerData joinedLearnerData,
        GetAcademicYearsResponse currentAcademicYear,
        bool hasSubsequencePriceEpisodes)
    {

        return hasSubsequencePriceEpisodes
            ? joinedLearnerData.Episodes
                .SelectMany(x => x.Instalments)
                .Count(x => x.AcademicYear == short.Parse(currentAcademicYear.AcademicYear))
            : 0;
    }

    private static int? GetPriceEpisodeInstalmentsThisPeriod(
        this JoinedLearnerData joinedLearnerData,
        JoinedPriceEpisode joinedPriceEpisode,
        GetAcademicYearsResponse currentAcademicYear,
        byte collectionPeriod)
    {

        var censusDateForCollectionPeriod = currentAcademicYear.GetCensusDateForCollectionPeriod(collectionPeriod);

        return joinedPriceEpisode.StartDate <= censusDateForCollectionPeriod
                && censusDateForCollectionPeriod <= joinedPriceEpisode.EndDate
                && joinedLearnerData.Episodes
                        .SelectMany(x => x.Instalments)
                        .Any(x => x.AcademicYear == short.Parse(currentAcademicYear.AcademicYear) && x.DeliveryPeriod == collectionPeriod) ? 1 : 0;
    }

    private static int GetLearnDelAppPrevAccDaysIL(
        JoinedLearnerData joinedLearnerData,
        GetAcademicYearsResponse currentAcademicYear)
    {
        return 1 + ((joinedLearnerData.PlannedEndDate < currentAcademicYear.EndDate
                        ? joinedLearnerData.PlannedEndDate
                        : currentAcademicYear.EndDate)
                - (joinedLearnerData.StartDate > currentAcademicYear.StartDate
                    ? joinedLearnerData.StartDate
                    : currentAcademicYear.StartDate)).Days;
    }

    private static decimal GetLearnDelHistProgEarnings(JoinedLearnerData joinedLearnerData, GetAcademicYearsResponse currentAcademicYear)//, short collectionPeriod)
    {
        //  Currently this will be for only this provider as the api request is for a single provider, but this may need to be expanded in the future
        var previousYearEarnings = joinedLearnerData?
            .Episodes
            .SelectMany(x => x.Instalments)
            .Where(x => x.AcademicYear == currentAcademicYear.AcademicYear.GetLastYear())
            .Sum(x => x.Amount);

        var currentYearEarnings = joinedLearnerData?
            .Episodes
            .SelectMany(x => x.Instalments)
            .Where(x => x.AcademicYear == currentAcademicYear.GetShortAcademicYear())
            .Sum(x => x.Amount);

        return previousYearEarnings.GetValueOrDefault() + currentYearEarnings.GetValueOrDefault();

    }

    private static JoinedPriceEpisode? GetNextPriceEpisode(this JoinedLearnerData joinedLearnerData, JoinedPriceEpisode currentPriceEpisode)
    {
        return joinedLearnerData.Episodes
            .Where(x => x.EpisodePriceKey != currentPriceEpisode.EpisodePriceKey)
            .OrderBy(x => x.StartDate)
            .FirstOrDefault(x => x.StartDate > currentPriceEpisode.StartDate);
    }
}