using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch
{
    public class CreateUserMatchCommandHandler : IRequestHandler<CreateUserMatchCommand, Unit>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public CreateUserMatchCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<Unit> Handle(CreateUserMatchCommand command, CancellationToken cancellationToken)
        {
            var request = new PostCreateUserMatchRequest((PostCreateUserMatchRequestData)command, command.UserId);

            var response = await _digitalCertificatesApiClient.PostWithResponseCode<PostCreateUserMatchRequestData, object>(request, false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
