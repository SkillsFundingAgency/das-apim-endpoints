using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.Recruit.Application.Queries.GetAllAccountLegalEntities
{
    public class GetAllAccountLegalEntitiesQueryHandler(IAccountsApiClient<AccountsConfiguration> apiClient)
        : IRequestHandler<GetAllAccountLegalEntitiesQuery, GetAllAccountLegalEntitiesQueryResult>
    {
        public async Task<GetAllAccountLegalEntitiesQueryResult> Handle(GetAllAccountLegalEntitiesQuery request, CancellationToken cancellationToken)
        {
            var response =
                await apiClient.PostWithResponseCode<GetAllAccountLegalEntitiesApiResponse>(
                    new GetAllAccountLegalEntitiesApiRequest(new GetAllAccountLegalEntitiesApiRequest.GetAllAccountLegalEntitiesApiRequestData
                    {
                        SearchTerm = request.SearchTerm,
                        AccountIds = request.AccountIds,
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize,
                        SortColumn = request.SortColumn,
                        IsAscending = request.IsAscending
                    }));

            response.EnsureSuccessStatusCode();

            return response.Body;
        }
    }
}
