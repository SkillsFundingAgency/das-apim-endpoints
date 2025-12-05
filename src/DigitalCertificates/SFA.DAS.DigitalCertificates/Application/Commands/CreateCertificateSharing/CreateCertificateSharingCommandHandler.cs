using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing
{
    public class CreateCertificateSharingCommandHandler : IRequestHandler<CreateCertificateSharingCommand, CreateCertificateSharingResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public CreateCertificateSharingCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<CreateCertificateSharingResult> Handle(CreateCertificateSharingCommand command, CancellationToken cancellationToken)
        {
            var request = new PostCreateSharingRequest(command);

            var response = await _digitalCertificatesApiClient
                 .PostWithResponseCode<PostCreateSharingRequestData, PostCreateSharingResponse>(request);

            response.EnsureSuccessStatusCode();

            return (CreateCertificateSharingResult)response.Body;
        }
    }
}