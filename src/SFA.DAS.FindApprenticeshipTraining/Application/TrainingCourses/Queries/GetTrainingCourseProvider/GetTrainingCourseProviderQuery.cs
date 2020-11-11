using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider
{
    public class GetTrainingCourseProviderQuery : IRequest<GetTrainingCourseProviderResult>
    {
        public int CourseId { get ; set ; }
        public int ProviderId { get ; set ; }
        public string Location { get ; set ; }
        public double Lat { get ; set ; }
        public double Lon { get ; set ; }
    }
}