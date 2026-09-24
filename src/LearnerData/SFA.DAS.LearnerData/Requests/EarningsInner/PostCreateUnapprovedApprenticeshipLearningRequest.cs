using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.EarningsInner;

public class PostCreateUnapprovedApprenticeshipLearningRequest : IPostApiRequest
{
    public string PostUrl => "learning";
    public object Data { get; set; }

    public PostCreateUnapprovedApprenticeshipLearningRequest(CreateUnapprovedApprenticeshipLearningRequest request)
    {
        Data = request;
    }
}

public class CreateUnapprovedApprenticeshipLearningRequest
{
    public Guid LearningKey { get; set; }
    public Guid EpisodeKey { get; set; }
    public long ApprovalsApprenticeshipId { get; set; }
    public DraftApprenticeshipLearnerRequest Learner { get; set; } = null!;
    public DraftApprenticeshipOnProgrammeRequest OnProgramme { get; set; } = null!;
    public DateTime? CompletionDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public DateTime? PauseDate { get; set; }
    public DateTime? AchievementDate { get; set; }
    public List<LearningEpisodePriceItem> Prices { get; set; } = [];
    public List<ApprenticeshipPeriodInLearningItem> PeriodsInLearning { get; set; } = [];
    public List<DraftEnglishAndMathsItem> EnglishAndMaths { get; set; } = [];
    public List<ApprenticeshipLearningSupportItem> LearningSupport { get; set; } = [];
}

public class DraftApprenticeshipLearnerRequest
{
    public DateTime DateOfBirth { get; set; }
    public string Uln { get; set; } = null!;
    public DraftCareRequest Care { get; set; } = null!;
}

public class DraftApprenticeshipOnProgrammeRequest
{
    public string TrainingCode { get; set; } = null!;
    public long Ukprn { get; set; }
    public long EmployerAccountId { get; set; }
    public long? FundingEmployerAccountId { get; set; }
    public string LegalEntityName { get; set; } = string.Empty;
    public ApprenticeshipFundingType EmployerType { get; set; }
    public int? FundingBandMaximum { get; set; }
}

public class ApprenticeshipPeriodInLearningItem
{
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime OriginalExpectedEndDate { get; set; }
}

public class DraftEnglishAndMathsItem
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Course { get; set; } = null!;
    public string LearnAimRef { get; set; } = null!;
    public decimal Amount { get; set; }
    public decimal? CombinedFundingAdjustmentPercentage { get; set; }
    public DateTime? PauseDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public List<ApprenticeshipPeriodInLearningItem> PeriodsInLearning { get; set; } = [];
}

public class DraftCareRequest
{
    public bool HasEHCP { get; set; }
    public bool IsCareLeaver { get; set; }
    public bool CareLeaverEmployerConsentGiven { get; set; }
}

public class LearningEpisodePriceItem
{
    public Guid Key { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal? TrainingPrice { get; set; }
    public decimal? EndPointAssessmentPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class ApprenticeshipLearningSupportItem
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public enum ApprenticeshipFundingType
{
    Levy,
    NonLevy,
    Transfer
}
