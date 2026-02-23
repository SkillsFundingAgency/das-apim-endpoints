using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Commands.Cmad
{
    public class CreateMyApprenticeshipCommandHandler : IRequestHandler<CreateMyApprenticeshipCommand, ApiResponse<object>>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _client;

        public CreateMyApprenticeshipCommandHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> client)
        {
            _client = client;
        }

        public async Task<ApiResponse<object>> Handle(CreateMyApprenticeshipCommand command, CancellationToken cancellationToken)
        {
            var data = new CreateMyApprenticeshipData
            {
                Uln = command.Data.Uln,
                ApprenticeshipId = command.Data.ApprenticeshipId,
                EmployerName = command.Data.EmployerName,
                StartDate = command.Data.StartDate,
                EndDate = command.Data.EndDate,
                TrainingProviderId = command.Data.TrainingProviderId,
                TrainingProviderName = command.Data.TrainingProviderName,
                TrainingCode = command.Data.TrainingCode,
                StandardUId = command.Data.StandardUId
            };

            var response = await _client.PostWithResponseCode<object>(new CreateMyApprenticeshipRequest(command.ApprenticeId, data), false);            

            return response;
        }

    }
}
