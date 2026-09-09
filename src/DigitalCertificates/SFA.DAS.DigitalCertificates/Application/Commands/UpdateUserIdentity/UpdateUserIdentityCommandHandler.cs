using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using static SFA.DAS.DigitalCertificates.InnerApi.Requests.PostUpdateUserIdentityRequest;

namespace SFA.DAS.DigitalCertificates.Application.Commands.UpdateUserIdentity
{
    public class UpdateUserIdentityCommandHandler : IRequestHandler<UpdateUserIdentityCommand, Unit>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public UpdateUserIdentityCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<Unit> Handle(UpdateUserIdentityCommand command, CancellationToken cancellationToken)
        {
            var request = new PostUpdateUserIdentityRequest(new PostUpdateUserIdentityRequestData
            {
                Names = command.Names,
                DateOfBirth = command.DateOfBirth
            }, command.UserId);

            var response = await _digitalCertificatesApiClient
                .PostWithResponseCode<PostUpdateUserIdentityRequestData, PostUpdateUserIdentityResponse>(request);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
