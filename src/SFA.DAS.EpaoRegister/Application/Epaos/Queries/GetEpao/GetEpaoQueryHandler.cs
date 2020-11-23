using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EpaoRegister.InnerApi.Requests;
using SFA.DAS.EpaoRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpao
{
    public class GetEpaoQueryHandler : IRequestHandler<GetEpaoQuery, GetEpaoResult>
    {
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;

        public GetEpaoQueryHandler(IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient)
        {
            _assessorsApiClient = assessorsApiClient;
        }

        public async Task<GetEpaoResult> Handle(GetEpaoQuery request, CancellationToken cancellationToken)
        {
            var apiRequest = new GetEpaoRequest{EpaoId = request.EpaoId};
            var searchItems = (await _assessorsApiClient.GetAll<SearchEpaosListItem>(apiRequest))?.ToList();
            
            if (searchItems == null || searchItems.Count > 1)
            {
                throw new ArgumentException();
            }

            return new GetEpaoResult {Epao = searchItems[0]};
        }
    }
}