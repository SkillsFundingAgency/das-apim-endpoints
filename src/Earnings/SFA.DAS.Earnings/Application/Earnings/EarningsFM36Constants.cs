namespace SFA.DAS.Earnings.Application.Earnings;

public class EarningsFM36Constants
{
    //Learner
    public const string LearnRefNumber = "9999999999";

    //LearningDelivery
    public const int AimSeqNumber = 1;

    //LearningDeliveryValues
    public const int ActualDaysIL = 0;
    public static DateTime ApplicCompDate = new DateTime(9999, 9, 9);
    public const int CombinedAdjProp = 1;
    public const bool Completed = false;
    public const bool FundStart = true;
    public const int LDApplic1618FrameworkUpliftTotalActEarnings = 0;
    public const string LearnAimRef = "ZPROG001";
    public const int LearnDelApplicDisadvAmount = 0;
    public const int LearnDelApplicEmp1618Incentive = 0;
    public const int LearnDelApplicProv1618FrameworkUplift = 0;
    public const int LearnDelApplicProv1618Incentive = 0;
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
    public static DateTime? SecondIncentiveThresholdDate = null;
    public const int ThresholdDays = 42;
    public const decimal LearnDelApplicCareLeaverIncentive = 0;
    public const int LearnDelHistDaysCareLeavers = 0;
    public const int LearnDelAccDaysILCareLeavers = 0;
    public const int LearnDelPrevAccDaysILCareLeavers = 0;
    public static DateTime? LearnDelLearnerAddPayThresholdDate = null;
    public const int LearnDelRedCode = 0;
    public static DateTime? LearnDelRedStartDate = new DateTime(9999, 9, 9);

    //LearningDeliveryPeriodisedTextValues
    public const string LearnDelContType = "ACT1";

    //CoInvestment Multipliers
    public const decimal CoInvestEmployerMultiplier = 0.05m;
    public const decimal CoInvestSfaMultiplier = 0.95m;
}