using MediatR;
using RestEase;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;

namespace SFA.DAS.ProviderPR.Application.Queries.GetRelationshipByEmail;
public class GetRelationshipByEmailQueryHandler(IAccountsApiClient<AccountsConfiguration> accountsApiClient, IProviderRelationshipsApiRestClient providerRelationshipsApiClient) : IRequestHandler<GetRelationshipByEmailQuery, GetRelationshipByEmailQueryResult>
{
    public async Task<GetRelationshipByEmailQueryResult> Handle(GetRelationshipByEmailQuery request, CancellationToken cancellationToken)
    {
        var queryResult =
            new GetRelationshipByEmailQueryResult();

        var result = await accountsApiClient.GetWithResponseCode<GetUserByEmailResponse>(new GetUserByEmailRequest(request.Email));

        if (result.StatusCode == HttpStatusCode.NotFound)
        {
            return queryResult;
        }

        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"Error calling get user by email for {request.Email}");
        }

        var userRef = result.Body.Ref;

        var accounts = await accountsApiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(userRef.ToString()));

        if (!accounts.Any())
        {
            return queryResult;
        }

        queryResult.HasUserAccount = true;
        queryResult.HasOneEmployerAccount = false;

        if (accounts.Count() > 1)
        {
            return queryResult;
        }

        queryResult.HasOneEmployerAccount = true;

        var account = accounts.First();
        queryResult.AccountId = account.AccountId;

        var accountLegalEntities = await accountsApiClient.GetAll<GetAccountLegalEntityResponse>(new GetAccountLegalEntitiesRequest(queryResult.AccountId.Value));

        queryResult.HasOneLegalEntity = accountLegalEntities.Count() == 1;

        if (!queryResult.HasOneLegalEntity.Value)
        {
            return queryResult;
        }

        var legalEntity = accountLegalEntities.First();

        queryResult.AccountLegalEntityPublicHashedId = legalEntity.AccountLegalEntityPublicHashedId;
        queryResult.AccountLegalEntityId = legalEntity.AccountLegalEntityId;
        queryResult.AccountLegalEntityName = legalEntity.Name;
        queryResult.HasRelationship = false;

        Response<GetRelationshipResponse> relationshipResponse = await providerRelationshipsApiClient.GetRelationship(request.Ukprn, queryResult.AccountLegalEntityId.Value, cancellationToken);

        if (relationshipResponse.ResponseMessage.StatusCode == HttpStatusCode.OK)
        {
            queryResult.HasRelationship = true;
            queryResult.Operations = relationshipResponse.GetContent().Operations.ToList();
        }

        return queryResult;
    }
}
