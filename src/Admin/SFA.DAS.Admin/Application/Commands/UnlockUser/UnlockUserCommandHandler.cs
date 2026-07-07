using MediatR;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.Apim.Shared.Extensions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using CreateAdminActionCommand = SFA.DAS.DigitalCertificates.Contracts.ApiResponses.CreateAdminActionCommand;
using PostUsersAdminactionsApiRequest = SFA.DAS.DigitalCertificates.Contracts.ApiRequests.PostUsersAdminactionsApiRequest;
using PutUsersUnlockApiRequest = SFA.DAS.DigitalCertificates.Contracts.ApiRequests.PutUsersByUserIdUnlockApiRequest;
using SFA.DAS.Apim.Shared.Infrastructure;

namespace SFA.DAS.Admin.Application.Commands.UnlockUser
{
    public class UnlockUserCommandHandler : IRequestHandler<UnlockUserCommand, Unit?>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public UnlockUserCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<Unit?> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
        {
            var putRequest = new PutUsersUnlockApiRequest { UserId = request.UserId };

            var putResponse = await _digitalCertificatesApiClient.PutWithResponseCode<object, NullResponse>(putRequest);

            if (putResponse == null) return null;

            putResponse?.EnsureSuccessStatusCode();

            if (putResponse.StatusCode == HttpStatusCode.NoContent)
            {
                var create = new CreateAdminActionCommand
                {
                    Username = request.Username,
                    Action = AdminActionType.Unlocked,
                    UserActionId = request.UserActionId
                };

                var postResponse = await _digitalCertificatesApiClient.PostWithResponseCode<CreateAdminActionCommand>(new PostUsersAdminactionsApiRequest(create));
                postResponse?.EnsureSuccessStatusCode();
            }

            return Unit.Value;
        }
    }
}
