using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
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
            var request = new PostSharingSharingemailaccessApiRequest(new CreateSharingEmailAccessRequest
            {
                SharingEmailId = command.SharingEmailId
            });

            var response = await _digitalCertificatesApiClient.PostWithResponseCode<object>(request, false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
