using MediatR;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.WithdrawApplication
{
    public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, Unit>
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public WithdrawCommandHandler(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public async Task<Unit> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var postRequest = new PostWithdrawApplicationRequest(request.WithdrawRequest);

            await _client.Post(postRequest);

            return Unit.Value;
        }
    }
}
