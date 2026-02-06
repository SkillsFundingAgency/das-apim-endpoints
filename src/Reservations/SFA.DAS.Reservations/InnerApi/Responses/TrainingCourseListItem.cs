using System;

namespace SFA.DAS.Reservations.InnerApi.Responses;

public class TrainingCourseListItem
{
    public string StandardUId { get; set; }
    public string LarsCode { get; set; }
    public string Title { get; set; }
    public string Level { get; set; }
    public DateTime EffectiveTo { get; set; }
    public string ApprenticeshipType { get; set; }
    public string LearningType { get; set; }
}