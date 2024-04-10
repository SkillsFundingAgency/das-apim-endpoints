using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourse;
public class GetTrainingCourseQuery : IRequest<GetTrainingCourseQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public Guid TrainingCourseId { get; set; }
}
