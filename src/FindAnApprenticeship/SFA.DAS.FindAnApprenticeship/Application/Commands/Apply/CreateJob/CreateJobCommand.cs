using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob
{
    public class CreateJobCommand : IRequest<CreateJobCommandResponse>
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
        public string EmployerName { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class CreateJobCommandResponse
    {
        public Guid WorkHistoryId { get; set; }
    }

    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, CreateJobCommandResponse>
    {
        public async Task<CreateJobCommandResponse> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
