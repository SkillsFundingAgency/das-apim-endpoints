using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;

public class UpdateLearnerApiPutResponse
{
    public List<LearningUpdateChanges> Changes { get; set; } = [];
    public Guid LearningEpisodeKey { get; set; }
    public List<EpisodePrice> Prices { get; set; } = [];

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
        MathsAndEnglish = 1,
        LearningSupport = 2,
        Prices = 3,
        ExpectedEndDate = 4,
        Withdrawal = 5,
        ReverseWithdrawal = 6,
		PersonalDetails = 7,
        BreakInLearningStarted = 8,
        BreakInLearningRemoved = 9,
        MathsAndEnglishWithdrawal = 10,
        BreaksInLearningUpdated = 11,
        DateOfBirthChanged = 12,
        Care = 13,
        EnglishAndMathsBreaksInLearningUpdated = 14
    }
}
