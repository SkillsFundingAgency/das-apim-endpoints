using System.Collections.Generic;
using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourses;
using System.Linq;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetTrainingCoursesApiResponse
{
    public List<TrainingCourse> TrainingCourses { get; set; } = [];

    public static implicit operator GetTrainingCoursesApiResponse(GetTrainingCoursesQueryResult source)
    {
        return new GetTrainingCoursesApiResponse
        {
            TrainingCourses = source.TrainingCourses.Select(x => (TrainingCourse)x).ToList()
        };
    }

    public class TrainingCourse
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string CourseName { get; set; }
        public int YearAchieved { get; set; }

        public static implicit operator TrainingCourse(GetTrainingCoursesQueryResult.TrainingCourse source)
        {
            return new TrainingCourse
            {
                ApplicationId = source.ApplicationId,
                Id = source.Id,
                CourseName = source.CourseName,
                YearAchieved = source.YearAchieved
            };
        }
    }
}
