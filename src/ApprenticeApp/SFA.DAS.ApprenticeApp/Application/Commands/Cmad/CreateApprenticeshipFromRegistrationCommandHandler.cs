using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Commands.Cmad
{
    public class CreateApprenticeshipFromRegistrationCommandHandler : IRequestHandler<CreateApprenticeshipFromRegistrationCommand, Unit>
    {
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _client;

        public CreateApprenticeshipFromRegistrationCommandHandler(IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> client)
        {
            _client = client;
        }

        public async Task<Unit> Handle(CreateApprenticeshipFromRegistrationCommand command, CancellationToken cancellationToken)
        {
            var data = new CreateApprenticeshipFromRegistrationData
            {
                RegistrationId = command.RegistrationId,
                ApprenticeId = command.ApprenticeId,
                LastName = command.LastName,
                DateOfBirth = command.DateOfBirth
            };            

            await _client.PostWithResponseCode<object>(new CreateApprenticeshipFromRegistrationRequest(data), false);            

            return Unit.Value;            
        }
    }
}
