using MediatR;
using RestEase;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PayeSchemes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;

namespace SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationshipByEmail;
public class GetRelationshipByEmailQueryHandler(IAccountsApiClient<AccountsConfiguration> _accountsApiClient, IProviderRelationshipsApiRestClient _providerRelationshipsApiClient) : IRequestHandler<GetRelationshipByEmailQuery, GetRelationshipByEmailQueryResult>
{
    public async Task<GetRelationshipByEmailQueryResult> Handle(GetRelationshipByEmailQuery request, CancellationToken cancellationToken)
    {
        var queryResult =
            new GetRelationshipByEmailQueryResult();

        var res = await _providerRelationshipsApiClient.GetRequestByUkprnAndEmail(request.Ukprn, request.Email, cancellationToken);

        var isRequestPresent = IsRequestPresent(res!, request.Ukprn, request.Email);

        if (isRequestPresent)
        {
            queryResult.RequestId = res.GetContent()!.RequestId;
            queryResult.HasActiveRequest = true;
            return queryResult;
        }

        queryResult.HasUserAccount = false;

        var result = await _accountsApiClient.GetWithResponseCode<GetUserByEmailResponse>(new GetUserByEmailRequest(request.Email));

        if (result.StatusCode == HttpStatusCode.NotFound)
        {
            return queryResult;
        }

        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"Error calling get user by email for {request.Email}");
        }

        var userRef = result.Body.Ref;

        var accounts = await _accountsApiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(userRef.ToString()));

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

        var accountLegalEntities = await _accountsApiClient.GetAll<GetAccountLegalEntityResponse>(new GetAccountLegalEntitiesRequest(queryResult.AccountId.Value));

        queryResult.HasOneLegalEntity = accountLegalEntities.Count() == 1;

        if (!queryResult.HasOneLegalEntity.Value)
        {
            return queryResult;
        }

        var legalEntity = accountLegalEntities.First();

        queryResult.AccountLegalEntityPublicHashedId = legalEntity.AccountLegalEntityPublicHashedId;
        queryResult.AccountLegalEntityId = legalEntity.AccountLegalEntityId;
        queryResult.AccountLegalEntityName = legalEntity.Name;

        var payeSchemes = await _accountsApiClient.GetAll<PayeScheme>(new GetAccountPayeSchemesRequest(queryResult.AccountId.Value));
        queryResult.Paye = payeSchemes.Single().Id;

        var existingRequestCheck = await _providerRelationshipsApiClient.GetRequestByUkprnAndAccountLegalEntityId(request.Ukprn, legalEntity.AccountLegalEntityId, cancellationToken);

        var isExistingRequestPresent = IsExistingRequestPresent(existingRequestCheck!, request.Ukprn, legalEntity.AccountLegalEntityId);

        if (isExistingRequestPresent)
        {
            queryResult.RequestId = existingRequestCheck.GetContent()!.RequestId;
            queryResult.HasActiveRequest = true;
            return queryResult;
        }

        Response<GetRelationshipResponse> relationshipResponse = await _providerRelationshipsApiClient.GetRelationship(request.Ukprn, queryResult.AccountLegalEntityId.Value, cancellationToken);

        if (relationshipResponse.ResponseMessage.StatusCode == HttpStatusCode.OK)
        {
            queryResult.HasRelationship = true;
            queryResult.Operations = relationshipResponse.GetContent().Operations.ToList();
        }
        else
        {
            queryResult.HasRelationship = false;
        }

        return queryResult;
    }

    private static bool IsRequestPresent(Response<GetRequestByUkprnAndEmailResponse> res, long ukprn, string email)
    {
        return res.ResponseMessage.StatusCode switch
        {
            HttpStatusCode.OK => true,
            HttpStatusCode.NotFound => false,
            _ => throw new InvalidOperationException(
                $"Provider PR API threw unexpected response for ukprn {ukprn} and email {email}")
        };
    }

    private static bool IsExistingRequestPresent(Response<GetRequestByUkprnAndAccountLegalEntityIdResponse> res, long ukprn, long accountLegalEntityId)
    {
        return res.ResponseMessage.StatusCode switch
        {
            HttpStatusCode.OK => true,
            HttpStatusCode.NotFound => false,
            _ => throw new InvalidOperationException(
                $"Provider PR API threw unexpected response for ukprn {ukprn} and accountLegalEntityId {accountLegalEntityId}")
        };
    }
}
