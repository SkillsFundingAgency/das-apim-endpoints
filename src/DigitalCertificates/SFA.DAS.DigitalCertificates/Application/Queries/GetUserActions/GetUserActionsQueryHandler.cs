using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions
{
    public class GetUserActionsQueryHandler : IRequestHandler<GetUserActionsQuery, GetUserActionsQueryResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public GetUserActionsQueryHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<GetUserActionsQueryResult> Handle(GetUserActionsQuery request, CancellationToken cancellationToken)
        {
            var apiResponse = await _digitalCertificatesApiClient.GetWithResponseCode<GetUserActionsResponse>(new GetUserActionsRequest(request.UserId));

            if (apiResponse?.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                apiResponse?.EnsureSuccessStatusCode();
            }

            var responseBody = apiResponse?.Body ?? new GetUserActionsResponse { UserActions = new System.Collections.Generic.List<UserAction>() };

            return responseBody;
        }
    }
}
