using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob
{
    public class PostDeleteJobCommand : IRequest<Unit>
    {
        public Guid JobId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
    }
}
