using System;
using System.Linq;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;

public class UpdateShortCourseLearningPutResponse
{
    public Guid LearningKey { get; set; }
    public string[] Changes { get; set; } = [];
    public decimal Price { get; set; }
    public long Uln { get; set; }
    public string LearnerRef { get; set; }
    public string LearningType { get; set; }
    public string TrainingCode { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public DateTime? CompletionDate { get; set; }
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
    CompletionDate = 2,
    LearnerRef = 3

}