using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess
{
    public class CreateSharingEmailAccessCommandHandler : IRequestHandler<CreateSharingEmailAccessCommand, Unit>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public CreateSharingEmailAccessCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<Unit> Handle(CreateSharingEmailAccessCommand command, CancellationToken cancellationToken)
        {
            var request = new PostCreateSharingEmailAccessRequest(command);

            var response = await _digitalCertificatesApiClient.PostWithResponseCode<PostCreateSharingEmailAccessRequestData, object>(request, false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
