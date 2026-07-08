using MediatR;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Admin.InnerApi.Requests.Assessor;
using DigitalCertificatesApiClient = SFA.DAS.DigitalCertificates.Contracts.Client.IDigitalCertificatesApiClient<SFA.DAS.DigitalCertificates.Contracts.Client.DigitalCertificatesApiConfiguration>;
using SFA.DAS.Admin.Enums;
using SFA.DAS.Admin.InnerApi.Responses.Assessor;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GetUserActionByCodeResponse = SFA.DAS.DigitalCertificates.Contracts.ApiResponses.GetUserActionByCodeQueryResult;
using SFA.DAS.Apim.Shared.Extensions;

namespace SFA.DAS.Admin.Application.Queries.GetUserActionByCode
{
    public class GetUserActionByCodeQueryHandler : IRequestHandler<GetUserActionByCodeQuery, GetUserActionByCodeQueryResult>
    {
        private readonly DigitalCertificatesApiClient _digitalCertificatesApiClient;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;

        public GetUserActionByCodeQueryHandler(DigitalCertificatesApiClient digitalCertificatesApiClient, IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
            _assessorsApiClient = assessorsApiClient;
        }

        public async Task<GetUserActionByCodeQueryResult> Handle(GetUserActionByCodeQuery request, CancellationToken cancellationToken)
        {
            var apiResponse = await _digitalCertificatesApiClient.GetWithResponseCode<GetUserActionByCodeResponse>(
                new GetUsersUseractionsByCodeApiRequest(request.Code));

            if (apiResponse?.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            apiResponse?.EnsureSuccessStatusCode();

            if (apiResponse?.Body == null)
            {
                return null;
            }

            var result = (GetUserActionByCodeQueryResult)apiResponse.Body;

            if (result.CertificateType == CertificateType.Standard.ToString() && result.CertificateId.HasValue)
            {
                var certResponse = await _assessorsApiClient
                    .GetWithResponseCode<GetStandardCertificateResponse>(
                        new GetStandardCertificateRequest(result.CertificateId.Value, false));

                if (certResponse?.StatusCode == HttpStatusCode.NotFound)
                {
                    return result;
                }

                certResponse?.EnsureSuccessStatusCode();

                result.StandardCode = certResponse?.Body?.StandardCode;
            }

            return result;
        }
    }
}
