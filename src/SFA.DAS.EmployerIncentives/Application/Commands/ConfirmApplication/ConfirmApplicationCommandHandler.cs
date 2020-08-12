using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication
{
    public class ConfirmApplicationCommandHandler : IRequestHandler<ConfirmApplicationCommand>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public ConfirmApplicationCommandHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Unit> Handle(ConfirmApplicationCommand command, CancellationToken cancellationToken)
        {
            var request = new ConfirmIncentiveApplicationRequest
            {
                Data = new ConfirmIncentiveApplicationRequestData(command.ApplicationId, command.AccountId, command.DateSubmitted, command.SubmittedBy)
            };

            await _employerIncentivesService.ConfirmIncentiveApplication(request, cancellationToken);

            return Unit.Value;
        }
    }
}