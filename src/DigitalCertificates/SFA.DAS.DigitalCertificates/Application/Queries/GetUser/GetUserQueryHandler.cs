using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using User = SFA.DAS.DigitalCertificates.InnerApi.Responses.User;

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
            ApiResponse<User> userRequest = await _digitalCertificatesApiClient.
                GetWithResponseCode<User>(new GetUserRequest(request.GovUkIdentifier));

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
