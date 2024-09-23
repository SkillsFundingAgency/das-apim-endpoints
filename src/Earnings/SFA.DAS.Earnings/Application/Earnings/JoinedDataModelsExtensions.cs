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
                PriceEpisodeRemainingTNPAmount = joinedPriceEpisode.ApprenticeshipEpisodePrice.FundingBandMaximum
                                     - GetPreviousEarnings(joinedEarningsApprenticeship.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), collectionPeriod),
                PriceEpisodeRemainingAmountWithinUpperLimit = joinedPriceEpisode.ApprenticeshipEpisodePrice.FundingBandMaximum
                                                  - GetPreviousEarnings(joinedEarningsApprenticeship.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), collectionPeriod),
                PriceEpisodeCappedRemainingTNPAmount = joinedPriceEpisode.ApprenticeshipEpisodePrice.FundingBandMaximum
                                           - GetPreviousEarnings(joinedEarningsApprenticeship.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), collectionPeriod),
                PriceEpisodeExpectedTotalMonthlyValue = joinedPriceEpisode.ApprenticeshipEpisodePrice.FundingBandMaximum
                - GetPreviousEarnings(joinedEarningsApprenticeship.EarningsApprenticeship, currentAcademicYear.GetShortAcademicYear(), collectionPeriod)
                                            - joinedPriceEpisode.EarningsEpisode.CompletionPayment,
            };
        }

        private static DateTime GetCensusDateForCollectionPeriod(short academicYear, byte collectionPeriod)
        {
            int year;
            int month;
            if (collectionPeriod < 6)
            {
                year = academicYear / 100;
                month = collectionPeriod + 7;
            }
            else
            {
                year = (academicYear / 100) + 1;
                month = collectionPeriod - 5;
            }

            var day = DateTime.DaysInMonth(year, month);
            return new DateTime(year, month, day);
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
            return joinedPriceEpisode.ApprenticeshipEpisodePrice.StartDate <= GetCensusDateForCollectionPeriod(short.Parse(currentAcademicYear.AcademicYear), collectionPeriod)
                    && GetCensusDateForCollectionPeriod(short.Parse(currentAcademicYear.AcademicYear), collectionPeriod) <= joinedPriceEpisode.ApprenticeshipEpisodePrice.EndDate
                    && joinedEarningsApprenticeship.EarningsApprenticeship.Episodes
                            .SelectMany(x => x.Instalments)
                            .Any(x => x.AcademicYear == short.Parse(currentAcademicYear.AcademicYear) && x.DeliveryPeriod == collectionPeriod) ? 1 : 0;
        }
    }
}
