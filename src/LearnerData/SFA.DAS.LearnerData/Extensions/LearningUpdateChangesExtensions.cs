using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;

namespace SFA.DAS.LearnerData.Extensions;

public static class LearningUpdateChangesExtensions
{
    public static bool HasOnProgrammeUpdate(this List<UpdateLearnerApiPutResponse.LearningUpdateChanges> changes)
    {
        return changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate)
               || changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.CompletionDate)
               || changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.Withdrawal)
               || changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.ReverseWithdrawal)
               || changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.BreakInLearningStarted)
               || changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.BreakInLearningRemoved)
               || changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.BreaksInLearningUpdated)
               || changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.Prices)
               || changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.DateOfBirthChanged)
               || changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.Care);
    }

    public static bool HasEnglishAndMathsUpdate(this List<UpdateLearnerApiPutResponse.LearningUpdateChanges> changes)
    {
        return changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.MathsAndEnglish)
               || changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.MathsAndEnglishWithdrawal)
               || changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.EnglishAndMathsBreaksInLearningUpdated);
    }

    public static bool HasLearningSupportUpdate(this List<UpdateLearnerApiPutResponse.LearningUpdateChanges> changes)
    {
        return changes.Contains(UpdateLearnerApiPutResponse.LearningUpdateChanges.LearningSupport);
    }

    public static bool HasPersonalDetailsOnly(this List<UpdateLearnerApiPutResponse.LearningUpdateChanges> changes)
    {
        return changes.SequenceEqual(new [] { UpdateLearnerApiPutResponse.LearningUpdateChanges.PersonalDetails });
    }
}
