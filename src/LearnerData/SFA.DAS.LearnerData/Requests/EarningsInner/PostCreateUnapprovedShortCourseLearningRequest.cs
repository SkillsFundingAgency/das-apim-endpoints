<<<<<<<< HEAD:src/Shared/SFA.DAS.SharedOuterApi.Types/InnerApi/Requests/Earnings/PostCreateUnapprovedShortCourseLearningRequest.cs
using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Constants;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Earnings;
========
﻿using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Requests.EarningsInner;
>>>>>>>> origin/master:src/LearnerData/SFA.DAS.LearnerData/Requests/EarningsInner/PostCreateUnapprovedShortCourseLearningRequest.cs

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

    public LearningType LearningType { get; set; }
}

public enum Milestone
{
    ThirtyPercentLearningComplete = 1,
    LearningComplete = 2,
}