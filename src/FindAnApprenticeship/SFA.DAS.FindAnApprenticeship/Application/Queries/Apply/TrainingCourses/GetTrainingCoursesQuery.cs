using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourses;
public class GetTrainingCoursesQuery : IRequest<GetTrainingCoursesQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}
