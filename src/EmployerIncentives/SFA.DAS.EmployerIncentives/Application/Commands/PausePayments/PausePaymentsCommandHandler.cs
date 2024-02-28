using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.PausePayments
{
    public class PausePaymentsCommandHandler : IRequestHandler<PausePaymentsCommand, Unit>
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public PausePaymentsCommandHandler(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public async Task<Unit> Handle(PausePaymentsCommand request, CancellationToken cancellationToken)
        {
            var postRequest = new PostPausePaymentsRequest(request.PausePaymentsRequest);

            await _client.Post(postRequest);

            return Unit.Value;
        }
    }
}
