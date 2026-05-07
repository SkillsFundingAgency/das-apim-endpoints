using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;

namespace SFA.DAS.LearnerData.Services.ShortCourses;

public interface IUpdateShortCourseOnProgrammeEarningPutRequestBuilder
{
    UpdateShortCourseOnProgrammeRequestBody Build(OnProgramme onProgramme);
    UpdateShortCourseOnProgrammeRequestBody Build(ShortCourseOnProgramme onProgramme);
}

public class UpdateShortCourseOnProgrammeEarningPutRequestBuilder : IUpdateShortCourseOnProgrammeEarningPutRequestBuilder
{
    public UpdateShortCourseOnProgrammeRequestBody Build(OnProgramme onProgramme)
        => BuildBody(onProgramme.WithdrawalDate, onProgramme.CompletionDate, onProgramme.Milestones);

    public UpdateShortCourseOnProgrammeRequestBody Build(ShortCourseOnProgramme onProgramme)
        => BuildBody(onProgramme.WithdrawalDate, onProgramme.CompletionDate, onProgramme.Milestones);

    private static UpdateShortCourseOnProgrammeRequestBody BuildBody(
        DateTime? withdrawalDate,
        DateTime? completionDate,
        IEnumerable<Milestone> sourceMilestones)
    {
        var milestones = sourceMilestones.ToList();
        if (completionDate.HasValue && !milestones.Contains(Milestone.LearningComplete))
            milestones.Add(Milestone.LearningComplete);

        return new UpdateShortCourseOnProgrammeRequestBody
        {
            WithdrawalDate = withdrawalDate,
            CompletionDate = completionDate,
            Milestones = milestones
        };
    }
}
