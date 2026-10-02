using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.DigitalCertificates.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetCertificates
{
    public class GetCertificatesQueryHandler : IRequestHandler<GetCertificatesQuery, GetCertificatesResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;

        public GetCertificatesQueryHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient, IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
            _assessorsApiClient = assessorsApiClient;
        }

        public async Task<GetCertificatesResult> Handle(GetCertificatesQuery request, CancellationToken cancellationToken)
        {
            var result = new GetCertificatesResult();

            ApiResponse<GetUserAuthorisationResponse> authorisationResponse = await _digitalCertificatesApiClient.
                GetWithResponseCode<GetUserAuthorisationResponse>(new GetUsersByUserIdAuthorisationApiRequest(request.UserId));

            if (authorisationResponse != null && authorisationResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                authorisationResponse.EnsureSuccessStatusCode();

                result.Authorisation = (UlnAuthorisation)authorisationResponse.Body.Authorisation;

                if (result.Authorisation != null)
                {
                    ApiResponse<GetCertificatesResponse> certificatesResponse = await _assessorsApiClient.
                        GetWithResponseCode<GetCertificatesResponse>(new GetCertificatesRequest(result.Authorisation.Uln));

                    if (certificatesResponse != null && certificatesResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
                    {
                        certificatesResponse.EnsureSuccessStatusCode();
                        result.Certificates = certificatesResponse.Body.Certificates;
                    }
                }
            }

            return result;
        }
    }
}
