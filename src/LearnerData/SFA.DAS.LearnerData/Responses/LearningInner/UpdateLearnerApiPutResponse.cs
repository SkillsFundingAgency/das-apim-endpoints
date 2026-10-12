using System.Text.Json.Nodes;
using SFA.DAS.Common.Domain.Types;

namespace SFA.DAS.LearnerData.Responses.LearningInner;

public class UpdateLearnerApiPutResponse : BaseLearnerApiPutResponse;

public class CreateDraftLearnerApiPutResponse : BaseLearnerApiPutResponse
{
    public Guid? RemovedLearningKey { get; set; }
}

public abstract class BaseLearnerApiPutResponse
{
    /// <summary>
    /// The change types that were made. This is read from the wire as-is (not derived from <see cref="ChangeDetails"/>),
    /// so it is still populated when talking to a Learning API that does not yet return the details.
    /// </summary>
    public List<LearningUpdateChanges> Changes { get; set; } = [];

    /// <summary>
    /// Every change made, with old/new values and whether it requires approval where they apply.
    /// </summary>
    public List<ChangeDetail> ChangeDetails { get; set; } = [];

    public Guid LearningKey { get; set; }
    public Guid LearningEpisodeKey { get; set; }
    public long ApprovalsApprenticeshipId { get; set; }
    public bool IsApproved { get; set; }
    public LearningType LearningType { get; set; }
    public List<EpisodePrice> Prices { get; set; } = [];

    public class ChangeDetail
    {
        public LearningUpdateChanges ChangeType { get; set; }

        /// <summary>The value before the change: a scalar for a field, an array for a structure such as prices.</summary>
        public JsonNode? Old { get; set; }

        /// <summary>The value after the change, in the same form as <see cref="Old"/>.</summary>
        public JsonNode? New { get; set; }

        public bool RequiresApproval { get; set; }
    }

    public class EpisodePrice
    {
        public Guid Key { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TrainingPrice { get; set; }
        public decimal? EndPointAssessmentPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public enum LearningUpdateChanges
    {
        CompletionDate = 0,
        EnglishAndMaths = 1,
        OnprogrammeLearningSupport = 2,
        Prices = 3,
        ExpectedEndDate = 4,
        Withdrawal = 5,
        ReverseWithdrawal = 6,
		PersonalDetails = 7,
        BreakInLearningStarted = 8,
        BreakInLearningRemoved = 9,
        EnglishAndMathsWithdrawal = 10,
        BreaksInLearningUpdated = 11,
        DateOfBirthChanged = 12,
        Care = 13,
        EnglishAndMathsBreaksInLearningUpdated = 14,
        AchievementDateChanged = 15,
		Reinstated = 16,
        NewApprenticeshipLearner = 17,
        EnglishAndMathsLearningSupport = 18,
        StartDate = 19
    }
}