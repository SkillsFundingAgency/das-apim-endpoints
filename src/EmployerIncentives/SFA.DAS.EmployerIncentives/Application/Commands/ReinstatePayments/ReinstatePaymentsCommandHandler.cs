using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ReinstatePayments
{
    public class ReinstatePaymentsCommandHandler : IRequestHandler<ReinstatePaymentsCommand, Unit>
    {
        private readonly IEmployerIncentivesService _service;

        public ReinstatePaymentsCommandHandler(IEmployerIncentivesService service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(ReinstatePaymentsCommand request, CancellationToken cancellationToken)
        {
            await _service.ReinstatePayments(request.ReinstatePaymentsRequest);

            return Unit.Value;
        }
    }
}
