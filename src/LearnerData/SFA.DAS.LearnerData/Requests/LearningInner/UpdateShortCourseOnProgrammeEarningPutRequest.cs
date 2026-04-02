using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

public class UpdateShortCourseOnProgrammeEarningPutRequest(Guid learningKey, UpdateShortCourseOnProgrammeRequestBody data) : IPutApiRequest<UpdateShortCourseOnProgrammeRequestBody>
{
    public string PutUrl { get; } = $"/{learningKey}/shortCourses/on-programme";
    public UpdateShortCourseOnProgrammeRequestBody Data { get; set; } = data;
}

#pragma warning disable CS8618 

public class UpdateShortCourseOnProgrammeRequestBody
{
    public DateTime? WithdrawalDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public List<Milestone> Milestones { get; set; }
}

#pragma warning restore CS8618
