using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Shared;
using SFA.DAS.SharedOuterApi.Types.Constants;

namespace SFA.DAS.LearnerData.Requests;

public class PostCreateUnapprovedShortCourseLearningRequest : IPostApiRequest
{
    public string PostUrl => $"shortCourses";
    public object Data { get; set; }

    public PostCreateUnapprovedShortCourseLearningRequest(CreateUnapprovedShortCourseLearningRequest request)
    {
        Data = request;
    }
}

public class CreateUnapprovedShortCourseLearningRequest
{
    public Guid LearningKey { get; set; }
    public Guid EpisodeKey { get; set; }
    public Learner Learner { get; set; }
    public List<LearningSupport> LearningSupport { get; set; }
    public OnProgramme OnProgramme { get; set; }
}

public class Learner
{
    public DateTime DateOfBirth { get; set; }
    public string Uln { get; set; }
}

public class OnProgramme
{
    public string CourseCode { get; set; } = null!;

    public long EmployerId { get; set; }

    public long Ukprn { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? WithdrawalDate { get; set; }

    public DateTime? CompletionDate { get; set; }

    public DateTime ExpectedEndDate { get; set; }

    public List<Milestone> Milestones { get; set; } = new();

    public decimal TotalPrice { get; set; }

    public LearningType LearningType { get; set; }
}
