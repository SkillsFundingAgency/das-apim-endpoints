using System;
using System.Linq;
using System.Threading.Channels;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;

public class UpdateShortCourseLearningPutResponse
{
    public Guid LearningKey { get; set; }
    public string[] Changes { get; set; } = [];
}

public static class UpdateShortCourseLearningPutResponseExtensions
{
    public static ShortCourseUpdateChanges[] GetChangesEnums(this UpdateShortCourseLearningPutResponse response)
    {
        return response.Changes
            .Select(c => Enum.TryParse<ShortCourseUpdateChanges>(c, out var result)
                ? result
                : throw new InvalidOperationException($"Unknown change type '{c}'"))
            .ToArray();
    }
}

public enum ShortCourseUpdateChanges
{
    WithdrawalDate = 0,
    Milestone = 1,
    CompletionDate = 2
}