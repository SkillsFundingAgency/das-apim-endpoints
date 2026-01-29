using MediatR;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.Apprentices
{
    public class GetApprenticeByPersonalDetailQueryHandler : IRequestHandler<GetApprenticeByPersonalDetailQuery, GetApprenticeByPersonalDetailQueryResponse>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _apprenticeAccountsApiClient;

        public GetApprenticeByPersonalDetailQueryHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> apprenticeAccountsApiClient)
        => _apprenticeAccountsApiClient = apprenticeAccountsApiClient;

        public async Task<GetApprenticeByPersonalDetailQueryResponse> Handle(GetApprenticeByPersonalDetailQuery request, CancellationToken cancellationToken)
        {
            var response = await _apprenticeAccountsApiClient.GetWithResponseCode<List<Apprentice>>
                (new GetApprenticeByPersonalDetailsRequest(request.FirstName, request.LastName, request.DateOfBirth));

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            return new GetApprenticeByPersonalDetailQueryResponse
            {
                Apprentices = response.Body
            };
        }
    }    
}
