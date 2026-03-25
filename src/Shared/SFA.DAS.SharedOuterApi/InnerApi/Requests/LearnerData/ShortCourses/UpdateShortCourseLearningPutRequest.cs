using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;

/// <summary>
/// This request will be sent to the Learning Inner Api
/// </summary>
public class UpdateShortCourseLearningPutRequest : IPutApiRequest<UpdateShortCourseLearningRequestBody>
{
    public string PutUrl { get; }
    public UpdateShortCourseLearningRequestBody Data { get; set; }

    public UpdateShortCourseLearningPutRequest(Guid learningKey, UpdateShortCourseLearningRequestBody data)
    {
        PutUrl = $"shortCourses/{learningKey}";
        Data = data;
    }
}
#pragma warning disable CS8618
public class UpdateShortCourseLearningRequestBody
{
    public ShortCourseLearnerUpdateDetails LearnerUpdateDetails { get; set; }
    public ShortCourseOnProgrammeUpdateDetails OnProgramme { get; set; }
}

public class ShortCourseLearnerUpdateDetails
{
    public string LearnerRef { get; set; }
}

public class ShortCourseOnProgrammeUpdateDetails
{
    public DateTime? WithdrawalDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public List<Milestone> Milestones { get; set; } = new();
}


#pragma warning restore CS8618