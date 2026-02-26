using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.TestHelpers;

public class DefaultLearner
{
    public static int Ukprn => 10000001;
    public static string TrainingCode => "274";
    public static int FundingBandMax => 15000; 
    public static string FundingLineType => "19+ Apprenticeship (Employer on App Service)";

    public static LearnerRequestDetails LearnerRequestDetails => new LearnerRequestDetails
    {
        FirstName = "Merlin",
        LastName = "Page",
        Email = "merlin.page@example.com",
        Dob = new DateTime(2006, 1, 1),
        HasEhcp = false,
        Uln = 1234567890
    };

    // Represents a simple apprenticeship with a single on-programme
    public static OnProgrammeRequestDetails OnProgramme => new OnProgrammeRequestDetails
    {
        StandardCode = 101,
        AgreementId = "AG123",
        StartDate = new DateTime(2022, 9, 1),
        ExpectedEndDate = new DateTime(2024, 8, 31),
        Costs = new List<CostDetails>
        {
            new CostDetails
            {
                TrainingPrice = 8000,
                EpaoPrice = 500,
                FromDate = new DateTime(2022, 9, 1)
            }
        },
        LearningSupport = new List<LearningSupportRequestDetails>(),
        Care = new Care(),
        AimSequenceNumber = 1,
        LearnAimRef = "ZPROG001"
    };

    public static TestLearner CreateNew => new TestLearner
    {
        LearningKey = Guid.NewGuid(),
        Ukprn = DefaultLearner.Ukprn,
        TrainingCode = DefaultLearner.TrainingCode,
        FundingBandMax = DefaultLearner.FundingBandMax,
        FundingLineType = DefaultLearner.FundingLineType,
        UpdateLearnerRequest = new UpdateLearnerRequest
        {
            Learner = LearnerRequestDetails,
            Delivery = new UpdateLearnerRequestDeliveryDetails
            {
                OnProgramme = new List<OnProgrammeRequestDetails> { OnProgramme },
                EnglishAndMaths = new List<MathsAndEnglish>()
            }
        }
    };
}

