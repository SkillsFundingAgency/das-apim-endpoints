using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ReinstateApplication
{
    public class ReinstateApplicationCommandHandler : IRequestHandler<ReinstateApplicationCommand>
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public ReinstateApplicationCommandHandler(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public async Task<Unit> Handle(ReinstateApplicationCommand request, CancellationToken cancellationToken)
        {
            var postRequest = new PostReinstateApplicationRequest(request.ReinstateApplicationRequest);

            await _client.PostWithResponseCode<object>(postRequest, false);

            return Unit.Value;
        }
    }
}
