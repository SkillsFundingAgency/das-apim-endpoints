using MediatR;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ValidationOverride
{
    public class ValidationOverrideCommandHandler : IRequestHandler<ValidationOverrideCommand>
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public ValidationOverrideCommandHandler(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public async Task<Unit> Handle(ValidationOverrideCommand request, CancellationToken cancellationToken)
        {
            var postRequest = new PostValidationOverrideRequest(request.ValidationOverrideRequest);

            await _client.Post(postRequest);

            return Unit.Value;
        }
    }
}
