using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCoursesList
{
    public class GetTrainingCoursesListQuery : IRequest<GetTrainingCoursesListResult>
    {
        public string Keyword { get ; set ; }
        public List<string> RouteIds { get ; set ; }
        public List<int> Levels { get ; set ; }
        public OrderBy OrderBy { get; set; }
        public Guid? ShortlistUserId { get ; set ; }
    }
}
