using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.DigitalCertificates.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using User = SFA.DAS.DigitalCertificates.InnerApi.Responses.User;

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

            ApiResponse<GetAuthorisationResponse> authorisationResponse = await _digitalCertificatesApiClient.
                GetWithResponseCode<GetAuthorisationResponse>(new GetAuthorisationRequest(request.UserId));

            if (authorisationResponse != null && authorisationResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                authorisationResponse.EnsureSuccessStatusCode();
                result.Authorisation = authorisationResponse.Body.Authorisation;

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
