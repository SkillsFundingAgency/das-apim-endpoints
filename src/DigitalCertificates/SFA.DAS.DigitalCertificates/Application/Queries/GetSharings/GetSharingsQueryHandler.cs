using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharings
{
    public class GetSharingsQueryHandler : IRequestHandler<GetSharingsQuery, GetSharingsQueryResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public GetSharingsQueryHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<GetSharingsQueryResult> Handle(GetSharingsQuery request, CancellationToken cancellationToken)
        {
            var apiResponse = await _digitalCertificatesApiClient.GetWithResponseCode<GetSharingsResponse>(new GetUsersByUserIdSharingsApiRequest(request.UserId, request.CertificateId, request.Limit));

            if (apiResponse?.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                apiResponse.EnsureSuccessStatusCode();
            }

            return new GetSharingsQueryResult { Response = apiResponse?.Body };
        }
    }
}
