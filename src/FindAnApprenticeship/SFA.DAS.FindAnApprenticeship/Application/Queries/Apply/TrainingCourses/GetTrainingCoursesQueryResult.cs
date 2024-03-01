using System.Collections.Generic;
using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System.Linq;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourses;
public class GetTrainingCoursesQueryResult
{
    public List<TrainingCourse> TrainingCourses { get; set; } = [];

    public static implicit operator GetTrainingCoursesQueryResult(GetTrainingCoursesApiResponse source)
    {
        return new GetTrainingCoursesQueryResult
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

        public static implicit operator TrainingCourse(GetTrainingCoursesApiResponse.TrainingCourseItem source)
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
