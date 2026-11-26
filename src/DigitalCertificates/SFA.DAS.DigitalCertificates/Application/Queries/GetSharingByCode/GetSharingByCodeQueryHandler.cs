using MediatR;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.DigitalCertificates.Models;
using SFA.DAS.Apim.Shared.Extensions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByCode
{
    public class GetSharingByCodeQueryHandler : IRequestHandler<GetSharingByCodeQuery, GetSharingByCodeQueryResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public GetSharingByCodeQueryHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<GetSharingByCodeQueryResult> Handle(GetSharingByCodeQuery request, CancellationToken cancellationToken)
        {
            var emailSharingTask = _digitalCertificatesApiClient.GetWithResponseCode<GetSharingByEmailLinkCodeResponse>(new GetSharingSharingemailEmaillinkcodeByEmailLinkCodeApiRequest(request.Code));
            var directSharingTask = _digitalCertificatesApiClient.GetWithResponseCode<GetSharingByLinkCodeResponse>(new GetSharingLinkcodeByLinkCodeApiRequest(request.Code));

            await Task.WhenAll(emailSharingTask, directSharingTask);

            var emailSharingResponse = emailSharingTask.Result;
            var directSharingResponse = directSharingTask.Result;

            var result = new GetSharingByCodeQueryResult();

            var emailSharingFound = emailSharingResponse != null && emailSharingResponse.StatusCode != HttpStatusCode.NotFound;
            var directSharingFound = directSharingResponse != null && directSharingResponse.StatusCode != HttpStatusCode.NotFound;

            SharingByCode responseModel = null;

            if (emailSharingFound)
            {
                emailSharingResponse.EnsureSuccessStatusCode();
                var body = emailSharingResponse.Body;
                responseModel = new SharingByCode
                {
                    CertificateId = body.CertificateId,
                    CertificateType = body.CertificateType.ToString(),
                    ExpiryTime = body.ExpiryTime,
                    SharingEmailId = body.SharingEmailId
                };
            }

            if (directSharingFound)
            {
                directSharingResponse.EnsureSuccessStatusCode();
                var body = directSharingResponse.Body;

                responseModel = new SharingByCode
                {
                    CertificateId = body.CertificateId,
                    CertificateType = body.CertificateType.ToString(),
                    ExpiryTime = body.ExpiryTime,
                    SharingId = body.SharingId
                };
            }

            result.Response = responseModel;
            result.BothFound = emailSharingFound && directSharingFound;

            return result;
        }
    }
}
