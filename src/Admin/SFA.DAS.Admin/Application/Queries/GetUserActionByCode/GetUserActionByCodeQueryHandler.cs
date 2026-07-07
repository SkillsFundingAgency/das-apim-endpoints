using MediatR;
using SFA.DAS.Admin.InnerApi.Requests;
using SFA.DAS.Admin.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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
            var apiResponse = await _digitalCertificatesApiClient.GetWithResponseCode<GetUserActionByCodeResponse>(new GetUserActionByCodeRequest(request.Code));

            if (apiResponse?.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            apiResponse?.EnsureSuccessStatusCode();

            var body = apiResponse?.Body;

            if (body == null) return null;

            return body;
        }
    }
}
