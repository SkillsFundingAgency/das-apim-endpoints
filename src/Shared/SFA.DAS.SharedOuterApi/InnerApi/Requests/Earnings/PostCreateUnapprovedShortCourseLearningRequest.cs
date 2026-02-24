using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;

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
    public Learner Learner { get; set; }
    public List<LearningSupportItem> LearningSupport { get; set; }
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
}

public enum Milestone
{
    ThirtyPercentLearningComplete = 1,
    LearningComplete = 2,
}

public class LearningSupportItem
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}