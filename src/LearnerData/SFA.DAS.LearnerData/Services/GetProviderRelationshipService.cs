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

        Task<GetCoursesForProviderResponse> GetCoursesForProviderByUkprn(long ukprn);
    }

    public class GetProviderRelationshipService(
    IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipApiClient,
        IAccountsApiClient<AccountsConfiguration> accountsApiClient,
        IFjaaApiClient<FjaaApiConfiguration> fjaaApiClient,
        IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient) : IGetProviderRelationshipService
    {
        public GetAgenciesResponse GetAgencies { get; set; }

        public async Task<List<EmployerDetails>> GetEmployerDetails(GetProviderAccountLegalEntitiesResponse providerDetails)
        {
            ConcurrentBag<EmployerDetails> employerDetails = [];
            GetAgencies = await GetAgencyDetails();
            ConcurrentBag<GetAccountByIdResponse> accountdetails = [];

            var accountTasks = new ConcurrentDictionary<long, Task<GetAccountByIdResponse>>();

            var employerTasks = providerDetails.AccountProviderLegalEntities.
                Select(async legalEntity =>
                {
                    var accountTask = accountTasks.GetOrAdd(legalEntity.AccountId, GetEmployerAccountDetails);
                    var accountDetails = await accountTask;
                    var isFunded = accountDetails?.LegalEntities is not null && GetIsFunded(accountDetails.LegalEntities);

                    return CreateEmployerDetails(accountDetails, isFunded, legalEntity.AccountLegalEntityPublicHashedId);
                });

            var results = await Task.WhenAll(employerTasks);

            return results.ToList();
        }

        private async Task<GetAccountByIdResponse> GetEmployerAccountDetails(long accountId)
        {
            var accountResponse = await accountsApiClient.Get<GetAccountByIdResponse>(
                new GetAccountByIdRequest(accountId));

            return accountResponse;
        }

        private async Task<GetAgenciesResponse> GetAgencyDetails()
        {
            var agencyResponse = await fjaaApiClient.Get<GetAgenciesResponse>(
               new GetAgenciesQuery());

            return agencyResponse;
        }

        private EmployerDetails CreateEmployerDetails(GetAccountByIdResponse? accountDetails, bool isFunded, string legalEntityHashedId)
        {
            return new EmployerDetails()
            {
                AgreementId = legalEntityHashedId,
                IsLevy = accountDetails is not null && accountDetails.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy,
                IsFlexiEmployer = isFunded
            };
        }

        public async Task<GetProviderAccountLegalEntitiesResponse> GetAllProviderRelationShipDetails(int ukprn)
        {
            var providerResponse =
                              await providerRelationshipApiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                                  new GetProviderAccountLegalEntitiesRequest(ukprn, [Operation.CreateCohort]));

            return providerResponse;
        }

        public async Task<GetCoursesForProviderResponse> GetCoursesForProviderByUkprn(long ukprn)
        {
            var coursesApiResponse =  await roatpCourseManagementApiClient.GetWithResponseCode<GetCoursesForProviderResponse>(new GetCoursesForProviderRequest(ukprn));
            return coursesApiResponse.Body;
        }

        public bool GetIsFunded(ResourceList legalEntities)
        {
            return GetAgencies.Agencies.Any(t => legalEntities.Any(k => long.Parse(k.Id) == t.LegalEntityId));
        }
    }
}