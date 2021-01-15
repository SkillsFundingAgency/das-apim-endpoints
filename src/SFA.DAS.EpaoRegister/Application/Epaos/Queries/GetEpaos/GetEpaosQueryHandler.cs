using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EpaoRegister.InnerApi.Requests;
using SFA.DAS.EpaoRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaos
{
    public class GetEpaosQueryHandler : IRequestHandler<GetEpaosQuery, GetEpaosResult>
    {
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;

        public GetEpaosQueryHandler(IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient)
        {
            _assessorsApiClient = assessorsApiClient;
        }

        public async Task<GetEpaosResult> Handle(GetEpaosQuery request, CancellationToken cancellationToken)
        {
            var apiRequest = new GetEpaosRequest();
            var epaos = (await _assessorsApiClient.GetAll<GetEpaosListItem>(apiRequest))
                .Where(item => item.Status == EpaoStatus.Live);

            return new GetEpaosResult {Epaos = epaos};
        }
    }
}