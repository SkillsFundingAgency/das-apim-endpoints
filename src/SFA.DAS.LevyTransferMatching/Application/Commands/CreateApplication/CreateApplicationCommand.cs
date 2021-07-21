using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication
{
    public class CreateApplicationCommand : IRequest<CreateApplicationCommandResult>
    {
        public int PledgeId { get; set; }
        public long EmployerAccountId { get; set; }
    }

    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, CreateApplicationCommandResult>
    {
        public CreateApplicationCommandHandler()
        {
            
        }

        public Task<CreateApplicationCommandResult> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class CreateApplicationCommandResult
    {
        public int ApplicationId { get; set; }
    }
}
