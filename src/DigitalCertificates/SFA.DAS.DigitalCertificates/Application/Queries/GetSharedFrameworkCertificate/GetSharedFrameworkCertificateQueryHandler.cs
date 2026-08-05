using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharedFrameworkCertificate
{
    public class GetSharedFrameworkCertificateQueryHandler : IRequestHandler<GetSharedFrameworkCertificateQuery, GetSharedFrameworkCertificateQueryResult>
    {
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;

        public GetSharedFrameworkCertificateQueryHandler(IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient)
        {
            _assessorsApiClient = assessorsApiClient;
        }

        public async Task<GetSharedFrameworkCertificateQueryResult> Handle(GetSharedFrameworkCertificateQuery request, CancellationToken cancellationToken)
        {
            var response = await _assessorsApiClient.GetWithResponseCode<GetFrameworkCertificateResponse>(new GetFrameworkCertificateRequest(request.Id, false));

            if (response == null || response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var body = response.Body;

            var result = (GetSharedFrameworkCertificateQueryResult)body;

            return result;
        }
    }
}
