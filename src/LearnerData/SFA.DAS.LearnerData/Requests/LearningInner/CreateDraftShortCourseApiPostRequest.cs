using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Shared;
using SFA.DAS.SharedOuterApi.Types.Constants;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

public class CreateDraftShortCourseApiPostRequest(CreateDraftShortCourseRequest data) : IPostApiRequest
{
    public string PostUrl { get; } = $"shortCourses";
    public object Data { get; set; } = data;
}

/// <summary>
/// Request to create a draft short course learner record
/// </summary>
public class CreateDraftShortCourseRequest
{
    /// <summary>
    /// Learner details to be updated
    /// </summary>
    public ShortCourseLearningUpdateDetails LearnerUpdateDetails { get; set; }

    /// <summary>
    /// Learning support details
    /// </summary>
    public List<LearningSupport> LearningSupport { get; set; } = new();

    /// <summary>
    /// On programme details
    /// </summary>
    public OnProgramme OnProgramme { get; set; }
}

public class ShortCourseLearningUpdateDetails : LearningUpdateDetails
{
    public long Uln { get; set; }
    public string LearnerRef { get; set; }
}

/// <summary>
/// On programme details
/// </summary>
public class OnProgramme
{
    /// <summary>
    /// Course code
    /// </summary>
    public string CourseCode { get; set; } = null!;

    /// <summary>
    /// Employer identifier
    /// </summary>
    public long EmployerId { get; set; }

    /// <summary>
    /// Provider UKPRN
    /// </summary>
    public long Ukprn { get; set; }

    /// <summary>
    /// Start date of the short course
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Withdrawal date of the short course
    /// </summary>
    public DateTime? WithdrawalDate { get; set; }

    /// <summary>
    /// Completion date of the short course
    /// </summary>
    public DateTime? CompletionDate { get; set; }

    /// <summary>
    /// Expected end date of the short course
    /// </summary>
    public DateTime ExpectedEndDate { get; set; }

    /// <summary>
    /// Milestones for the short course
    /// </summary>
    public List<Milestone> Milestones { get; set; } = new();

    /// <summary>
    /// Price of the short course
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Learning type of the short course
    /// </summary>
    public LearningType LearningType { get; set; }
}
