using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction
{
    public class CreateUserActionCommandHandler : IRequestHandler<CreateUserActionCommand, CreateUserActionResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public CreateUserActionCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<CreateUserActionResult> Handle(CreateUserActionCommand command, CancellationToken cancellationToken)
        {
            var request = new PostCreateUserActionRequest((PostCreateUserActionRequestData)command, command.UserId);

            var response = await _digitalCertificatesApiClient
                 .PostWithResponseCode<PostCreateUserActionRequestData, PostCreateUserActionResponse>(request);

            response.EnsureSuccessStatusCode();

            return (CreateUserActionResult)response.Body;
        }
    }
}
