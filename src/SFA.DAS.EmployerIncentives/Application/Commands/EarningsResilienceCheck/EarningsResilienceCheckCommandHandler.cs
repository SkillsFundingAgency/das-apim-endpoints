using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.EarningsResilienceCheck
{
    public class EarningsResilienceCheckCommandHandler : IRequestHandler<EarningsResilienceCheckCommand>
    {
        private readonly IEarningsResilienceCheckService _earningsResilienceCheckService;

        public EarningsResilienceCheckCommandHandler(IEarningsResilienceCheckService earningsResilienceCheckService)
        {
            _earningsResilienceCheckService = earningsResilienceCheckService;
        }

        public async Task<Unit> Handle(EarningsResilienceCheckCommand request, CancellationToken cancellationToken)
        {
            await _earningsResilienceCheckService.RunCheck();

            return Unit.Value;
        }
    
    }
}
