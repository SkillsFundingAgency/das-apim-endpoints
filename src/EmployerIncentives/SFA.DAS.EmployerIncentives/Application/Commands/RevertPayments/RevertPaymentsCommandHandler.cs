using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.RevertPayments
{
    public class RevertPaymentsCommandHandler : IRequestHandler<RevertPaymentsCommand, Unit>
    {
        private readonly IEmployerIncentivesService _service;

        public RevertPaymentsCommandHandler(IEmployerIncentivesService service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(RevertPaymentsCommand request, CancellationToken cancellationToken)
        {
            await _service.RevertPayments(request.RevertPaymentsRequest);

            return Unit.Value;
        }
    }
}