using System;
using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse
{
    public class GetTrainingCourseQuery : IRequest<GetTrainingCourseResult>
    {
        public int Id { get ; set ; }
        public double Lon { get ; set ; }
        public double Lat { get ; set ; }
        public Guid? ShortlistUserId { get ; set ; }
        public string LocationName { get ; set ; }
    }
}