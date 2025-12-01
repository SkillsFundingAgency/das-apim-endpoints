using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharings
{
    public class GetCertificateSharingDetailsQueryHandler : IRequestHandler<GetCertificateSharingDetailsQuery, GetCertificateSharingDetailsQueryResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public GetCertificateSharingDetailsQueryHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<GetCertificateSharingDetailsQueryResult> Handle(GetCertificateSharingDetailsQuery request, CancellationToken cancellationToken)
        {
            var apiResponse = await _digitalCertificatesApiClient.GetWithResponseCode<GetCertificateSharingDetailsResponse>(new GetCertificateSharingDetailsRequest(request.UserId, request.CertificateId, request.Limit));

            apiResponse.EnsureSuccessStatusCode();

            return new GetCertificateSharingDetailsQueryResult { Response = apiResponse?.Body };
        }
    }
}
