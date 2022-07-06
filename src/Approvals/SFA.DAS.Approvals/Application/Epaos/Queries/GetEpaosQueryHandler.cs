using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Epaos.Queries
{
    public class GetEpaosQueryHandler : IRequestHandler<GetEpaosQuery, GetEpaosResult>
    {
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _apiClient;

        public GetEpaosQueryHandler (IAssessorsApiClient<AssessorsApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetEpaosResult> Handle(GetEpaosQuery request, CancellationToken cancellationToken)
        {
            var results = await _apiClient.GetAll<GetEpaosListItem>(new GetEpaosRequest());
            
            return new GetEpaosResult
            {
                Epaos = results.ToList()
            };
        }
    }
}