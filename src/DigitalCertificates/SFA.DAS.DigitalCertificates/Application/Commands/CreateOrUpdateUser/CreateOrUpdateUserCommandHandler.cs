using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommandHandler : IRequestHandler<CreateOrUpdateUserCommand, CreateOrUpdateUserResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public CreateOrUpdateUserCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<CreateOrUpdateUserResult> Handle(CreateOrUpdateUserCommand command, CancellationToken cancellationToken)
        {
            var request = new PostUsersApiRequest(new CreateOrUpdateUserRequest
            {
                GovUkIdentifier = command.GovUkIdentifier,
                EmailAddress = command.EmailAddress,
                PhoneNumber = command.PhoneNumber
            });

            var response = await _digitalCertificatesApiClient
                .PostWithResponseCode<CreateOrUpdateUserResponse>(request);

            response.EnsureSuccessStatusCode();

            return new CreateOrUpdateUserResult
            {
                UserId = response.Body.UserId
            };
        }
    }
}
