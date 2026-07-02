using System.Collections.Concurrent;
using System.Threading;
using SFA.DAS.LearnerData.Application.GetProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Rofjaa;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

namespace SFA.DAS.LearnerData.Services;

public interface IGetProviderRelationshipService
{
    Task<List<EmployerDetails>> GetEmployerDetails(GetProviderAccountLegalEntitiesResponse providerDetails, CancellationToken cancellationToken = default);

    Task<GetProviderAccountLegalEntitiesResponse> GetAllProviderRelationShipDetails(int ukprn, CancellationToken cancellationToken = default);

    Task<GetCoursesForProviderResponse> GetCoursesForProviderByUkprn(long ukprn, CancellationToken cancellationToken = default);
}

public class GetProviderRelationshipService(
    IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipApiClient,
    IAccountsApiClient<AccountsConfiguration> accountsApiClient,
    IFjaaAgenciesService fjaaAgenciesService,
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient) : IGetProviderRelationshipService
{
    private const int AccountsQueryParallelism = 5;

    public async Task<List<EmployerDetails>> GetEmployerDetails(
        GetProviderAccountLegalEntitiesResponse providerDetails,
        CancellationToken cancellationToken = default)
    {
        if (providerDetails?.AccountProviderLegalEntities is null || providerDetails.AccountProviderLegalEntities.Count == 0)
        {
            return [];
        }

        var agencies = await fjaaAgenciesService.GetAgencies(cancellationToken);
        var accountIds = providerDetails.AccountProviderLegalEntities
            .Select(x => x.AccountId)
            .Distinct()
            .ToList();

        var accountsById = await GetEmployerAccountsByIds(accountIds, cancellationToken);

        return providerDetails.AccountProviderLegalEntities
            .Select(legalEntity =>
            {
                accountsById.TryGetValue(legalEntity.AccountId, out var accountDetails);
                var isFunded = accountDetails?.LegalEntities is not null && GetIsFunded(agencies, accountDetails.LegalEntities);
                return CreateEmployerDetails(accountDetails, isFunded, legalEntity.AccountLegalEntityPublicHashedId);
            })
            .ToList();
    }

    private async Task<IReadOnlyDictionary<long, GetAccountByIdResponse>> GetEmployerAccountsByIds(
        IReadOnlyList<long> accountIds,
        CancellationToken cancellationToken)
    {
        if (accountIds.Count == 0)
        {
            return new Dictionary<long, GetAccountByIdResponse>();
        }

        var distinctAccountIds = accountIds.Distinct().ToList();
        var batches = distinctAccountIds
            .Chunk(AccountQueryFieldNames.MaxAccountIdsPerRequest)
            .ToList();

        var accountsById = new ConcurrentDictionary<long, GetAccountByIdResponse>();
        using var semaphore = new SemaphoreSlim(AccountsQueryParallelism);

        var batchTasks = batches.Select(async batch =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var apiResponse = await accountsApiClient.PostWithResponseCode<AccountsQueryRequestBody, PostAccountsQueryResponse>(
                    new PostAccountsQueryRequest(batch));
                var response = apiResponse.Body;

                if (response?.Accounts is null)
                {
                    return;
                }

                foreach (var account in response.Accounts)
                {
                    accountsById[account.AccountId] = new GetAccountByIdResponse
                    {
                        AccountId = account.AccountId,
                        ApprenticeshipEmployerType = ParseApprenticeshipEmployerType(account.ApprenticeshipEmployerType),
                        LegalEntities = account.LegalEntities ?? []
                    };
                }
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(batchTasks);

        return accountsById;
    }

    private static ApprenticeshipEmployerType ParseApprenticeshipEmployerType(string apprenticeshipEmployerType)
    {
        return Enum.TryParse<ApprenticeshipEmployerType>(apprenticeshipEmployerType, true, out var result)
            ? result
            : default;
    }

    public async Task<GetProviderAccountLegalEntitiesResponse> GetAllProviderRelationShipDetails(int ukprn, CancellationToken cancellationToken = default)
    {
        return await providerRelationshipApiClient.Get<GetProviderAccountLegalEntitiesResponse>(
            new GetProviderAccountLegalEntitiesRequest(ukprn, [Operation.CreateCohort]));
    }

    public async Task<GetCoursesForProviderResponse> GetCoursesForProviderByUkprn(long ukprn, CancellationToken cancellationToken = default)
    {
        var coursesApiResponse = await roatpCourseManagementApiClient.GetWithResponseCode<GetCoursesForProviderResponse>(
            new GetCoursesForProviderRequest(ukprn));
        return coursesApiResponse.Body;
    }

    private static bool GetIsFunded(GetAgenciesResponse agencies, ResourceList legalEntities)
    {
        return agencies.Agencies.Any(agency => legalEntities.Any(le => long.Parse(le.Id) == agency.LegalEntityId));
    }

    private static EmployerDetails CreateEmployerDetails(GetAccountByIdResponse? accountDetails, bool isFunded, string legalEntityHashedId)
    {
        return new EmployerDetails
        {
            AgreementId = legalEntityHashedId,
            IsLevy = accountDetails is not null && accountDetails.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy,
            IsFlexiEmployer = isFunded
        };
    }
}
