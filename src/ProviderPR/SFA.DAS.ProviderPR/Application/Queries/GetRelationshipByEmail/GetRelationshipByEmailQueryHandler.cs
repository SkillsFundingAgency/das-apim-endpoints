using MediatR;
using RestEase;
using SFA.DAS.ProviderPR.Application.Queries.GetRelationship;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;
using System.Net;

namespace SFA.DAS.ProviderPR.Application.Queries.GetRelationshipByEmail;
public class GetRelationshipByEmailQueryHandler(IAccountsApiClient<AccountsConfiguration> accountsApiClient, IProviderRelationshipsApiRestClient providerRelationshipsApiClient) : IRequestHandler<GetRelationshipByEmailQuery, GetRelationshipByEmailQueryResult>
{
    public async Task<GetRelationshipByEmailQueryResult> Handle(GetRelationshipByEmailQuery request, CancellationToken cancellationToken)
    {
        var hasUserAccount = false;
        bool? hasOneEmployerAccount = null;
        bool? hasOneLegalEntity = null;

        var result = await accountsApiClient.Get<GetUserByEmailResponse>(new GetUserByEmailRequest(request.Email));

        if (result == null)
        {
            return new GetRelationshipByEmailQueryResult(hasUserAccount, hasOneEmployerAccount, null, hasOneLegalEntity);
        }

        hasUserAccount = true;
        var userRef = result.Ref;

        var accounts = await accountsApiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(userRef.ToString()));

        if (accounts.Count() > 1)
        {
            return new GetRelationshipByEmailQueryResult(hasUserAccount, false, null, hasOneLegalEntity);
        }

        hasOneEmployerAccount = true;

        var account = accounts.First();
        var accountId = account!.AccountId;

        var accountLegalEntities = await accountsApiClient.GetAll<GetAccountLegalEntityResponse>(new GetAccountLegalEntitiesRequest(accountId));

        hasOneLegalEntity = accountLegalEntities.Count() == 1;

        if (!hasOneLegalEntity.Value)
        {
            return new GetRelationshipByEmailQueryResult(hasUserAccount, hasOneEmployerAccount, account.AccountId, hasOneLegalEntity);
        }

        var legalEntity = accountLegalEntities.First();

        var accountLegalEntityPublicHashedId = legalEntity.AccountLegalEntityPublicHashedId;
        var accountLegalEntityId = legalEntity.AccountLegalEntityId;
        var accountLegalEntityName = legalEntity.Name;
        var hasRelationship = false;
        List<Operation>? operations = new List<Operation>();

        Response<GetRelationshipResponse> relationshipResponse = await providerRelationshipsApiClient.GetRelationship(request.Ukprn, accountLegalEntityId, cancellationToken);

        if (relationshipResponse.ResponseMessage.StatusCode == HttpStatusCode.OK)
        {
            hasRelationship = true;
            operations = relationshipResponse.GetContent().Operations.ToList();
        }

        return new GetRelationshipByEmailQueryResult(hasUserAccount,
            hasOneEmployerAccount,
            account.AccountId,
            hasOneLegalEntity,
            accountLegalEntityPublicHashedId,
            accountLegalEntityId,
            accountLegalEntityName,
            hasRelationship,
            operations);
    }
}
