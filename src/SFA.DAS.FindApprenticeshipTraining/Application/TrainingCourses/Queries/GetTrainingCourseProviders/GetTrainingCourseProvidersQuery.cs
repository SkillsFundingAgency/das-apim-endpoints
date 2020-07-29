using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCourseProviders
{
    public class GetTrainingCourseProvidersQuery : IRequest<GetTrainingCourseProvidersResult>
    {
        public int Id { get ; set ; }
    }
}