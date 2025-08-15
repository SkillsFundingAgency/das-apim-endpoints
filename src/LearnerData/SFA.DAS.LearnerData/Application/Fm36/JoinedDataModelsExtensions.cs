using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;

namespace SFA.DAS.LearnerData.Application.Fm36;

internal static class JoinedDataModelsExtensions
{
    internal static List<PriceEpisodePeriodisedValues> GetPriceEpisodePeriodisedValues(this JoinedEarningsApprenticeship joinedEarningsApprenticeship, JoinedPriceEpisode joinedPriceEpisode, GetAcademicYearsResponse currentAcademicYear)
    {
        var periodisedValues = new List<PriceEpisodePeriodisedValues>();

        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeApplic1618FrameworkUpliftBalancing, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment, 0);
        periodisedValues.AddInstallmentAmountValues(joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeBalancePayment, InstalmentType.Balancing);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeBalanceValue, 0);
        periodisedValues.AddInstallmentAmountValues(joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeCompletionPayment, InstalmentType.Completion);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeFirstDisadvantagePayment, 0);
        periodisedValues.AddNthIncentivePaymentValues(
                joinedEarningsApprenticeship,
                joinedPriceEpisode,
                currentAcademicYear.GetShortAcademicYear(),
                EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeFirstEmp1618Pay,
                EarningsFM36Constants.AdditionalPaymentsTypes.EmployerIncentive,
                1);
        periodisedValues.AddNthIncentivePaymentValues(
                joinedEarningsApprenticeship,
                joinedPriceEpisode,
                currentAcademicYear.GetShortAcademicYear(),
                EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeFirstProv1618Pay,
                EarningsFM36Constants.AdditionalPaymentsTypes.ProviderIncentive,
                1);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeLevyNonPayInd, 0);
        periodisedValues.AddAdditionalPaymentPerPeriodValues(joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeLSFCash, EarningsFM36Constants.AdditionalPaymentsTypes.LearningSupport);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeSecondDisadvantagePayment, 0);
        periodisedValues.AddNthIncentivePaymentValues(
                joinedEarningsApprenticeship,
                joinedPriceEpisode,
                currentAcademicYear.GetShortAcademicYear(),
                EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeSecondEmp1618Pay,
                EarningsFM36Constants.AdditionalPaymentsTypes.EmployerIncentive,
                2);
        periodisedValues.AddNthIncentivePaymentValues(
                joinedEarningsApprenticeship,
                joinedPriceEpisode,
                currentAcademicYear.GetShortAcademicYear(),
                EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeSecondProv1618Pay,
                EarningsFM36Constants.AdditionalPaymentsTypes.ProviderIncentive,
                2);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeLearnerAdditionalPayment, 0);

        periodisedValues.AddPriceEpisodeInstalmentsThisPeriodValues(joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear());

        periodisedValues.AddInstallmentAmountValues(joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeOnProgPayment);
        periodisedValues.AddCoInvestmentValues(joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeProgFundIndMaxEmpCont, EarningsFM36Constants.CoInvestEmployerMultiplier);
        periodisedValues.AddCoInvestmentValues(joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeProgFundIndMinCoInvest, EarningsFM36Constants.CoInvestSfaMultiplier);
        periodisedValues.AddInstallmentAmountValues(joinedPriceEpisode, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeTotProgFunding);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeESFAContribPct, EarningsFM36Constants.CoInvestSfaMultiplier);

        return periodisedValues;
    }

    internal static PriceEpisodeValues GetPriceEpisodeValues(
        this JoinedEarningsApprenticeship joinedEarningsApprenticeship,
        JoinedPriceEpisode joinedPriceEpisode,
        GetAcademicYearsResponse currentAcademicYear,
        byte collectionPeriod,
        bool hasSubsequentPriceEpisodes)
    {
        var previousEarnings = GetPreviousEarnings(joinedEarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), collectionPeriod);

        //Total earnings are for the entire episode, irrespective of academic year
        var totalEpisodeEarnings = joinedEarningsApprenticeship.Episodes
            .Single(x => x.EpisodePriceKey == joinedPriceEpisode.EpisodePriceKey)
            .Instalments.Sum(x => x.Amount);

        return new PriceEpisodeValues
        {
            EpisodeStartDate = joinedPriceEpisode.StartDate.LatestOf(currentAcademicYear.StartDate),

            TNP1 = joinedPriceEpisode.TrainingPrice,
            TNP2 = joinedPriceEpisode.EndPointAssessmentPrice,
            TNP3 = EarningsFM36Constants.TNP3,
            TNP4 = EarningsFM36Constants.TNP4,

            PriceEpisodeActualEndDateIncEPA = joinedEarningsApprenticeship.GetPriceEpisodeActualEndDateIncEPA(joinedPriceEpisode, hasSubsequentPriceEpisodes),



            PriceEpisode1618FUBalValue = EarningsFM36Constants.PriceEpisode1618FUBalValue,
            PriceEpisodeApplic1618FrameworkUpliftCompElement = EarningsFM36Constants.PriceEpisodeApplic1618FrameworkUpliftCompElement,
            PriceEpisode1618FrameworkUpliftTotPrevEarnings = EarningsFM36Constants.PriceEpisode1618FrameworkUpliftTotPrevEarnings,
            PriceEpisode1618FrameworkUpliftRemainingAmount = EarningsFM36Constants.PriceEpisode1618FrameworkUpliftRemainingAmount,
            PriceEpisode1618FUMonthInstValue = EarningsFM36Constants.PriceEpisode1618FUMonthInstValue,
            PriceEpisode1618FUTotEarnings = EarningsFM36Constants.PriceEpisode1618FUTotEarnings,

            PriceEpisodeUpperBandLimit = joinedPriceEpisode.FundingBandMaximum,
            PriceEpisodePlannedEndDate = joinedEarningsApprenticeship.PlannedEndDate,


            PriceEpisodeActualEndDate = joinedEarningsApprenticeship.GetActualEndDate(joinedPriceEpisode, hasSubsequentPriceEpisodes),


            PriceEpisodeTotalTNPPrice = joinedPriceEpisode.TotalPrice,
            PriceEpisodeUpperLimitAdjustment = EarningsFM36Constants.PriceEpisodeUpperLimitAdjustment,

            PriceEpisodePlannedInstalments = joinedPriceEpisode.StartDate.GetNumberOfIncludedCensusDatesUntil(joinedEarningsApprenticeship.PlannedEndDate),
            PriceEpisodeActualInstalments = joinedEarningsApprenticeship.GetPriceEpisodeActualInstalments(currentAcademicYear, hasSubsequentPriceEpisodes),
            PriceEpisodeInstalmentsThisPeriod = joinedEarningsApprenticeship.GetPriceEpisodeInstalmentsThisPeriod(joinedPriceEpisode, currentAcademicYear, collectionPeriod),

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
            PriceEpisodeFundLineType = joinedEarningsApprenticeship.FundingLineType,
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
            - GetPreviousEarnings(joinedEarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), collectionPeriod)
                                        - joinedPriceEpisode.CompletionPayment,
        };
    }

    private static DateTime? GetPriceEpisodeActualEndDateIncEPA(this JoinedEarningsApprenticeship joinedEarningsApprenticeship, JoinedPriceEpisode currentPriceEpisode, bool hasSubsequentPriceEpisodes)
    {
        if (hasSubsequentPriceEpisodes)
        {
            var nextPEStartDate = joinedEarningsApprenticeship.GetNextPriceEpisode(currentPriceEpisode)?.StartDate;
            return joinedEarningsApprenticeship.CompletionDate.EarliestOf(nextPEStartDate);
        }

        return joinedEarningsApprenticeship.CompletionDate;

    }

    private static DateTime? GetActualEndDate(this JoinedEarningsApprenticeship joinedEarningsApprenticeship, JoinedPriceEpisode currentPriceEpisode, bool hasSubsequentPriceEpisodes)
    {
        if (hasSubsequentPriceEpisodes)
        {
            var dayBeforeNextPEStartDate = joinedEarningsApprenticeship.GetNextPriceEpisode(currentPriceEpisode)?.StartDate.AddDays(-1);
            return currentPriceEpisode.ActualEndDate.EarliestOf(dayBeforeNextPEStartDate);
        }

        return currentPriceEpisode.ActualEndDate;

    }

    internal static LearningDeliveryValues GetLearningDelivery(
        this JoinedEarningsApprenticeship joinedEarningsApprenticeship,
        GetAcademicYearsResponse currentAcademicYear)
    {
        var daysInLearning = joinedEarningsApprenticeship.DaysInLearning();
        var firstAdditionalPaymentDate = joinedEarningsApprenticeship.Episodes
            .SelectMany(x => x.AdditionalPayments.Where(p => p.IsIncentive()))
            .MinBy(x => x.DueDate)?.DueDate;
        var secondAdditionalPaymentDate = joinedEarningsApprenticeship.Episodes
            .SelectMany(x => x.AdditionalPayments.Where(p => p.IsIncentive()))
            .DistinctBy(x => x.DueDate)
            .OrderBy(x => x.DueDate)
            .Skip(1)
            .FirstOrDefault()?.DueDate;
        return new LearningDeliveryValues
        {
            ActualDaysIL = daysInLearning,
            AdjStartDate = joinedEarningsApprenticeship.StartDate,
            AgeAtProgStart = joinedEarningsApprenticeship.AgeAtStartOfApprenticeship,
            AppAdjLearnStartDate = joinedEarningsApprenticeship.StartDate,
            AppAdjLearnStartDateMatchPathway = joinedEarningsApprenticeship.StartDate,
            ApplicCompDate = EarningsFM36Constants.ApplicCompDate,
            CombinedAdjProp = EarningsFM36Constants.CombinedAdjProp,
            Completed = EarningsFM36Constants.Completed,
            FundStart = daysInLearning.FundingStart(),
            LDApplic1618FrameworkUpliftTotalActEarnings = EarningsFM36Constants.LDApplic1618FrameworkUpliftTotalActEarnings,
            LearnAimRef = EarningsFM36Constants.LearnAimRef,
            LearnStartDate = joinedEarningsApprenticeship.StartDate,
            LearnDel1618AtStart = joinedEarningsApprenticeship.Episodes.Any(episode =>
                episode.AdditionalPayments.Any(additionalPayment =>
                    additionalPayment.AdditionalPaymentType
                        is EarningsFM36Constants.AdditionalPaymentsTypes.EmployerIncentive
                        or EarningsFM36Constants.AdditionalPaymentsTypes.ProviderIncentive)),
            LearnDelAppAccDaysIL = 1 + ((joinedEarningsApprenticeship.PlannedEndDate < currentAcademicYear.EndDate
                    ? joinedEarningsApprenticeship.PlannedEndDate
                    : currentAcademicYear.EndDate) - joinedEarningsApprenticeship.StartDate).Days,

            LearnDelApplicDisadvAmount = EarningsFM36Constants.LearnDelApplicDisadvAmount,
            LearnDelApplicEmp1618Incentive = joinedEarningsApprenticeship.Episodes.SelectMany(x => x.AdditionalPayments).Where(x => x.AdditionalPaymentType == "EmployerIncentive").Sum(x => x.Amount),
            LearnDelApplicProv1618FrameworkUplift = EarningsFM36Constants.LearnDelApplicProv1618FrameworkUplift,
            LearnDelApplicProv1618Incentive = joinedEarningsApprenticeship.Episodes.SelectMany(x => x.AdditionalPayments).Where(x => x.AdditionalPaymentType == "ProviderIncentive").Sum(x => x.Amount),
            LearnDelAppPrevAccDaysIL = GetLearnDelAppPrevAccDaysIL(joinedEarningsApprenticeship, currentAcademicYear),
            LearnDelDisadAmount = EarningsFM36Constants.LearnDelDisadAmount,
            LearnDelEligDisadvPayment = EarningsFM36Constants.LearnDelEligDisadvPayment,
            LearnDelEmpIdFirstAdditionalPaymentThreshold = EarningsFM36Constants.LearnDelEmpIdFirstAdditionalPaymentThreshold,
            LearnDelEmpIdSecondAdditionalPaymentThreshold = EarningsFM36Constants.LearnDelEmpIdSecondAdditionalPaymentThreshold,
            LearnDelHistDaysThisApp = 1 + (currentAcademicYear.EndDate - joinedEarningsApprenticeship.StartDate).Days,
            LearnDelHistProgEarnings = GetLearnDelHistProgEarnings(joinedEarningsApprenticeship, currentAcademicYear),
            LearnDelInitialFundLineType = joinedEarningsApprenticeship.FundingLineType,
            LearnDelMathEng = EarningsFM36Constants.LearnDelMathEng,
            LearnDelProgEarliestACT2Date = EarningsFM36Constants.LearnDelProgEarliestACT2Date,
            LearnDelNonLevyProcured = EarningsFM36Constants.LearnDelNonLevyProcured,
            MathEngAimValue = EarningsFM36Constants.MathEngAimValue,
            OutstandNumOnProgInstalm = EarningsFM36Constants.OutstandNumOnProgInstalm,
            PlannedNumOnProgInstalm = joinedEarningsApprenticeship.StartDate.GetNumberOfIncludedCensusDatesUntil(joinedEarningsApprenticeship.PlannedEndDate),
            PlannedTotalDaysIL = 1 + (joinedEarningsApprenticeship.PlannedEndDate - joinedEarningsApprenticeship.StartDate).Days,
            ProgType = EarningsFM36Constants.ProgType,
            PwayCode = EarningsFM36Constants.PwayCode,
            SecondIncentiveThresholdDate = secondAdditionalPaymentDate >= joinedEarningsApprenticeship.StartDate && secondAdditionalPaymentDate <= joinedEarningsApprenticeship.PlannedEndDate ? secondAdditionalPaymentDate : null,
            StdCode = int.TryParse(joinedEarningsApprenticeship.Episodes.MinBy(x => x.StartDate)?.TrainingCode, out int parsedTrainingCode) ? parsedTrainingCode : null,
            ThresholdDays = Constants.QualifyingPeriod, // This will eventually change to a calculated value, but for now is using a global constant instead of the local EarningsFM36Constants as other components refer to QualifyingPeriod
            LearnDelApplicCareLeaverIncentive = EarningsFM36Constants.LearnDelApplicCareLeaverIncentive,
            LearnDelHistDaysCareLeavers = EarningsFM36Constants.LearnDelHistDaysCareLeavers,
            LearnDelAccDaysILCareLeavers = EarningsFM36Constants.LearnDelAccDaysILCareLeavers,
            LearnDelPrevAccDaysILCareLeavers = EarningsFM36Constants.LearnDelPrevAccDaysILCareLeavers,
            LearnDelLearnerAddPayThresholdDate = EarningsFM36Constants.LearnDelLearnerAddPayThresholdDate,
            LearnDelRedCode = EarningsFM36Constants.LearnDelRedCode,
            LearnDelRedStartDate = EarningsFM36Constants.LearnDelRedStartDate,
            FirstIncentiveThresholdDate = firstAdditionalPaymentDate >= joinedEarningsApprenticeship.StartDate && firstAdditionalPaymentDate <= joinedEarningsApprenticeship.PlannedEndDate ? firstAdditionalPaymentDate : null
        };
    }

    internal static List<LearningDeliveryPeriodisedValues> GetLearningDeliveryPeriodisedValues(
        this JoinedEarningsApprenticeship joinedEarningsApprenticeship,
        GetAcademicYearsResponse currentAcademicYear)
    {
        var periodisedValues = new List<LearningDeliveryPeriodisedValues>();

        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.DisadvFirstPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.DisadvSecondPayment, 0);
        periodisedValues.AddInstPerPeriodValues(joinedEarningsApprenticeship, currentAcademicYear.GetShortAcademicYear());
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LDApplic1618FrameworkUpliftBalancingPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LDApplic1618FrameworkUpliftCompletionPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LDApplic1618FrameworkUpliftOnProgPayment, 0);
        periodisedValues.AddNthIncentivePaymentValues(joinedEarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.LearnDelFirstEmp1618Pay, "EmployerIncentive", 1);
        periodisedValues.AddNthIncentivePaymentValues(joinedEarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.LearnDelFirstProv1618Pay, "ProviderIncentive", 1);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelLearnAddPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelLevyNonPayInd, 0);
        periodisedValues.AddNthIncentivePaymentValues(joinedEarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.LearnDelSecondEmp1618Pay, "EmployerIncentive", 2);
        periodisedValues.AddNthIncentivePaymentValues(joinedEarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.LearnDelSecondProv1618Pay, "ProviderIncentive", 2);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelSEMContWaiver, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelESFAContribPct, 0.95m);
        periodisedValues.AddAdditionalPaymentPerPeriodIndicators(joinedEarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.LearnSuppFund, EarningsFM36Constants.AdditionalPaymentsTypes.LearningSupport);
        periodisedValues.AddAdditionalPaymentPerPeriodValues(joinedEarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.LearnSuppFundCash, EarningsFM36Constants.AdditionalPaymentsTypes.LearningSupport);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.MathEngBalPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.MathEngOnProgPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimBalPayment, 0);
        periodisedValues.AddWithSamePeriodisedValues(EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimCompletionPayment, 0);
        periodisedValues.AddInstallmentAmountValues(joinedEarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimOnProgPayment);
        periodisedValues.AddCoInvestmentValues(joinedEarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimProgFundIndMaxEmpCont, EarningsFM36Constants.CoInvestEmployerMultiplier);
        periodisedValues.AddCoInvestmentValues(joinedEarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimProgFundIndMinCoInvest, EarningsFM36Constants.CoInvestSfaMultiplier);
        periodisedValues.AddInstallmentAmountValues(joinedEarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimTotProgFund);

        return periodisedValues;
    }

    internal static List<LearningDeliveryPeriodisedTextValues> GetLearningDeliveryPeriodisedTextValues(this JoinedEarningsApprenticeship joinedEarningsApprenticeship)
    {
        return new List<LearningDeliveryPeriodisedTextValues>
            {
                LearningDeliveryPeriodisedTextValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.FundLineType, joinedEarningsApprenticeship.FundingLineType),
                LearningDeliveryPeriodisedTextValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelContType, EarningsFM36Constants.LearnDelContType)
            };
    }

    /// <summary>
    /// Currently only returns days in learning for withdrawn apprenticeship, in future this will need to be expanded to include completed apprenticeships
    /// </summary>
    private static int DaysInLearning(this JoinedEarningsApprenticeship joinedEarningsApprenticeship)
    {
        if (joinedEarningsApprenticeship.WithdrawnDate.HasValue)
        {
            return 1 + (joinedEarningsApprenticeship.WithdrawnDate.Value - joinedEarningsApprenticeship.StartDate).Days;
        }
        return 0;// Default to zero if still in learning
    }

    private static bool FundingStart(this int daysInLearning)
    {
        if (daysInLearning == 0)
        {
            // if we dont have a days in learning value, then we assume the funding start is true. If later the days in learning
            // does not meet the qualifying period, then the funding start will be false and payments will be clawed back
            return true;
        }

        return daysInLearning > Constants.QualifyingPeriod;
    }

    private static decimal GetPreviousEarnings(JoinedEarningsApprenticeship? apprenticeship, short academicYear, short collectionPeriod)
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

    private static int? GetPriceEpisodeActualInstalments(
        this JoinedEarningsApprenticeship joinedEarningsApprenticeship,
        GetAcademicYearsResponse currentAcademicYear,
        bool hasSubsequencePriceEpisodes)
    {

        return hasSubsequencePriceEpisodes
            ? joinedEarningsApprenticeship.Episodes
                .SelectMany(x => x.Instalments)
                .Count(x => x.AcademicYear == short.Parse(currentAcademicYear.AcademicYear))
            : 0;
    }

    private static int? GetPriceEpisodeInstalmentsThisPeriod(
        this JoinedEarningsApprenticeship joinedEarningsApprenticeship,
        JoinedPriceEpisode joinedPriceEpisode,
        GetAcademicYearsResponse currentAcademicYear,
        byte collectionPeriod)
    {

        var censusDateForCollectionPeriod = currentAcademicYear.GetCensusDateForCollectionPeriod(collectionPeriod);

        return joinedPriceEpisode.StartDate <= censusDateForCollectionPeriod
                && censusDateForCollectionPeriod <= joinedPriceEpisode.EndDate
                && joinedEarningsApprenticeship.Episodes
                        .SelectMany(x => x.Instalments)
                        .Any(x => x.AcademicYear == short.Parse(currentAcademicYear.AcademicYear) && x.DeliveryPeriod == collectionPeriod) ? 1 : 0;
    }

    private static int GetLearnDelAppPrevAccDaysIL(
        JoinedEarningsApprenticeship joinedEarningsApprenticeship,
        GetAcademicYearsResponse currentAcademicYear)
    {
        return 1 + ((joinedEarningsApprenticeship.PlannedEndDate < currentAcademicYear.EndDate
                        ? joinedEarningsApprenticeship.PlannedEndDate
                        : currentAcademicYear.EndDate)
                - (joinedEarningsApprenticeship.StartDate > currentAcademicYear.StartDate
                    ? joinedEarningsApprenticeship.StartDate
                    : currentAcademicYear.StartDate)).Days;
    }

    private static decimal GetLearnDelHistProgEarnings(JoinedEarningsApprenticeship joinedEarningsApprenticeship, GetAcademicYearsResponse currentAcademicYear)//, short collectionPeriod)
    {
        //  Currently this will be for only this provider as the api request is for a single provider, but this may need to be expanded in the future
        var previousYearEarnings = joinedEarningsApprenticeship?
            .Episodes
            .SelectMany(x => x.Instalments)
            .Where(x => x.AcademicYear == currentAcademicYear.AcademicYear.GetLastYear())
            .Sum(x => x.Amount);

        var currentYearEarnings = joinedEarningsApprenticeship?
            .Episodes
            .SelectMany(x => x.Instalments)
            .Where(x => x.AcademicYear == currentAcademicYear.GetShortAcademicYear())
            .Sum(x => x.Amount);

        return previousYearEarnings.GetValueOrDefault() + currentYearEarnings.GetValueOrDefault();

    }

    private static JoinedPriceEpisode? GetNextPriceEpisode(this JoinedEarningsApprenticeship joinedEarningsApprenticeship, JoinedPriceEpisode currentPriceEpisode)
    {
        return joinedEarningsApprenticeship.Episodes
            .Where(x => x.EpisodePriceKey != currentPriceEpisode.EpisodePriceKey)
            .OrderBy(x => x.StartDate)
            .FirstOrDefault(x => x.StartDate > currentPriceEpisode.StartDate);
    }
}
