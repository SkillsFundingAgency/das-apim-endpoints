using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse
{
    public class GetTrainingCourseQuery : IRequest<GetTrainingCourseResult>
    {
        public int Id { get ; set ; }
    }
}