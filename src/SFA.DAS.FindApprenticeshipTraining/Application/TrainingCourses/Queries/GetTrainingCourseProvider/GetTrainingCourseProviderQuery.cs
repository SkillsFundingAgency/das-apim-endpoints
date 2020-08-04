using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider
{
    public class GetTrainingCourseProviderQuery : IRequest<GetTrainingCourseProviderResult>
    {
        public int CourseId { get ; set ; }
        public int ProviderId { get ; set ; }
    }
}