using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorise
{
    public class CreateUserAuthoriseCommandHandler : IRequestHandler<CreateUserAuthoriseCommand, Unit>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public CreateUserAuthoriseCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<Unit> Handle(CreateUserAuthoriseCommand command, CancellationToken cancellationToken)
        {
            var request = new PostAuthoriseUserRequest((PostAuthoriseUserRequestData)command, command.UserId);

            var response = await _digitalCertificatesApiClient.PostWithResponseCode<PostAuthoriseUserRequestData, object>(request, false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
