using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.RecalculateEarnings
{
    public class RecalculateEarningsCommandHandler :  IRequestHandler<RecalculateEarningsCommand, Unit>
    {
        private readonly IEmployerIncentivesService _service;

        public RecalculateEarningsCommandHandler(IEmployerIncentivesService service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(RecalculateEarningsCommand request, CancellationToken cancellationToken)
        {            
            await _service.RecalculateEarnings(request.RecalculateEarningsRequest);

            return Unit.Value;
        }
    }
}
