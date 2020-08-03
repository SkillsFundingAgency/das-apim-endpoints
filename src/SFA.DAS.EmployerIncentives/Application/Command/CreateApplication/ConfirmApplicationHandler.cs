using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.Application.Command.CreateApplication
{
    public class ConfirmApplicationHandler : IRequestHandler<ConfirmApplicationCommand>
    {
        private readonly ICommitmentsV2Service _commitmentsV2Service;
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public ConfirmApplicationHandler(ICommitmentsV2Service commitmentsV2Service, IEmployerIncentivesService employerIncentivesService)
        {
            _commitmentsV2Service = commitmentsV2Service;
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