using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.DigitalCertificates.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction
{
    public class CreateUserActionCommandHandler : IRequestHandler<CreateUserActionCommand, CreateUserActionResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public CreateUserActionCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<CreateUserActionResult> Handle(CreateUserActionCommand command, CancellationToken cancellationToken)
        {
            var request = new PostUsersByUserIdUserActionsApiRequest(new CreateUserActionRequest
            {
                ActionType = command.ActionType.ToActionType(),
                FamilyName = command.FamilyName,
                GivenNames = command.GivenNames,
                CertificateId = command.CertificateId,
                CertificateType = command.CertificateType.ToCertificateType(),
                CourseName = command.CourseName
            })
            {
                UserId = command.UserId
            };

            var response = await _digitalCertificatesApiClient
                 .PostWithResponseCode<CreateUserActionResponse>(request);

            response.EnsureSuccessStatusCode();

            return (CreateUserActionResult)response.Body;
        }
    }
}
