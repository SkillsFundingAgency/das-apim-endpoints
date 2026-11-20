using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
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
            var request = new PostSharingSharingaccessApiRequest(new CreateSharingAccessRequest
            {
                SharingId = command.SharingId
            });

            var response = await _digitalCertificatesApiClient.PostWithResponseCode<object>(request, false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
