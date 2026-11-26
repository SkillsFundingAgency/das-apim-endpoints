using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public GetUserQueryHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<GetUserResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            ApiResponse<GetUserResponse> userRequest = await _digitalCertificatesApiClient.
                GetWithResponseCode<GetUserResponse>(new GetUsersByGovUkIdentifierApiRequest(request.GovUkIdentifier));

            if (userRequest?.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                userRequest.EnsureSuccessStatusCode();
            }

            return new GetUserResult
            {
                User = (Models.User)userRequest?.Body
            };
        }
    }
}
