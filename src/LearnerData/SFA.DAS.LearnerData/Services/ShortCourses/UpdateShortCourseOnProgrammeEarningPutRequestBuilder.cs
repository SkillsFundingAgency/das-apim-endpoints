using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;

namespace SFA.DAS.LearnerData.Services.ShortCourses;

public interface IUpdateShortCourseOnProgrammeEarningPutRequestBuilder
{
    UpdateShortCourseOnProgrammeRequestBody Build(OnProgramme onProgramme, Guid learnerKey, string learnerRef);
    UpdateShortCourseOnProgrammeRequestBody Build(ShortCourseOnProgramme onProgramme, Guid LearnerKey, string LearnerRef);
}

public class UpdateShortCourseOnProgrammeEarningPutRequestBuilder : IUpdateShortCourseOnProgrammeEarningPutRequestBuilder
{
    public UpdateShortCourseOnProgrammeRequestBody Build(OnProgramme onProgramme, Guid learnerKey, string learnerRef)
        => BuildBody(onProgramme.WithdrawalDate, onProgramme.CompletionDate, onProgramme.Milestones, onProgramme.StartDate, onProgramme.ExpectedEndDate, learnerKey, learnerRef);

    public UpdateShortCourseOnProgrammeRequestBody Build(ShortCourseOnProgramme onProgramme, Guid learnerKey, string learnerRef)
        => BuildBody(onProgramme.WithdrawalDate, onProgramme.CompletionDate, onProgramme.Milestones, onProgramme.StartDate, onProgramme.ExpectedEndDate, learnerKey, learnerRef);

    private static UpdateShortCourseOnProgrammeRequestBody BuildBody(
        DateTime? withdrawalDate,
        DateTime? completionDate,
        IEnumerable<Milestone> sourceMilestones,
        DateTime startDate,
        DateTime expectedEndDate,
        Guid learnerKey,
        string learnerRef
        )
    {
        var milestones = sourceMilestones.ToList();
        if (completionDate.HasValue && !milestones.Contains(Milestone.LearningComplete))
            milestones.Add(Milestone.LearningComplete);

        return new UpdateShortCourseOnProgrammeRequestBody
        {
            WithdrawalDate = withdrawalDate,
            CompletionDate = completionDate,
            Milestones = milestones,
            StartDate = startDate,
            ExpectedEndDate = expectedEndDate,
            LearnerKey = learnerKey,
            LearnerRef = learnerRef
        };
    }
}
