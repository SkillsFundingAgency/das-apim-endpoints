using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication
{
    public class ConfirmApplicationHandler : IRequestHandler<ConfirmApplicationCommand>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public ConfirmApplicationHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Unit> Handle(ConfirmApplicationCommand command, CancellationToken cancellationToken)
        {
            var request = new ConfirmIncentiveApplicationRequest
            {
                AccountId = command.AccountId,
                IncentiveApplicationId = command.ApplicationId,
                DateSubmitted = command.DateSubmitted
            };

            await _employerIncentivesService.ConfirmIncentiveApplication(request, cancellationToken);

            return Unit.Value;
        }
    }
}