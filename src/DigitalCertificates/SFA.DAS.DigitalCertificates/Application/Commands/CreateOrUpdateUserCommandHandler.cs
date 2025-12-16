using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.DigitalCertificates.InnerApi.Requests.PostCreateOrUpdateUserRequest;

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
            var request = new PostCreateOrUpdateUserRequest(new PostCreateOrUpdateUserRequestData
            {
                GovUkIdentifier = command.GovUkIdentifier,
                EmailAddress = command.EmailAddress,
                PhoneNumber = command.PhoneNumber,
                Names = command.Names,
                DateOfBirth = command.DateOfBirth
            });

            var response = await _digitalCertificatesApiClient
                .PostWithResponseCode<PostCreateOrUpdateUserRequestData, CreateOrUpdateUserResponse>(request);

            response.EnsureSuccessStatusCode();

            return new CreateOrUpdateUserResult
            {
                UserId = response.Body.UserId
            };
        }
    }
}
