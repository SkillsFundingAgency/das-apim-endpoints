using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProviders
{
    public class GetTrainingCourseProvidersQuery : IRequest<GetTrainingCourseProvidersResult>
    {
        public int Id { get ; set ; }
        public string Location { get ; set ; }
    }
}