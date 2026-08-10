using SFA.DAS.LearnerData.Enums;
using SFA.DAS.SharedOuterApi.Types.Constants;

namespace SFA.DAS.LearnerData.Services.ShortCourses;

/// <summary>
/// On-programme data resolved from the Learning inner API's response after a create/update call, used to build
/// the Earnings request and the LearnerDataEvent. Distinct from Requests.LearningInner.OnProgramme, which is the
/// outbound request shape sent to the Learning inner API.
/// </summary>
public class ResolvedOnProgramme
{
    public string CourseCode { get; set; } = null!;
    public long Ukprn { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public short? WithdrawalReasonCode { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public List<Milestone> Milestones { get; set; } = new();
    public decimal Price { get; set; }
    public LearningType LearningType { get; set; }
}
