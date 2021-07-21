using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication
{
    public class CreateApplicationCommand : IRequest<CreateApplicationCommandResult>
    {
        public int PledgeId { get; set; }
        public long EmployerAccountId { get; set; }
        public string EncodedAccountId { get; set; }
    }
}
