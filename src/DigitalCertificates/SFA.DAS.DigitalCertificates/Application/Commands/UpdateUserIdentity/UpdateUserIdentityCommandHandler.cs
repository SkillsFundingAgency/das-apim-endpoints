using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;

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
            var request = new PostUsersByUserIdIdentityApiRequest(new UpdateUserIdentityRequest
            {
                Names = command.Names?.Select(n => new NameRequest
                {
                    UserIdentityId = n.UserIdentityId,
                    ValidSince = n.ValidSince,
                    ValidUntil = n.ValidUntil,
                    FamilyName = n.FamilyName,
                    GivenNames = n.GivenNames
                }).ToList(),
                DateOfBirth = command.DateOfBirth
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
