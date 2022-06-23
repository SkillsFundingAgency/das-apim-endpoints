using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.AddJobRequest
{
    public class AddJobCommandHandler : IRequestHandler<AddJobCommand>
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public AddJobCommandHandler(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public async Task<Unit> Handle(AddJobCommand command, CancellationToken cancellationToken)
        {
            var request = new PutJobRequest(new JobRequest { Type = command.Type, Data = command.Data });

            await _client.Put(request);

            return Unit.Value;
        }
    }
}