using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.VacanciesManage.Application.Providers.Queries.GetProviderAccountLegalEntities;

public class GetProviderAccountLegalEntitiesQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient)
    : IRequestHandler<GetProviderAccountLegalEntitiesQuery, GetProviderAccountLegalEntitiesQueryResponse>
{
    public async Task<GetProviderAccountLegalEntitiesQueryResponse> Handle(GetProviderAccountLegalEntitiesQuery request, CancellationToken cancellationToken)
    {
        var response =
            await apiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                new GetProviderAccountLegalEntitiesRequest(request.Ukprn, [
                    Operation.Recruitment,
                    Operation.RecruitmentRequiresReview
                ]));

        return new GetProviderAccountLegalEntitiesQueryResponse
        {
            ProviderAccountLegalEntities = response.AccountProviderLegalEntities
        };
    }
}