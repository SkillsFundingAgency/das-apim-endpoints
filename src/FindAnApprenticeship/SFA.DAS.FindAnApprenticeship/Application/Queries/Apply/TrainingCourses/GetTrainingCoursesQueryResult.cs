using System.Collections.Generic;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourses;
public class GetTrainingCoursesQueryResult
{
    public bool? IsSectionCompleted { get; set; }
    public List<TrainingCourse> TrainingCourses { get; set; } = [];

    public class TrainingCourse
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string CourseName { get; set; }
        public int YearAchieved { get; set; }
    }
}
