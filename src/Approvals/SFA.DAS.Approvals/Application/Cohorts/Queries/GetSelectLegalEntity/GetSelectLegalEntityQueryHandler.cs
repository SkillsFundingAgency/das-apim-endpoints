using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetSelectLegalEntity
{
    public class GetSelectLegalEntityQueryHandler(
        IAccountsApiClient<AccountsConfiguration> _apiClient)
        : IRequestHandler<GetSelectLegalEntityQuery, GetSelectLegalEntityQueryResult>
    {
        public async Task<GetSelectLegalEntityQueryResult> Handle(GetSelectLegalEntityQuery request, CancellationToken cancellationToken)
        {
            var response =
                await _apiClient.GetAll<GetLegalEntitiesForAccountResponseItem>(
                    new GetAccountLegalEntitiesRequest(request.AccountId));

            if (response == null)
            {
                return new GetSelectLegalEntityQueryResult();
            }

            return new GetSelectLegalEntityQueryResult()
            {
                LegalEntities = response.ToList()
            };
        }
    }
}