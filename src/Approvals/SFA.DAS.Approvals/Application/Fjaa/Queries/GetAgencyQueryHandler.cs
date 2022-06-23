using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Fjaa.Queries
{
    public class GetFjaaQueryHandler : IRequestHandler<GetAgencyQuery, GetAgencyResult>
    {
        private readonly IFjaaApiClient<FjaaApiConfiguration> _apiClient;

        public GetFjaaQueryHandler(IFjaaApiClient<FjaaApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetAgencyResult> Handle(GetAgencyQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetAgencyResponse>(new GetAgencyRequest(request.LegalEntityId));

            if (result == null) 
                return null;

            return new GetAgencyResult
            {
                LegalEntityId = result.LegalEntityId
            };
        }
    }
}