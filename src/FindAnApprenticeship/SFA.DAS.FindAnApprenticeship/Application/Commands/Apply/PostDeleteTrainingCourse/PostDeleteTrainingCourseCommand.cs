using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PostDeleteTrainingCourse
{
    public class PostDeleteTrainingCourseCommand : IRequest<Unit>
    {
        public Guid TrainingCourseId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
    }
}
