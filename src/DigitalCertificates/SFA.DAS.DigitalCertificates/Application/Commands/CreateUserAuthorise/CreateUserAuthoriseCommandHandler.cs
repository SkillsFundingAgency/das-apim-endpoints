using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorise
{
    public class CreateUserAuthoriseCommandHandler : IRequestHandler<CreateUserAuthoriseCommand, Unit>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public CreateUserAuthoriseCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<Unit> Handle(CreateUserAuthoriseCommand command, CancellationToken cancellationToken)
        {
            var request = new PostUsersByUserIdAuthoriseApiRequest(new CreateUserAuthorisationRequest
            {
                Uln = command.Uln
            })
            {
                UserId = command.UserId
            };

            var response = await _digitalCertificatesApiClient.PostWithResponseCode<object>(request, false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
