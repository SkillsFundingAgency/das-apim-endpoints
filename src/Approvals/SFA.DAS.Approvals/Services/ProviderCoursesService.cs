using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Requests;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Responses;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.Roatp;

namespace SFA.DAS.Approvals.Services;

public interface IProviderStandardsService
{
    Task<ProviderStandardsData> GetStandardsData(long providerId);
}

public class ProviderStandardsService(
    ServiceParameters serviceParameters,
    ITrainingProviderService trainingProviderService,
    IProviderCoursesApiClient<ProviderCoursesApiConfiguration> providerCoursesApiClient,
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
    ICacheStorageService cacheStorageService,
    ILogger<ProviderStandardsService> logger)
    : IProviderStandardsService
{
    public const string AllStandardsCacheKey = "ProviderCoursesService.GetAllStandardsResponse";
    public const string ProviderDetailsCacheKey = "ProviderCoursesService.TrainingProviderResponse";
    public const int CacheExpiryHours = 12;

    public async Task<ProviderStandardsData> GetStandardsData(long providerId)
    {
        var providerDetails = await GetTrainingProviderDetails(providerId);

        if (serviceParameters.CallingParty == Party.Employer || !providerDetails.IsMainProvider)
        {
            var standards = await GetAllStandards();
            var standardsList = standards.ToList();
            logger.LogInformation(
                "Standards fetched for UKPRN {Ukprn}: {Count} standards from Get all standards",
                providerId,
                standardsList.Count);
            return new ProviderStandardsData
            {
                IsMainProvider = providerDetails.IsMainProvider,
                Standards = standardsList
            };
        }

        var providerStandards = await GetStandardsForProvider(providerId);
        var providerStandardsList = providerStandards.ToList();
        logger.LogInformation(
            "Standards fetched for UKPRN {Ukprn}: {Count} standards from Get standards for provider",
            providerId,
            providerStandardsList.Count);
        return new ProviderStandardsData
        {
            IsMainProvider = providerDetails.IsMainProvider,
            Standards = providerStandardsList
        };
    }

    private async Task<ProviderDetailsModel> GetTrainingProviderDetails(long providerId)
    {
        var cacheKey = $"{ProviderDetailsCacheKey}-{providerId}";

        var cacheResult = await cacheStorageService.RetrieveFromCache<ProviderDetailsModel>(cacheKey);

        if (cacheResult != null)
        {
            return cacheResult;
        }

        var result = await trainingProviderService.GetProviderDetails((int)providerId);
        await cacheStorageService.SaveToCache(cacheKey, result, CacheExpiryHours);
        return result;
    }

    private async Task<IEnumerable<Standard>> GetAllStandards()
    {
        var cacheResult =
            await cacheStorageService.RetrieveFromCache<GetAllStandardsResponse>(AllStandardsCacheKey);

        if (cacheResult != null)
        {
            return cacheResult.TrainingProgrammes.Select(x => new Standard(x.CourseCode, x.Name, x.Level)).OrderBy(x => x.Name);
        }
        
        var result = await commitmentsV2ApiClient.Get<GetAllStandardsResponse>(new GetAllStandardsRequest());
        await cacheStorageService.SaveToCache(AllStandardsCacheKey, result, CacheExpiryHours);
        return result.TrainingProgrammes.Select(x => new Standard(x.CourseCode, x.Name, x.Level)).OrderBy(x => x.Name);
    }

    private async Task<IEnumerable<Standard>> GetStandardsForProvider(long providerId)
    {
        try
        {
            var providerStandards =
                await providerCoursesApiClient.Get<IEnumerable<GetProviderStandardsResponse>>(
                    new GetProviderStandardsRequest(providerId));

            if (providerStandards?.Any() != true)
            {
                logger.LogWarning($"No Standards Declared For Provider {providerId}");
                return [];
            }

            return providerStandards.Select(
                x => new Standard(x.LarsCode.ToString(), x.CourseNameWithLevel, x.Level)).OrderBy(x => x.Name).ToList();
        }
        catch (Exception e)
        {
            logger.LogError($"No Standards Declared For Provider {providerId}", e);
            return [];
        }
    }
}