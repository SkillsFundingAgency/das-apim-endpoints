using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.Client;
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
            var request = new DeleteSharingByIdApiRequest(command.SharingId);

            var response = await _digitalCertificatesApiClient.DeleteWithResponseCode<object>(request);

            response.EnsureSuccessStatusCode();
        }
    }
}
