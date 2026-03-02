using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Extensions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess
{
    public class CreateSharingAccessCommandHandler : IRequestHandler<CreateSharingAccessCommand, Unit>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public CreateSharingAccessCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<Unit> Handle(CreateSharingAccessCommand command, CancellationToken cancellationToken)
        {
            var request = new PostCreateSharingAccessRequest(command);

            var response = await _digitalCertificatesApiClient.PostWithResponseCode<PostCreateSharingAccessRequestData, object>(request,false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
