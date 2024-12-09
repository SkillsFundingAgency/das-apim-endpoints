using System.Net;
using MediatR;
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
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationshipsByUkprnPayeAorn;
public class GetRelationshipByUkprnPayeAornQueryHandler(IPensionRegulatorApiClient<PensionRegulatorApiConfiguration> pensionsRegulatorApiClient, IProviderRelationshipsApiRestClient providerRelationshipsApiClient, IAccountsApiClient<AccountsConfiguration> accountsApiClient) : IRequestHandler<GetRelationshipsByUkprnPayeAornQuery, GetRelationshipsByUkprnPayeAornResult>
{
    public async Task<GetRelationshipsByUkprnPayeAornResult> Handle(GetRelationshipsByUkprnPayeAornQuery request, CancellationToken cancellationToken)
    {
        var queryResult =
            new GetRelationshipsByUkprnPayeAornResult();

        var res = await providerRelationshipsApiClient.GetRequestByUkprnAndPaye(request.Ukprn, request.Paye,
            cancellationToken);

        var isRequestPresent = IsRequestPresent(res!, request.Ukprn, request.Paye);

        if (isRequestPresent)
        {
            queryResult.HasActiveRequest = true;
            return queryResult;
        }

        var pensionRegulatorResponse =
            await pensionsRegulatorApiClient.GetWithResponseCode<List<PensionRegulatorOrganisation>>(
                new GetPensionsRegulatorOrganisationsRequest(request.Aorn, request.Paye));

        var hasInvalidPaye = CheckPensionOrganisationsHaveInvalidPaye(pensionRegulatorResponse);

        queryResult.HasInvalidPaye = hasInvalidPaye;

        if (hasInvalidPaye)
        {
            return queryResult;
        }

        queryResult.Organisation = GetPensionRegulatorOrganisationDetails(pensionRegulatorResponse.Body);

        var accountHistoryFromPayeRefResult =
            await accountsApiClient.GetWithResponseCode<AccountHistory>(new GetPayeSchemeAccountByRefRequest(request.Paye));

        if (accountHistoryFromPayeRefResult.StatusCode != HttpStatusCode.OK)
        {
            return queryResult;
        }

        AccountHistory accountDetails = accountHistoryFromPayeRefResult.Body;

        var accountId = accountDetails.AccountId;

        queryResult.Account = BuildAccountDetails(accountDetails);

        queryResult.HasOneLegalEntity = false;

        var accountLegalEntities = await accountsApiClient.GetAll<GetAccountLegalEntityResponse>(new GetAccountLegalEntitiesRequest(accountId));

        if (accountLegalEntities.Count() != 1) return queryResult;

        UpdateSingleLegalEntityDetails(queryResult, accountLegalEntities);

        Response<GetRelationshipResponse> relationshipResponse =
            await providerRelationshipsApiClient.GetRelationship(request.Ukprn,
                queryResult.AccountLegalEntityId!.Value, cancellationToken);

        if (relationshipResponse.ResponseMessage.StatusCode == HttpStatusCode.OK)
        {
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

    private static bool IsRequestPresent(Response<GetRequestByUkprnAndPayeResponse>? res, long ukprn, string paye)
    {
        if (res != null)
        {
            switch (res.ResponseMessage.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        GetRequestByUkprnAndPayeResponse getResponse = res.GetContent();
                        if (getResponse.RequestType != "CreateAccount") return false;
                        return true;

                    }
                case HttpStatusCode.NotFound:
                    return false;
            }
        }

        throw new InvalidOperationException(
            $"Pensions regulator API threw unexpected response for ukprn {ukprn} and paye {paye}");
    }

    private static OrganisationDetails? GetPensionRegulatorOrganisationDetails(List<PensionRegulatorOrganisation> pensionRegulatorOrganisations)
    {
        OrganisationDetails? organisationDetails = null;

        var pensionRegulatorOrganisation = pensionRegulatorOrganisations.Find(b => b.Status == "");

        if (pensionRegulatorOrganisation == null) return organisationDetails;


        organisationDetails = new OrganisationDetails
        {
            Name = pensionRegulatorOrganisation.Name
        };

        if (pensionRegulatorOrganisation.Address != null)
        {
            organisationDetails.Address = new AddressDetails
            {
                Line1 = pensionRegulatorOrganisation.Address.Line1,
                Line2 = pensionRegulatorOrganisation.Address.Line2,
                Line3 = pensionRegulatorOrganisation.Address.Line3,
                Line4 = pensionRegulatorOrganisation.Address.Line4,
                Line5 = pensionRegulatorOrganisation.Address.Line5,
                Postcode = pensionRegulatorOrganisation.Address.Postcode
            };
        }

        return organisationDetails;
    }

    private static bool CheckPensionOrganisationsHaveInvalidPaye(ApiResponse<List<PensionRegulatorOrganisation>> result)
    {
        if (result.StatusCode == HttpStatusCode.OK)
        {
            if (result.Body.TrueForAll(s => s.Status != ""))
            {
                return true;
            }

            return false;
        }

        return true;
    }
}
