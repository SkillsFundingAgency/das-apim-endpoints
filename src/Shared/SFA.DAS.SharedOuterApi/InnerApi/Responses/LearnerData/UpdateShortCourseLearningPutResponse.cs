using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;

public class UpdateShortCourseLearningPutResponse
{
    public Guid LearningKey { get; set; }
    public ShortCourseUpdateChanges[] Changes { get; set; } = [];
}

public enum ShortCourseUpdateChanges
{
    WithdrawalDate = 0,
    Milestone = 1,
    CompletionDate = 2
}