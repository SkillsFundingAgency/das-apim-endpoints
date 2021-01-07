using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.EarningsResilienceCheck
{
    public class EarningsResilienceCheckCommandHandler : IRequestHandler<EarningsResilienceCheckCommand>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public EarningsResilienceCheckCommandHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Unit> Handle(EarningsResilienceCheckCommand request, CancellationToken cancellationToken)
        {
            await _employerIncentivesService.EarningsResilienceCheck();

            return Unit.Value;
        }
    
    }
}
