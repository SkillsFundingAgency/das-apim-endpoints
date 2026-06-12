using MediatR;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using GetUserActionByCodeResponse = SFA.DAS.DigitalCertificates.Contracts.ApiResponses.GetUserActionByCodeQueryResult;

namespace SFA.DAS.Admin.Application.Queries.GetUserActionByCode
{
    public class GetUserActionByCodeQueryHandler : IRequestHandler<GetUserActionByCodeQuery, GetUserActionByCodeQueryResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public GetUserActionByCodeQueryHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<GetUserActionByCodeQueryResult> Handle(GetUserActionByCodeQuery request, CancellationToken cancellationToken)
        {
            var apiResponse = await _digitalCertificatesApiClient.GetWithResponseCode<GetUserActionByCodeResponse>(new GetUsersUseractionsByCodeApiRequest(request.Code));

            if (apiResponse?.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            apiResponse?.EnsureSuccessStatusCode();

            var body = apiResponse?.Body;

            if (body == null) return null;

            return (GetUserActionByCodeQueryResult)body;
        }
    }
}
