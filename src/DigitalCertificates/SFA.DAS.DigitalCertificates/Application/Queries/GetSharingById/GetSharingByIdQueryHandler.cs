using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
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
            var apiResponse = await _digitalCertificatesApiClient.GetWithResponseCode<GetSharingByIdResponse>(new GetSharingByIdRequest(request.SharingId, request.Limit));

            if (apiResponse?.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                apiResponse.EnsureSuccessStatusCode();
            }

            return new GetSharingByIdQueryResult { Response = apiResponse?.Body };
        }
    }
}
