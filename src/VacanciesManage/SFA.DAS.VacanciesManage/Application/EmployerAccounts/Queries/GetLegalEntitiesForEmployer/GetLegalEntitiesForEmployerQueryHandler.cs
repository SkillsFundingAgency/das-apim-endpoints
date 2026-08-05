using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.VacanciesManage.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer;

public class GetLegalEntitiesForEmployerQueryHandler(IAccountsApiClient<AccountsConfiguration> accountsApiClient)
    : IRequestHandler<GetLegalEntitiesForEmployerQuery, GetLegalEntitiesForEmployerResult>
{
    public async Task<GetLegalEntitiesForEmployerResult> Handle(GetLegalEntitiesForEmployerQuery request, CancellationToken cancellationToken)
    {
        var resourceListResponse = await accountsApiClient.Get<AccountDetail>(
            new GetAllEmployerAccountLegalEntitiesRequest(request.EncodedAccountId));

        var legalEntities = new List<GetEmployerAccountLegalEntityItem>();

        if (resourceListResponse == null)
        {
            return new GetLegalEntitiesForEmployerResult
            {
                LegalEntities = legalEntities
            };
        }

        foreach (var resource in resourceListResponse.LegalEntities)
        {
            var accountLegalEntityItem = await accountsApiClient.Get<GetEmployerAccountLegalEntityItem>(
                new GetEmployerAccountLegalEntityRequest(resource.Href));

            if (accountLegalEntityItem.Agreements.Any(c => c.Status == EmployerAgreementStatus.Signed))
            {
                accountLegalEntityItem.AccountPublicHashedId = request.EncodedAccountId;
                accountLegalEntityItem.AccountName = resourceListResponse.DasAccountName;
                legalEntities.Add(accountLegalEntityItem);
            }

        }

        return new GetLegalEntitiesForEmployerResult
        {
            LegalEntities = legalEntities
        };
    }
}