using System.Collections.Concurrent;
using SFA.DAS.LearnerData.Application.GetProviderRelationships;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Rofjaa;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Rofjaa;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.LearnerData.Services
{
    public interface IGetProviderRelationshipService
    {
        Task<List<EmployerDetails>> GetEmployerDetails(GetProviderAccountLegalEntitiesResponse providerDetails);

        Task<GetProviderAccountLegalEntitiesResponse> GetAllProviderRelationShipDetails(int ukprn);
    }

    public class GetProviderRelationshipService(
    IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipApiClient,
        IAccountsApiClient<AccountsConfiguration> accountsApiClient,
        IFjaaApiClient<FjaaApiConfiguration> fjaaApiClient) : IGetProviderRelationshipService
    {
        public async Task<List<EmployerDetails>> GetEmployerDetails(GetProviderAccountLegalEntitiesResponse providerDetails)
        {
            ConcurrentBag<EmployerDetails> employerDetails = new ConcurrentBag<EmployerDetails>();

            await Parallel.ForEachAsync(providerDetails.AccountProviderLegalEntities,
                 new ParallelOptions { MaxDegreeOfParallelism = 5 },
                 async (legalEntity1, cancellationToken) =>
                 {
                     var accountDetailsTask = GetEmployerAccountDetails(legalEntity1.AccountId);
                     var agencyDetailsResult = await GetAgencyDetails(legalEntity1.AccountLegalEntityId);
                     var accountDetailsResult = await accountDetailsTask;

                     employerDetails.Add(CreateEmployerDetails(accountDetailsResult, agencyDetailsResult, legalEntity1.AccountLegalEntityPublicHashedId));
                 });

            return employerDetails.ToList();
        }

        private async Task<GetAccountByIdResponse> GetEmployerAccountDetails(long accountId)
        {
            var accountResponse = await accountsApiClient.Get<GetAccountByIdResponse>(
                new GetAccountByIdRequest(accountId));
            return accountResponse;
        }

        private async Task<GetAgencyResponse> GetAgencyDetails(long legalEntityId)
        {
            var agencyResponse = await fjaaApiClient.Get<GetAgencyResponse>(
               new GetAgencyQuery(legalEntityId));
            return agencyResponse;
        }

        private EmployerDetails CreateEmployerDetails(GetAccountByIdResponse accountDetails, GetAgencyResponse agencyDetails, string legalEntityHashedId)
        {
            return new EmployerDetails()
            {
                AgreementId = legalEntityHashedId,
                IsLevy = accountDetails is null ? false : accountDetails.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy,
                IsFlexiEmployer = agencyDetails?.IsGrantFunded ?? false
            };
        }

        public async Task<GetProviderAccountLegalEntitiesResponse> GetAllProviderRelationShipDetails(int ukprn)
        {
            var providerResponse =
                              await providerRelationshipApiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                                  new GetProviderAccountLegalEntitiesRequest(ukprn, [Operation.CreateCohort]));

            return providerResponse;
        }
    }
}