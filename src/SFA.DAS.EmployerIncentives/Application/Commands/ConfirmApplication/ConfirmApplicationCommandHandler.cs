using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication
{
    public class ConfirmApplicationCommandHandler : IRequestHandler<ConfirmApplicationCommand>
    {
        private readonly IApplicationService _applicationService;

        public ConfirmApplicationCommandHandler(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public async Task<Unit> Handle(ConfirmApplicationCommand command, CancellationToken cancellationToken)
        {
            var request = new ConfirmIncentiveApplicationRequest
            {
                Data = new ConfirmIncentiveApplicationRequestData(command.ApplicationId, command.AccountId, command.DateSubmitted, command.SubmittedByEmail, command.SubmittedByName)
            };

            await _applicationService.Confirm(request, cancellationToken);

            return Unit.Value;
        }
    }
}