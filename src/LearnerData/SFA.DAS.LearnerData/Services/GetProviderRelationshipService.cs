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
using System.Collections.Concurrent;

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
                     var accountTask = await GetEmployerAccountDetails(legalEntity1.AccountId);

                     var agencyTask = await GetAgencyDetais(legalEntity1.AccountLegalEntityId);

                     if (agencyTask is null) { return; }

                     employerDetails.Add(CreateEmployerDetails(accountTask, agencyTask, legalEntity1.AccountLegalEntityPublicHashedId));
                 });

            return employerDetails.ToList();
        }

        private async Task<GetAccountByIdResponse> GetEmployerAccountDetails(long accountId)
        {
            var accountResponse = await accountsApiClient.Get<GetAccountByIdResponse>(
                new GetAccountByIdRequest(accountId));
            return accountResponse;
        }

        private async Task<GetAgencyResponse> GetAgencyDetais(long legalEntityId)
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
                IsLevy = accountDetails.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy,
                IsFelxiEmployer = agencyDetails.IsGrantFunded
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