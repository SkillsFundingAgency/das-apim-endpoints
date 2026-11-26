using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using System.Collections.Generic;
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
            var apiResponse = await _digitalCertificatesApiClient.GetWithResponseCode<GetUserActionsResponse>(new GetUsersByUserIdUserActionsApiRequest(request.UserId));

            if (apiResponse?.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                apiResponse?.EnsureSuccessStatusCode();
            }

            var responseBody = apiResponse?.Body ?? new GetUserActionsResponse { UserActions = new List<UserActionDetailDto>() };

            return responseBody;
        }
    }
}
