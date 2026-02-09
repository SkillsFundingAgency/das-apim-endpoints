using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.DigitalCertificates.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
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
            var emailSharingTask = _digitalCertificatesApiClient.GetWithResponseCode<GetSharingByEmailLinkCodeResponse>(new GetSharingByEmailLinkCodeRequest(request.Code));
            var directSharingTask = _digitalCertificatesApiClient.GetWithResponseCode<GetSharingByLinkCodeResponse>(new GetSharingByLinkCodeRequest(request.Code));

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
                    CertificateType = body.CertificateType,
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
                    CertificateType = body.CertificateType,
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
