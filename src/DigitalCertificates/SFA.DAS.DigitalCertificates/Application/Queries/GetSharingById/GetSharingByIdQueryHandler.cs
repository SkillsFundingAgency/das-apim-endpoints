using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById
{
    public class GetSharingByIdQueryHandler : IRequestHandler<GetSharingByIdQuery, GetSharingByIdQueryResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public GetSharingByIdQueryHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<GetSharingByIdQueryResult> Handle(GetSharingByIdQuery request, CancellationToken cancellationToken)
        {
            var apiResponse = await _digitalCertificatesApiClient.GetWithResponseCode<GetSharingByIdResponse>(new GetSharingByIdApiRequest(request.SharingId, request.Limit));

            if (apiResponse?.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                apiResponse.EnsureSuccessStatusCode();
            }

            return new GetSharingByIdQueryResult { Response = apiResponse?.Body };
        }
    }
}
