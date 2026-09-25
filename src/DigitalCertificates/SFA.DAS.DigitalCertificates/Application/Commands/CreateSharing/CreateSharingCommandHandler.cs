using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.DigitalCertificates.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing
{
    public class CreateSharingCommandHandler : IRequestHandler<CreateSharingCommand, CreateSharingResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public CreateSharingCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<CreateSharingResult> Handle(CreateSharingCommand command, CancellationToken cancellationToken)
        {
            var request = new PostSharingApiRequest(new CreateSharingRequest
            {
                UserId = command.UserId,
                CertificateId = command.CertificateId,
                CertificateType = command.CertificateType.ToCertificateType(),
                CourseName = command.CourseName
            });

            var response = await _digitalCertificatesApiClient
                 .PostWithResponseCode<CreateSharingResponse>(request);

            response.EnsureSuccessStatusCode();

            return (CreateSharingResult)response.Body;
        }
    }
}