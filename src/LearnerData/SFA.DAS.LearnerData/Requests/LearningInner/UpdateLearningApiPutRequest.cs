using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.LearnerData.Shared;

namespace SFA.DAS.LearnerData.Requests.LearningInner
{
    public class UpdateLearningApiPutRequest : IPutApiRequest<UpdateLearningRequestBody>
    {
        public string PutUrl { get; }

        public UpdateLearningRequestBody Data { get; set; }

        public UpdateLearningApiPutRequest(Guid learningKey, UpdateLearningRequestBody data)
        {
            PutUrl = learningKey.ToString();
            Data = data;
        }
    }

    public class UpdateLearningRequestBody
    {
        public Delivery Delivery { get; set; }
        public LearningUpdateDetails Learner { get; set; }
        public List<MathsAndEnglishDetails> EnglishAndMathsCourses { get; set; }
        public List<LearningSupport> LearningSupport { get; set; }
        public OnProgrammeDetails OnProgramme { get; set; }
    }

    public class LearningUpdateDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? EmailAddress { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public CareDetails Care { get; set; }
    }

    public class OnProgrammeDetails
    {
        public DateTime ExpectedEndDate { get; set; }
        public List<Cost> Costs { get; set; }
        public DateTime? PauseDate { get; set; }
        public List<BreakInLearning> BreaksInLearning { get; set; }
    }

    public class Cost
    {
        public int TrainingPrice { get; set; }
        public int? EpaoPrice { get; set; }
        public DateTime FromDate { get; set; }
    }

    public class MathsAndEnglishDetails
    {
        public string Course { get; set; }
        public string LearnAimRef { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? WithdrawalDate { get; set; }
        public DateTime? PauseDate { get; set; }
        public int? CombinedFundingAdjustmentPercentage { get; set; }
        public decimal Amount { get; set; }
        public List<BreakInLearning> BreaksInLearning { get; set; }
    }

    public class Delivery
    {
        public DateTime? WithdrawalDate { get; set; }
    }

    public class BreakInLearning
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PriorPeriodExpectedEndDate { get; set; }
    }

    public class CareDetails
    {
        public bool HasEHCP { get; set; }
        public bool IsCareLeaver { get; set; }
        public bool CareLeaverEmployerConsentGiven { get; set; }
    }
}
