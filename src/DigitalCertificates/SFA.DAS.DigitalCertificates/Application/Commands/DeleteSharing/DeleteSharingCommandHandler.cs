using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing
{
    public class DeleteSharingCommandHandler : IRequestHandler<DeleteSharingCommand>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public DeleteSharingCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task Handle(DeleteSharingCommand command, CancellationToken cancellationToken)
        {
            var request = new DeleteSharingRequest(command.SharingId);

            var response = await _digitalCertificatesApiClient.DeleteWithResponseCode<object>(request);

            response.EnsureSuccessStatusCode();

            return;
        }
    }
}
