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

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetCertificateById
{
    public class GetCertificateByIdQueryHandler : IRequestHandler<GetCertificateByIdQuery, GetCertificateByIdQueryResult>
    {
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;

        public GetCertificateByIdQueryHandler(IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient)
        {
            _assessorsApiClient = assessorsApiClient;
        }

        public async Task<GetCertificateByIdQueryResult> Handle(GetCertificateByIdQuery request, CancellationToken cancellationToken)
        {
            var certificateResponse = await _assessorsApiClient.GetWithResponseCode<GetCertificateByIdResponse>(new GetCertificateByIdRequest(request.Id, true));

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

            var result = (GetCertificateByIdQueryResult)certificate;
            result.AssessorName = assessorName;

            return result;
        }
    }
}
