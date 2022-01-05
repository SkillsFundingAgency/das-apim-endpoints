using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.EmploymentCheck
{
    public class EmploymentCheckCommandCommandHandler : IRequestHandler<EmploymentCheckCommand>
    {
        private readonly IEmploymentCheckService _employmentCheckService;

        public EmploymentCheckCommandCommandHandler(IEmploymentCheckService employmentCheckService)
        {
            _employmentCheckService = employmentCheckService;
        }

        public async Task<Unit> Handle(EmploymentCheckCommand request, CancellationToken cancellationToken)
        {
            await _employmentCheckService.Update(new UpdateRequest 
            {
                 CorrelationId = request.CorrelationId,
                 Result = request.Result,
                 DateChecked = request.DateChecked
            });

            return Unit.Value;
        }
    
    }
}
