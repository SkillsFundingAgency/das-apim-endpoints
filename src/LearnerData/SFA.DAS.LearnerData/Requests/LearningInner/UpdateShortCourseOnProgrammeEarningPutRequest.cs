using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Requests.EarningsInner;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

public class UpdateShortCourseOnProgrammeEarningPutRequest(Guid learningKey, Guid episodeKey, UpdateShortCourseOnProgrammeRequestBody data) : IPutApiRequest<UpdateShortCourseOnProgrammeRequestBody>
{
    public string PutUrl { get; } = $"/{learningKey}/shortCourses/{episodeKey}/on-programme";
    public UpdateShortCourseOnProgrammeRequestBody Data { get; set; } = data;
}

#pragma warning disable CS8618 

public class UpdateShortCourseOnProgrammeRequestBody
{
    public DateTime? WithdrawalDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public List<Milestone> Milestones { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public Guid LearnerKey { get; set; }
    public string LearnerRef { get; set; } = string.Empty;
}

#pragma warning restore CS8618
