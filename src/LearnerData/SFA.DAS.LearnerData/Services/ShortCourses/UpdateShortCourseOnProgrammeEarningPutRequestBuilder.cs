using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;

namespace SFA.DAS.LearnerData.Services.ShortCourses;

public interface IUpdateShortCourseOnProgrammeEarningPutRequestBuilder
{
    UpdateShortCourseOnProgrammeRequestBody Build(OnProgramme onProgramme);
    UpdateShortCourseOnProgrammeRequestBody Build(ShortCourseOnProgramme onProgramme, UpdateShortCourseLearningPutResponse learningResponse, long ukprn);
}

public class UpdateShortCourseOnProgrammeEarningPutRequestBuilder : IUpdateShortCourseOnProgrammeEarningPutRequestBuilder
{
    public UpdateShortCourseOnProgrammeRequestBody Build(OnProgramme onProgramme)
        => BuildBody(onProgramme.WithdrawalDate, onProgramme.CompletionDate, onProgramme.Milestones, null, null);

    public UpdateShortCourseOnProgrammeRequestBody Build(ShortCourseOnProgramme onProgramme, UpdateShortCourseLearningPutResponse learningResponse, long ukprn)
    {
        var learnerRef = learningResponse.Episodes
            .Where(e => e.Ukprn == ukprn)
            .OrderByDescending(e => e.StartDate)
            .Select(e => e.LearnerRef)
            .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(learnerRef))
        {
            throw new InvalidOperationException($"No episode LearnerRef found for Ukprn {ukprn} and LearnerKey {learningResponse.LearnerKey}");
        }

        return BuildBody(onProgramme.WithdrawalDate, onProgramme.CompletionDate, onProgramme.Milestones, learningResponse.LearnerKey, learnerRef);
    }

    private static UpdateShortCourseOnProgrammeRequestBody BuildBody(
        DateTime? withdrawalDate,
        DateTime? completionDate,
        IEnumerable<Milestone> sourceMilestones,
        Guid? learnerKey,
        string? learnerRef)
    {
        var milestones = sourceMilestones.ToList();
        if (completionDate.HasValue && !milestones.Contains(Milestone.LearningComplete))
            milestones.Add(Milestone.LearningComplete);

        return new UpdateShortCourseOnProgrammeRequestBody
        {
            WithdrawalDate = withdrawalDate,
            CompletionDate = completionDate,
            Milestones = milestones,
            LearnerKey = learnerKey ?? Guid.Empty,
            LearnerRef = learnerRef ?? string.Empty
        };
    }
}
