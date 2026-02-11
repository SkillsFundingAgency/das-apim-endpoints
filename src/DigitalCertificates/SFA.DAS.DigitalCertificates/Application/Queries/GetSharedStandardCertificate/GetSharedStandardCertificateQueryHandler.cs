using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharedStandardCertificate
{
    public class GetSharedStandardCertificateQueryHandler : IRequestHandler<GetSharedStandardCertificateQuery, GetSharedStandardCertificateQueryResult>
    {
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;

        public GetSharedStandardCertificateQueryHandler(IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient)
        {
            _assessorsApiClient = assessorsApiClient;
        }

        public async Task<GetSharedStandardCertificateQueryResult> Handle(GetSharedStandardCertificateQuery request, CancellationToken cancellationToken)
        {
            var certificateResponse = await _assessorsApiClient.GetWithResponseCode<GetStandardCertificateResponse>(new GetStandardCertificateRequest(request.Id, false));

            if (certificateResponse == null || certificateResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            certificateResponse.EnsureSuccessStatusCode();

            var certificate = certificateResponse.Body;

            var result = (GetSharedStandardCertificateQueryResult)certificate;

            return result;
        }
    }
}
