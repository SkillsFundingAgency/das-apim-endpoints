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

namespace SFA.DAS.ProviderPR.Application.Queries.GetEasUserByEmail;
public class GetEasUserByEmailQueryHandler(IAccountsApiClient<AccountsConfiguration> accountsApiClient, IProviderRelationshipsApiRestClient providerRelationshipsApiClient) : IRequestHandler<GetEasUserByEmailQuery, GetEasUserByEmailQueryResult>
{

    public async Task<GetEasUserByEmailQueryResult> Handle(GetEasUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var result = await accountsApiClient.Get<GetUserByEmailResponse>(new GetUserByEmailRequest(request.Email));

        var userRef = result?.Ref;

        if (userRef == null || userRef == Guid.Empty)
        {
            return new GetEasUserByEmailQueryResult { HasUserAccount = false };
        }

        var accounts = await accountsApiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(userRef.ToString()));

        if (accounts.Count() != 1)
        {
            return new GetEasUserByEmailQueryResult { HasUserAccount = true, HasOneEmployerAccount = false };
        }

        var account = accounts.FirstOrDefault();
        var hashedId = account!.EncodedAccountId;

        var accountLegalEntities = await accountsApiClient.GetAll<GetAccountLegalEntityResponse>(new GetAccountLegalEntitiesRequest(hashedId));

        var hasOneLegalEntity = accountLegalEntities.Count() == 1;

        if (!hasOneLegalEntity)
        {
            return new GetEasUserByEmailQueryResult { HasUserAccount = true, AccountId = account.AccountId, HasOneEmployerAccount = true, HasOneLegalEntity = false };
        }


        var legalEntity = accountLegalEntities.First();

        var accountLegalEntityPublicHashedId = legalEntity.AccountLegalEntityPublicHashedId;
        var accountLegalEntityId = legalEntity.AccountLegalEntityId;
        var accountLegalEntityName = legalEntity.Name;
        var hasRelationship = false;

        List<Operation>? operations = null;

        Response<GetRelationshipResponse> relationshipResponse = await providerRelationshipsApiClient.GetRelationship(request.Ukprn, accountLegalEntityId, cancellationToken);

        if (relationshipResponse.ResponseMessage.StatusCode == HttpStatusCode.OK)
        {
            hasRelationship = true;
            operations = relationshipResponse.GetContent().Operations.ToList();
        }

        return new GetEasUserByEmailQueryResult
        {
            HasUserAccount = true,
            AccountId = account.AccountId,
            HasOneEmployerAccount = true,
            HasOneLegalEntity = hasOneLegalEntity,
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId,
            AccountLegalEntityId = accountLegalEntityId,
            AccountLegalEntityName = accountLegalEntityName,
            HasRelationship = hasRelationship,
            Operations = operations
        };
    }
}
