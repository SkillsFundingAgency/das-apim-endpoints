namespace SFA.DAS.LearnerData.Application.Fm36.Common;

public class EarningsFM36Constants
{
    //Learner
    public const string LearnRefNumber = "9999999999";

    //LearningDelivery & PriceEpisode: AimSequenceNumber
    public const int AimSeqNumber = 1;

    //LearningDeliveryValues
    public static DateTime ApplicCompDate = new DateTime(9999, 9, 9);
    public const int CombinedAdjProp = 1;
    public const bool Completed = false;
    public const int LDApplic1618FrameworkUpliftTotalActEarnings = 0;
    public const string LearnAimRef = "ZPROG001";
    public const int LearnDelApplicDisadvAmount = 0;
    public const int LearnDelApplicProv1618FrameworkUplift = 0;
    public const int LearnDelDisadAmount = 0;
    public const bool LearnDelEligDisadvPayment = false;
    public static int? LearnDelEmpIdFirstAdditionalPaymentThreshold = null;
    public static int? LearnDelEmpIdSecondAdditionalPaymentThreshold = null;
    public const bool LearnDelMathEng = false;
    public static DateTime? LearnDelProgEarliestACT2Date = null;
    public const bool LearnDelNonLevyProcured = false;
    public const decimal MathEngAimValue = 0;
    public static int? OutstandNumOnProgInstalm = null;
    public const int ProgType = 25;
    public static int? PwayCode = null;
    public const decimal LearnDelApplicCareLeaverIncentive = 0;
    public const int LearnDelHistDaysCareLeavers = 0;
    public const int LearnDelAccDaysILCareLeavers = 0;
    public const int LearnDelPrevAccDaysILCareLeavers = 0;
    public static DateTime? LearnDelLearnerAddPayThresholdDate = null;
    public const int LearnDelRedCode = 0;
    public static DateTime? LearnDelRedStartDate = new DateTime(9999, 9, 9);

    //LearningDeliveryPeriodisedTextValues
    public const string LearnDelContType = "Contract for services with the employer";

    //CoInvestment Multipliers
    public const decimal CoInvestEmployerMultiplier = 0.05m;
    public const decimal CoInvestSfaMultiplier = 0.95m;

    //PriceEpisodeValues
    public const decimal TNP3 = 0;
    public const decimal TNP4 = 0;
    public static DateTime? PriceEpisodeActualEndDateIncEPA = null;
    public const decimal PriceEpisode1618FUBalValue = 0;
    public const decimal PriceEpisodeApplic1618FrameworkUpliftCompElement = 0;
    public const decimal PriceEpisode1618FrameworkUpliftTotPrevEarnings = 0;
    public const decimal PriceEpisode1618FrameworkUpliftRemainingAmount = 0;
    public const decimal PriceEpisode1618FUMonthInstValue = 0;
    public const decimal PriceEpisode1618FUTotEarnings = 0;
    public const decimal PriceEpisodeUpperLimitAdjustment = 0;
    public const decimal PriceEpisodePreviousEarnings = 0;
    public const decimal PriceEpisodeOnProgPayment = 0;
    public const decimal PriceEpisodeBalanceValue = 0;
    public const decimal PriceEpisodeBalancePayment = 0;
    public const decimal PriceEpisodeCompletionPayment = 0;
    public const decimal PriceEpisodeFirstDisadvantagePayment = 0;
    public const decimal PriceEpisodeSecondDisadvantagePayment = 0;
    public const decimal PriceEpisodeApplic1618FrameworkUpliftBalancing = 0;
    public const decimal PriceEpisodeApplic1618FrameworkUpliftCompletionPayment = 0;
    public const decimal PriceEpisodeApplic1618FrameworkUpliftOnProgPayment = 0;
    public const decimal PriceEpisodeSecondProv1618Pay = 0;
    public const decimal PriceEpisodeFirstEmp1618Pay = 0;
    public const decimal PriceEpisodeSecondEmp1618Pay = 0;
    public const decimal PriceEpisodeFirstProv1618Pay = 0;
    public const decimal PriceEpisodeLSFCash = 0;
    public const int PriceEpisodeLevyNonPayInd = 0;
    public static DateTime? PriceEpisodeFirstAdditionalPaymentThresholdDate = null;
    public static DateTime? PriceEpisodeSecondAdditionalPaymentThresholdDate = null;
    public const string PriceEpisodeContractType = "Contract for services with the employer";
    public const decimal PriceEpisodePreviousEarningsSameProvider = 0;
    public const decimal PriceEpisodeTotalPMRs = 0;
    public const decimal PriceEpisodeCumulativePMRs = 0;
    public const int PriceEpisodeCompExemCode = 0;
    public static DateTime? PriceEpisodeLearnerAdditionalPaymentThresholdDate = null;
    public static DateTime? PriceEpisodeRedStartDate = null;
    public const int PriceEpisodeRedStatusCode = 0;
    public const decimal PriceEpisodeAugmentedBandLimitFactor = 1;


    public static class PeriodisedAttributes
    {
        //LearningDelivery
        public const string InstPerPeriod = "InstPerPeriod";
        public const string DisadvFirstPayment = "DisadvFirstPayment";
        public const string DisadvSecondPayment = "DisadvSecondPayment";
        public const string LDApplic1618FrameworkUpliftBalancingPayment = "LDApplic1618FrameworkUpliftBalancingPayment";
        public const string LDApplic1618FrameworkUpliftCompletionPayment = "LDApplic1618FrameworkUpliftCompletionPayment";
        public const string LDApplic1618FrameworkUpliftOnProgPayment = "LDApplic1618FrameworkUpliftOnProgPayment";
        public const string LearnDelFirstEmp1618Pay = "LearnDelFirstEmp1618Pay";
        public const string LearnDelFirstProv1618Pay = "LearnDelFirstProv1618Pay";
        public const string LearnDelLearnAddPayment = "LearnDelLearnAddPayment";
        public const string LearnDelLevyNonPayInd = "LearnDelLevyNonPayInd";
        public const string LearnDelSecondEmp1618Pay = "LearnDelSecondEmp1618Pay";
        public const string LearnDelSecondProv1618Pay = "LearnDelSecondProv1618Pay";
        public const string LearnDelSEMContWaiver = "LearnDelSEMContWaiver";
        public const string LearnDelESFAContribPct = "LearnDelESFAContribPct";
        public const string LearnSuppFund = "LearnSuppFund";
        public const string LearnSuppFundCash = "LearnSuppFundCash";
        public const string MathEngBalPayment = "MathEngBalPayment";
        public const string MathEngOnProgPayment = "MathEngOnProgPayment";
        public const string ProgrammeAimBalPayment = "ProgrammeAimBalPayment";
        public const string ProgrammeAimCompletionPayment = "ProgrammeAimCompletionPayment";
        public const string ProgrammeAimOnProgPayment = "ProgrammeAimOnProgPayment";
        public const string ProgrammeAimProgFundIndMaxEmpCont = "ProgrammeAimProgFundIndMaxEmpCont";
        public const string ProgrammeAimProgFundIndMinCoInvest = "ProgrammeAimProgFundIndMinCoInvest";
        public const string ProgrammeAimTotProgFund = "ProgrammeAimTotProgFund";
        public const string FundLineType = "FundLineType";
        public const string LearnDelContType = "LearnDelContType";

        //PriceEpisode
        public const string PriceEpisodeApplic1618FrameworkUpliftBalancing = "PriceEpisodeApplic1618FrameworkUpliftBalancing";
        public const string PriceEpisodeApplic1618FrameworkUpliftCompletionPayment = "PriceEpisodeApplic1618FrameworkUpliftCompletionPayment";
        public const string PriceEpisodeApplic1618FrameworkUpliftOnProgPayment = "PriceEpisodeApplic1618FrameworkUpliftOnProgPayment";
        public const string PriceEpisodeBalancePayment = "PriceEpisodeBalancePayment";
        public const string PriceEpisodeBalanceValue = "PriceEpisodeBalanceValue";
        public const string PriceEpisodeCompletionPayment = "PriceEpisodeCompletionPayment";
        public const string PriceEpisodeFirstDisadvantagePayment = "PriceEpisodeFirstDisadvantagePayment";
        public const string PriceEpisodeFirstEmp1618Pay = "PriceEpisodeFirstEmp1618Pay";
        public const string PriceEpisodeFirstProv1618Pay = "PriceEpisodeFirstProv1618Pay";
        public const string PriceEpisodeLevyNonPayInd = "PriceEpisodeLevyNonPayInd";
        public const string PriceEpisodeLSFCash = "PriceEpisodeLSFCash";
        public const string PriceEpisodeSecondDisadvantagePayment = "PriceEpisodeSecondDisadvantagePayment";
        public const string PriceEpisodeSecondEmp1618Pay = "PriceEpisodeSecondEmp1618Pay";
        public const string PriceEpisodeSecondProv1618Pay = "PriceEpisodeSecondProv1618Pay";
        public const string PriceEpisodeLearnerAdditionalPayment = "PriceEpisodeLearnerAdditionalPayment";
        public const string PriceEpisodeInstalmentsThisPeriod = "PriceEpisodeInstalmentsThisPeriod";
        public const string PriceEpisodeOnProgPayment = "PriceEpisodeOnProgPayment";
        public const string PriceEpisodeProgFundIndMaxEmpCont = "PriceEpisodeProgFundIndMaxEmpCont";
        public const string PriceEpisodeProgFundIndMinCoInvest = "PriceEpisodeProgFundIndMinCoInvest";
        public const string PriceEpisodeTotProgFunding = "PriceEpisodeTotProgFunding";
        public const string PriceEpisodeESFAContribPct = "PriceEpisodeESFAContribPct";
    }

    public static class AdditionalPaymentsTypes
    {
        public const string ProviderIncentive = "ProviderIncentive";
        public const string EmployerIncentive = "EmployerIncentive";
        public const string LearningSupport = "LearningSupport";

        public static List<string> Incentives => [ProviderIncentive, EmployerIncentive];
    }
}