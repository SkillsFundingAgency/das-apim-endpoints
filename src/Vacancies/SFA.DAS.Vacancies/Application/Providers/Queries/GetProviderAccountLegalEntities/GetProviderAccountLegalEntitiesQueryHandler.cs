using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.Vacancies.Application.Providers.Queries.GetProviderAccountLegalEntities
{
    public class GetProviderAccountLegalEntitiesQueryHandler : IRequestHandler<GetProviderAccountLegalEntitiesQuery, GetProviderAccountLegalEntitiesQueryResponse>
    {
        private readonly IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _apiClient;

        public GetProviderAccountLegalEntitiesQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetProviderAccountLegalEntitiesQueryResponse> Handle(GetProviderAccountLegalEntitiesQuery request, CancellationToken cancellationToken)
        {
            var response =
                await _apiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                    new GetProviderAccountLegalEntitiesRequest(request.Ukprn, new List<Operation>
                        {
                            Operation.Recruitment,
                            Operation.RecruitmentRequiresReview
                        }));

            return new GetProviderAccountLegalEntitiesQueryResponse
            {
                ProviderAccountLegalEntities = response.AccountProviderLegalEntities
            };
        }
    }
}