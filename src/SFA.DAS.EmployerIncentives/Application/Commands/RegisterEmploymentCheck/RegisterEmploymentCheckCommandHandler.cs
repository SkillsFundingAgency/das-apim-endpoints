using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentCheck;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.RegisterEmploymentCheck
{
    public class RegisterEmploymentCheckCommandHandler : IRequestHandler<RegisterEmploymentCheckCommand, RegisterEmploymentCheckResponse>
    {
        private readonly IEmploymentCheckService _employmentCheckService;

        public RegisterEmploymentCheckCommandHandler(IEmploymentCheckService employmentCheckService)
        {
            _employmentCheckService = employmentCheckService;
        }

        public async Task<RegisterEmploymentCheckResponse> Handle(RegisterEmploymentCheckCommand command, CancellationToken cancellationToken)
        {
            return await _employmentCheckService.Register(new RegisterEmploymentCheckRequest(command));
        }
    }
}
