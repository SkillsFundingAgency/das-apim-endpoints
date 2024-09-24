using Azure.Core;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using SFA.DAS.Earnings.Application.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using System.Reflection;

namespace SFA.DAS.Earnings.Application.Earnings
{
    internal static class JoinedDataModelsExtensions
    {
        internal static List<PriceEpisodePeriodisedValues> GetPriceEpisodePeriodisedValues(this JoinedEarningsApprenticeship joinedEarningsApprenticeship, GetAcademicYearsResponse currentAcademicYear)
        {
            var earningsApprenticeship = joinedEarningsApprenticeship.EarningsApprenticeship;

            return new List<PriceEpisodePeriodisedValues>()
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
                PriceEpisodePeriodisedValuesBuilder.BuildPriceEpisodeInstalmentsThisPeriodValues(earningsApprenticeship, currentAcademicYear.GetShortAcademicYear()),
                PriceEpisodePeriodisedValuesBuilder.BuildInstallmentAmountValues(earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeOnProgPayment),
                PriceEpisodePeriodisedValuesBuilder.BuildCoInvestmentValues(earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeProgFundIndMaxEmpCont, EarningsFM36Constants.CoInvestEmployerMultiplier),
                PriceEpisodePeriodisedValuesBuilder.BuildCoInvestmentValues(earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeProgFundIndMinCoInvest, EarningsFM36Constants.CoInvestSfaMultiplier),
                PriceEpisodePeriodisedValuesBuilder.BuildInstallmentAmountValues(earningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeTotProgFunding),
                PriceEpisodePeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.PriceEpisodeESFAContribPct, EarningsFM36Constants.CoInvestSfaMultiplier),
            };
        }

        internal static PriceEpisodeValues GetPriceEpisodeValues(
            this JoinedEarningsApprenticeship joinedEarningsApprenticeship, 
            JoinedPriceEpisodeModel joinedPriceEpisode,
            IEnumerable<(Episode Episode, EpisodePrice Price)> priceEpisodesForAcademicYear,
            GetAcademicYearsResponse currentAcademicYear,
            byte collectionPeriod)
        {

            var previousEarnings = GetPreviousEarnings(joinedEarningsApprenticeship.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), collectionPeriod);

            return new PriceEpisodeValues
            {
                EpisodeStartDate = joinedPriceEpisode.ApprenticeshipEpisodePrice.StartDate,

                TNP1 = joinedPriceEpisode.ApprenticeshipEpisodePrice.TrainingPrice,
                TNP2 = joinedPriceEpisode.ApprenticeshipEpisodePrice.EndPointAssessmentPrice,
                TNP3 = EarningsFM36Constants.TNP3,
                TNP4 = EarningsFM36Constants.TNP4,

                PriceEpisodeActualEndDateIncEPA = EarningsFM36Constants.PriceEpisodeActualEndDateIncEPA,
                PriceEpisode1618FUBalValue = EarningsFM36Constants.PriceEpisode1618FUBalValue,
                PriceEpisodeApplic1618FrameworkUpliftCompElement = EarningsFM36Constants.PriceEpisodeApplic1618FrameworkUpliftCompElement,
                PriceEpisode1618FrameworkUpliftTotPrevEarnings = EarningsFM36Constants.PriceEpisode1618FrameworkUpliftTotPrevEarnings,
                PriceEpisode1618FrameworkUpliftRemainingAmount = EarningsFM36Constants.PriceEpisode1618FrameworkUpliftRemainingAmount,
                PriceEpisode1618FUMonthInstValue = EarningsFM36Constants.PriceEpisode1618FUMonthInstValue,
                PriceEpisode1618FUTotEarnings = EarningsFM36Constants.PriceEpisode1618FUTotEarnings,

                PriceEpisodeUpperBandLimit = joinedPriceEpisode.ApprenticeshipEpisodePrice.FundingBandMaximum,
                PriceEpisodePlannedEndDate = joinedEarningsApprenticeship.Apprenticeship.PlannedEndDate,
                PriceEpisodeActualEndDate = joinedPriceEpisode.ApprenticeshipEpisodePrice.EndDate,
                PriceEpisodeTotalTNPPrice = joinedPriceEpisode.ApprenticeshipEpisodePrice.TotalPrice,
                PriceEpisodeUpperLimitAdjustment = EarningsFM36Constants.PriceEpisodeUpperLimitAdjustment,
                
                PriceEpisodePlannedInstalments = joinedPriceEpisode.ApprenticeshipEpisodePrice.StartDate.GetNumberOfIncludedCensusDatesUntil(joinedEarningsApprenticeship.Apprenticeship.PlannedEndDate),
                PriceEpisodeActualInstalments = joinedEarningsApprenticeship.GetPriceEpisodeActualInstalments(joinedPriceEpisode,priceEpisodesForAcademicYear,currentAcademicYear),
                PriceEpisodeInstalmentsThisPeriod = joinedEarningsApprenticeship.GetPriceEpisodeInstalmentsThisPeriod(joinedPriceEpisode, priceEpisodesForAcademicYear,currentAcademicYear, collectionPeriod),

                PriceEpisodeCompletionElement = joinedPriceEpisode.EarningsEpisode.CompletionPayment,
                PriceEpisodePreviousEarnings = EarningsFM36Constants.PriceEpisodePreviousEarnings,
                PriceEpisodeInstalmentValue = joinedPriceEpisode.EarningsEpisode.Instalments.FirstOrDefault()?.Amount ?? 0,
                PriceEpisodeOnProgPayment = EarningsFM36Constants.PriceEpisodeOnProgPayment,
                PriceEpisodeTotalEarnings = joinedPriceEpisode.EarningsEpisode.Instalments.Sum(x => x.Amount),
                PriceEpisodeBalanceValue = EarningsFM36Constants.PriceEpisodeBalanceValue,
                PriceEpisodeBalancePayment = EarningsFM36Constants.PriceEpisodeBalancePayment,
                PriceEpisodeCompleted = joinedPriceEpisode.ApprenticeshipEpisodePrice.EndDate < DateTime.Now,
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
                PriceEpisodeFundLineType = joinedEarningsApprenticeship.EarningsApprenticeship.FundingLineType,
                PriceEpisodeLevyNonPayInd = EarningsFM36Constants.PriceEpisodeLevyNonPayInd,
                EpisodeEffectiveTNPStartDate = joinedPriceEpisode.ApprenticeshipEpisodePrice.StartDate,
                PriceEpisodeFirstAdditionalPaymentThresholdDate = EarningsFM36Constants.PriceEpisodeFirstAdditionalPaymentThresholdDate,
                PriceEpisodeSecondAdditionalPaymentThresholdDate = EarningsFM36Constants.PriceEpisodeSecondAdditionalPaymentThresholdDate,
                PriceEpisodeContractType = EarningsFM36Constants.PriceEpisodeContractType,
                PriceEpisodePreviousEarningsSameProvider = EarningsFM36Constants.PriceEpisodePreviousEarningsSameProvider,
                PriceEpisodeTotProgFunding = joinedPriceEpisode.EarningsEpisode.OnProgramTotal,
                PriceEpisodeProgFundIndMinCoInvest = joinedPriceEpisode.EarningsEpisode.OnProgramTotal * EarningsFM36Constants.CoInvestSfaMultiplier,
                PriceEpisodeProgFundIndMaxEmpCont = joinedPriceEpisode.EarningsEpisode.OnProgramTotal * EarningsFM36Constants.CoInvestEmployerMultiplier,
                PriceEpisodeTotalPMRs = EarningsFM36Constants.PriceEpisodeTotalPMRs,
                PriceEpisodeCumulativePMRs = EarningsFM36Constants.PriceEpisodeCumulativePMRs,
                PriceEpisodeCompExemCode = EarningsFM36Constants.PriceEpisodeCompExemCode,
                PriceEpisodeLearnerAdditionalPaymentThresholdDate = EarningsFM36Constants.PriceEpisodeLearnerAdditionalPaymentThresholdDate,
                PriceEpisodeRedStartDate = EarningsFM36Constants.PriceEpisodeRedStartDate,
                PriceEpisodeRedStatusCode = EarningsFM36Constants.PriceEpisodeRedStatusCode,
                PriceEpisodeLDAppIdent = $"{EarningsFM36Constants.ProgType}-{joinedPriceEpisode.ApprenticeshipEpisode.TrainingCode.Trim()}",
                PriceEpisodeAugmentedBandLimitFactor = EarningsFM36Constants.PriceEpisodeAugmentedBandLimitFactor,
                PriceEpisodeRemainingTNPAmount = joinedPriceEpisode.ApprenticeshipEpisodePrice.FundingBandMaximum - previousEarnings,
                PriceEpisodeRemainingAmountWithinUpperLimit = joinedPriceEpisode.ApprenticeshipEpisodePrice.FundingBandMaximum - previousEarnings,
                PriceEpisodeCappedRemainingTNPAmount = joinedPriceEpisode.ApprenticeshipEpisodePrice.FundingBandMaximum - previousEarnings,
                PriceEpisodeExpectedTotalMonthlyValue = joinedPriceEpisode.ApprenticeshipEpisodePrice.FundingBandMaximum
                - GetPreviousEarnings(joinedEarningsApprenticeship.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), collectionPeriod)
                                            - joinedPriceEpisode.EarningsEpisode.CompletionPayment,
            };
        }

        internal static LearningDeliveryValues GetLearningDelivery(
            this JoinedEarningsApprenticeship joinedEarningsApprenticeship, 
            GetAcademicYearsResponse currentAcademicYear)
        {
            return new LearningDeliveryValues
            {
                ActualDaysIL = EarningsFM36Constants.ActualDaysIL,
                AdjStartDate = joinedEarningsApprenticeship.Apprenticeship.StartDate,
                AgeAtProgStart = joinedEarningsApprenticeship.Apprenticeship.AgeAtStartOfApprenticeship,
                AppAdjLearnStartDate = joinedEarningsApprenticeship.Apprenticeship.StartDate,
                AppAdjLearnStartDateMatchPathway = joinedEarningsApprenticeship.Apprenticeship.StartDate,
                ApplicCompDate = EarningsFM36Constants.ApplicCompDate,
                CombinedAdjProp = EarningsFM36Constants.CombinedAdjProp,
                Completed = EarningsFM36Constants.Completed,
                FundStart = EarningsFM36Constants.FundStart,
                LDApplic1618FrameworkUpliftTotalActEarnings = EarningsFM36Constants.LDApplic1618FrameworkUpliftTotalActEarnings,
                LearnAimRef = EarningsFM36Constants.LearnAimRef,
                LearnStartDate = joinedEarningsApprenticeship.Apprenticeship.StartDate,
                LearnDel1618AtStart = joinedEarningsApprenticeship.Apprenticeship.AgeAtStartOfApprenticeship < 19,
                LearnDelAppAccDaysIL = 1 + ((joinedEarningsApprenticeship.Apprenticeship.PlannedEndDate < currentAcademicYear.EndDate
                        ? joinedEarningsApprenticeship.Apprenticeship.PlannedEndDate
                        : currentAcademicYear.EndDate) - joinedEarningsApprenticeship.Apprenticeship.StartDate).Days,

                LearnDelApplicDisadvAmount = EarningsFM36Constants.LearnDelApplicDisadvAmount,
                LearnDelApplicEmp1618Incentive = EarningsFM36Constants.LearnDelApplicEmp1618Incentive,
                LearnDelApplicProv1618FrameworkUplift = EarningsFM36Constants.LearnDelApplicProv1618FrameworkUplift,
                LearnDelApplicProv1618Incentive = EarningsFM36Constants.LearnDelApplicProv1618Incentive,
                LearnDelAppPrevAccDaysIL = GetLearnDelAppPrevAccDaysIL(joinedEarningsApprenticeship, currentAcademicYear),
                LearnDelDisadAmount = EarningsFM36Constants.LearnDelDisadAmount,
                LearnDelEligDisadvPayment = EarningsFM36Constants.LearnDelEligDisadvPayment,
                LearnDelEmpIdFirstAdditionalPaymentThreshold = EarningsFM36Constants.LearnDelEmpIdFirstAdditionalPaymentThreshold,
                LearnDelEmpIdSecondAdditionalPaymentThreshold = EarningsFM36Constants.LearnDelEmpIdSecondAdditionalPaymentThreshold,
                LearnDelHistDaysThisApp = 1 + (currentAcademicYear.EndDate - joinedEarningsApprenticeship.Apprenticeship.StartDate).Days,
                LearnDelHistProgEarnings = GetLearnDelHistProgEarnings(joinedEarningsApprenticeship, currentAcademicYear),
                LearnDelInitialFundLineType = joinedEarningsApprenticeship.EarningsApprenticeship.FundingLineType,
                LearnDelMathEng = EarningsFM36Constants.LearnDelMathEng,
                LearnDelProgEarliestACT2Date = EarningsFM36Constants.LearnDelProgEarliestACT2Date,
                LearnDelNonLevyProcured = EarningsFM36Constants.LearnDelNonLevyProcured,
                MathEngAimValue = EarningsFM36Constants.MathEngAimValue,
                OutstandNumOnProgInstalm = EarningsFM36Constants.OutstandNumOnProgInstalm,
                PlannedNumOnProgInstalm = joinedEarningsApprenticeship.Apprenticeship.StartDate.GetNumberOfIncludedCensusDatesUntil(joinedEarningsApprenticeship.Apprenticeship.PlannedEndDate),
                PlannedTotalDaysIL = 1 + (joinedEarningsApprenticeship.Apprenticeship.PlannedEndDate - joinedEarningsApprenticeship.Apprenticeship.StartDate).Days,
                ProgType = EarningsFM36Constants.ProgType,
                PwayCode = EarningsFM36Constants.PwayCode,
                SecondIncentiveThresholdDate = EarningsFM36Constants.SecondIncentiveThresholdDate,
                StdCode = int.TryParse(joinedEarningsApprenticeship.Apprenticeship.Episodes.MinBy(x => x.Prices.Min(price => price.StartDate))?.TrainingCode, out int parsedTrainingCode) ? parsedTrainingCode : null,
                ThresholdDays = EarningsFM36Constants.ThresholdDays,
                LearnDelApplicCareLeaverIncentive = EarningsFM36Constants.LearnDelApplicCareLeaverIncentive,
                LearnDelHistDaysCareLeavers = EarningsFM36Constants.LearnDelHistDaysCareLeavers,
                LearnDelAccDaysILCareLeavers = EarningsFM36Constants.LearnDelAccDaysILCareLeavers,
                LearnDelPrevAccDaysILCareLeavers = EarningsFM36Constants.LearnDelPrevAccDaysILCareLeavers,
                LearnDelLearnerAddPayThresholdDate = EarningsFM36Constants.LearnDelLearnerAddPayThresholdDate,
                LearnDelRedCode = EarningsFM36Constants.LearnDelRedCode,
                LearnDelRedStartDate = EarningsFM36Constants.LearnDelRedStartDate
            };
        }

        internal static List<LearningDeliveryPeriodisedValues> GetLearningDeliveryPeriodisedValues(
            this JoinedEarningsApprenticeship joinedEarningsApprenticeship,
            GetAcademicYearsResponse currentAcademicYear)
        {
            return new List<LearningDeliveryPeriodisedValues>
                {
                    LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.DisadvFirstPayment, 0),
                    LearningDeliveryPeriodisedValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.DisadvSecondPayment, 0),
                    LearningDeliveryPeriodisedValuesBuilder.BuildInstPerPeriodValues(joinedEarningsApprenticeship.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear()),
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
                        joinedEarningsApprenticeship.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimOnProgPayment),
                    LearningDeliveryPeriodisedValuesBuilder.BuildCoInvestmentValues(
                        joinedEarningsApprenticeship.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimProgFundIndMaxEmpCont, EarningsFM36Constants.CoInvestEmployerMultiplier),
                    LearningDeliveryPeriodisedValuesBuilder.BuildCoInvestmentValues(
                        joinedEarningsApprenticeship.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimProgFundIndMinCoInvest, EarningsFM36Constants.CoInvestSfaMultiplier),
                    LearningDeliveryPeriodisedValuesBuilder.BuildInstallmentAmountValues(
                        joinedEarningsApprenticeship.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), EarningsFM36Constants.PeriodisedAttributes.ProgrammeAimTotProgFund)
                };
        }

        internal static List<LearningDeliveryPeriodisedTextValues> GetLearningDeliveryPeriodisedTextValues(this JoinedEarningsApprenticeship joinedEarningsApprenticeship)
        {
            return new List<LearningDeliveryPeriodisedTextValues>
                {
                    LearningDeliveryPeriodisedTextValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.FundLineType, joinedEarningsApprenticeship.EarningsApprenticeship.FundingLineType),
                    LearningDeliveryPeriodisedTextValuesBuilder.BuildWithSameValues(EarningsFM36Constants.PeriodisedAttributes.LearnDelContType, EarningsFM36Constants.LearnDelContType)
                };
        }

        private static decimal GetPreviousEarnings(SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship? apprenticeship, short academicYear, short collectionPeriod)
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
            JoinedPriceEpisodeModel joinedPriceEpisode,
            IEnumerable<(Episode Episode, EpisodePrice Price)> priceEpisodesForAcademicYear,
            GetAcademicYearsResponse currentAcademicYear)
        {

            return priceEpisodesForAcademicYear.Any(x => x.Price.StartDate > joinedPriceEpisode.ApprenticeshipEpisodePrice.StartDate)
                ? 0
                : joinedEarningsApprenticeship.EarningsApprenticeship.Episodes
                .SelectMany(x => x.Instalments)
                .Count(x => x.AcademicYear == short.Parse(currentAcademicYear.AcademicYear));
        }

        private static int? GetPriceEpisodeInstalmentsThisPeriod(
            this JoinedEarningsApprenticeship joinedEarningsApprenticeship,
            JoinedPriceEpisodeModel joinedPriceEpisode,
            IEnumerable<(Episode Episode, EpisodePrice Price)> priceEpisodesForAcademicYear,
            GetAcademicYearsResponse currentAcademicYear,
            byte collectionPeriod)
        {

            var censusDateForCollectionPeriod = currentAcademicYear.GetCensusDateForCollectionPeriod(collectionPeriod);

            return joinedPriceEpisode.ApprenticeshipEpisodePrice.StartDate <= censusDateForCollectionPeriod
                    && censusDateForCollectionPeriod <= joinedPriceEpisode.ApprenticeshipEpisodePrice.EndDate
                    && joinedEarningsApprenticeship.EarningsApprenticeship.Episodes
                            .SelectMany(x => x.Instalments)
                            .Any(x => x.AcademicYear == short.Parse(currentAcademicYear.AcademicYear) && x.DeliveryPeriod == collectionPeriod) ? 1 : 0;
        }

        private static int GetLearnDelAppPrevAccDaysIL(
            JoinedEarningsApprenticeship joinedEarningsApprenticeship,
            GetAcademicYearsResponse currentAcademicYear)
        {
            return 1 + ((joinedEarningsApprenticeship.Apprenticeship.PlannedEndDate < currentAcademicYear.EndDate
                            ? joinedEarningsApprenticeship.Apprenticeship.PlannedEndDate
                            : currentAcademicYear.EndDate)
                    - (joinedEarningsApprenticeship.Apprenticeship.StartDate > currentAcademicYear.StartDate
                        ? joinedEarningsApprenticeship.Apprenticeship.StartDate
                        : currentAcademicYear.StartDate)).Days;
        }

        private static decimal GetLearnDelHistProgEarnings(JoinedEarningsApprenticeship joinedEarningsApprenticeship, GetAcademicYearsResponse currentAcademicYear)//, short collectionPeriod)
        {
            //  Currently this will be for only this provider as the api request is for a single provider, but this may need to be expanded in the future
            var previousYearEarnings = joinedEarningsApprenticeship.EarningsApprenticeship?
                .Episodes
                .SelectMany(x => x.Instalments)
                .Where(x => x.AcademicYear == currentAcademicYear.AcademicYear.GetLastYear())
                .Sum(x => x.Amount);

            var currentYearEarnings = joinedEarningsApprenticeship.EarningsApprenticeship?
                .Episodes
                .SelectMany(x => x.Instalments)
                .Where(x => x.AcademicYear == currentAcademicYear.GetShortAcademicYear())
                .Sum(x => x.Amount);

            return previousYearEarnings.GetValueOrDefault() + currentYearEarnings.GetValueOrDefault();

        }
    }
}
