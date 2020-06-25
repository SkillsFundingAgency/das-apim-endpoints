using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCoursesList
{
    public class GetTrainingCoursesListQuery : IRequest<GetTrainingCoursesListResult>
    {
        public string Keyword { get ; set ; }
        public List<Guid> RouteIds { get ; set ; }
    }
}
