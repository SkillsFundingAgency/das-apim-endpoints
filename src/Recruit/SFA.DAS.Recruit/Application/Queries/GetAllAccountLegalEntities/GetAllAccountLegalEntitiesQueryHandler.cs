using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetAllAccountLegalEntities
{
    public class GetAllAccountLegalEntitiesQueryHandler(IAccountsApiClient<AccountsConfiguration> apiClient)
        : IRequestHandler<GetAllAccountLegalEntitiesQuery, GetAllAccountLegalEntitiesQueryResult>
    {
        public async Task<GetAllAccountLegalEntitiesQueryResult> Handle(GetAllAccountLegalEntitiesQuery request, CancellationToken cancellationToken)
        {
            var response =
                await apiClient.Get<GetAllAccountLegalEntitiesApiResponse>(
                    new GetAllAccountLegalEntitiesApiRequest(request.AccountId, request.PageNumber, request.PageSize, request.SortColumn, request.IsAscending));

            return response;
        }
    }
}
