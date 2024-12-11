using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using RestEase;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PensionRegulator;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PensionsRegulator;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationshipsByUkprnPayeAorn;

public class GetRelationshipByUkprnPayeAornQueryHandler(IPensionRegulatorApiClient
    <PensionRegulatorApiConfiguration> pensionsRegulatorApiClient,
    IProviderRelationshipsApiRestClient providerRelationshipsApiClient,
    IAccountsApiClient<AccountsConfiguration> accountsApiClient,
    ILogger<GetRelationshipByUkprnPayeAornQueryHandler> logger)
    : IRequestHandler<GetRelationshipsByUkprnPayeAornQuery, GetRelationshipsByUkprnPayeAornResult>
{
    public const string TprStatusNotClosed = "Not Closed";
    public async Task<GetRelationshipsByUkprnPayeAornResult> Handle(GetRelationshipsByUkprnPayeAornQuery request, CancellationToken cancellationToken)
    {
        var queryResult =
            new GetRelationshipsByUkprnPayeAornResult();

        var isRequestPresent = await IsRequestPresent(request.Ukprn, request.Paye, cancellationToken);
        if (isRequestPresent)
        {
            logger.LogInformation("Found an active request for Ukprn:{Ukprn} and Paye:{Paye}", request.Ukprn, request.Paye);
            queryResult.HasActiveRequest = true;
            return queryResult;
        }

        var pensionRegulatorOrganisation = await GetOrganisationDetailsFromTpr(request.Aorn, request.Paye);

        queryResult.HasInvalidPaye = pensionRegulatorOrganisation is null;

        if (queryResult.HasInvalidPaye.Value)
        {
            return queryResult;
        }

        queryResult.Organisation = GetPensionRegulatorOrganisationDetails(pensionRegulatorOrganisation!);

        var accountHistoryFromPayeRefResult =
            await accountsApiClient.GetWithResponseCode<AccountHistory>(new GetPayeSchemeAccountByRefRequest(request.Paye));

        if (accountHistoryFromPayeRefResult.StatusCode != HttpStatusCode.OK)
        {
            logger.LogInformation("Account history not found for Paye:{Paye}", request.Paye);
            return queryResult;
        }

        AccountHistory accountDetails = accountHistoryFromPayeRefResult.Body;

        var accountId = accountDetails.AccountId;

        queryResult.Account = BuildAccountDetails(accountDetails);

        queryResult.HasOneLegalEntity = false;

        var accountLegalEntities = await accountsApiClient.GetAll<GetAccountLegalEntityResponse>(new GetAccountLegalEntitiesRequest(accountId));

        if (accountLegalEntities.Count() != 1) return queryResult;

        UpdateSingleLegalEntityDetails(queryResult, accountLegalEntities);

        Response<GetRelationshipResponse> relationshipResponse = await providerRelationshipsApiClient.GetRelationship(request.Ukprn, queryResult.AccountLegalEntityId!.Value, cancellationToken);

        if (relationshipResponse.ResponseMessage.StatusCode == HttpStatusCode.OK)
        {
            logger.LogInformation("Request not found for Ukprn:{Ukprn} and AccountLegalEntity {AccountLegalEntityId}", request.Ukprn, queryResult.AccountLegalEntityId!.Value);
            queryResult.HasRelationship = true;
            queryResult.Operations = relationshipResponse.GetContent().Operations.ToList();
        }

        return queryResult;
    }

    private static void UpdateSingleLegalEntityDetails(GetRelationshipsByUkprnPayeAornResult queryResult,
        IEnumerable<GetAccountLegalEntityResponse> accountLegalEntities)
    {
        queryResult.HasOneLegalEntity = true;
        var legalEntity = accountLegalEntities.First();
        queryResult.AccountLegalEntityId = legalEntity.AccountLegalEntityId;
        queryResult.AccountLegalEntityName = legalEntity.Name;
        queryResult.HasRelationship = false;
    }

    private static AccountDetails BuildAccountDetails(AccountHistory accountDetails)
    {
        return new AccountDetails
        {
            AccountId = accountDetails.AccountId,
            AddedDate = accountDetails.AddedDate,
            RemovedDate = accountDetails.RemovedDate
        };
    }

    private async Task<bool> IsRequestPresent(long ukprn, string paye, CancellationToken cancellationToken)
    {
        Response<GetRequestByUkprnAndPayeResponse?> res = await providerRelationshipsApiClient.GetRequestByUkprnAndPaye(ukprn, paye,
            cancellationToken);

        return res.ResponseMessage.StatusCode == HttpStatusCode.OK;
    }

    private static OrganisationDetails GetPensionRegulatorOrganisationDetails(PensionRegulatorOrganisation pensionRegulatorOrganisation)
        => new OrganisationDetails()
        {
            Name = pensionRegulatorOrganisation.Name,
            Address = new AddressDetails
            {
                Line1 = pensionRegulatorOrganisation.Address.Line1,
                Line2 = pensionRegulatorOrganisation.Address.Line2,
                Line3 = pensionRegulatorOrganisation.Address.Line3,
                Line4 = pensionRegulatorOrganisation.Address.Line4,
                Line5 = pensionRegulatorOrganisation.Address.Line5,
                Postcode = pensionRegulatorOrganisation.Address.Postcode
            }
        };

    private async Task<PensionRegulatorOrganisation?> GetOrganisationDetailsFromTpr(string aorn, string paye)
    {
        var pensionRegulatorResponse = await pensionsRegulatorApiClient.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(
                new GetPensionsRegulatorOrganisationsRequest(aorn, paye));

        if (pensionRegulatorResponse == null || pensionRegulatorResponse.StatusCode != HttpStatusCode.OK || !pensionRegulatorResponse.Body.Any())
        {
            logger.LogInformation("Did not find any organisation in TPR with AORN:{Aorn} and Paye:{Paye}", aorn, paye);
            return null;
        }

        return pensionRegulatorResponse.Body.FirstOrDefault(p => p.Status.Equals(TprStatusNotClosed, StringComparison.OrdinalIgnoreCase));
    }
}
