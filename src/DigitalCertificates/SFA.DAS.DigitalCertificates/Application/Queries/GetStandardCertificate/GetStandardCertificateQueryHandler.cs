using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetStandardCertificate
{
    public class GetStandardCertificateQueryHandler : IRequestHandler<GetStandardCertificateQuery, GetStandardCertificateQueryResult>
    {
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;

        public GetStandardCertificateQueryHandler(IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient)
        {
            _assessorsApiClient = assessorsApiClient;
        }

        public async Task<GetStandardCertificateQueryResult> Handle(GetStandardCertificateQuery request, CancellationToken cancellationToken)
        {
            var certificateResponse = await _assessorsApiClient.GetWithResponseCode<GetStandardCertificateResponse>(new GetStandardCertificateRequest(request.Id, true));

            if (certificateResponse == null || certificateResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            certificateResponse.EnsureSuccessStatusCode();

            var certificate = certificateResponse.Body;

            string assessorName = null;
            if (certificate.OrganisationId != Guid.Empty)
            {
                var orgResult = await _assessorsApiClient.GetWithResponseCode<GetOrganisationResponse>(new GetOrganisationRequest(certificate.OrganisationId));

                if (orgResult != null)
                {
                    assessorName = orgResult.Body?.EndPointAssessorName;
                }
            }

            var result = (GetStandardCertificateQueryResult)certificate;
            result.AssessorName = assessorName;

            return result;
        }
    }
}
