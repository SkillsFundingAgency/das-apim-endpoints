using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Requests;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.Roatp;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;

namespace SFA.DAS.Approvals.Services;

public interface IProviderCoursesOrStandardsService
{
    Task<ProviderStandardsData> GetCoursesData(long providerId);
}

public interface IProviderStandardsService
{
    Task<ProviderStandardsData> GetCoursesData(long providerId);
}

public class ProviderStandardsService(
    ServiceParameters serviceParameters,
    ITrainingProviderService trainingProviderService,
    IProviderCoursesApiClient<ProviderCoursesApiConfiguration> providerCoursesApiClient,
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
    ICacheStorageService cacheStorageService,
    ILogger<ProviderStandardsService> logger)
    : IProviderCoursesOrStandardsService, IProviderStandardsService
{
    public const string AllStandardsCacheKey = "ProviderStandardsService.GetAllStandardsResponse";
    public const string ProviderDetailsCacheKey = "ProviderStandardsService.TrainingProviderResponse";
    public const int CacheExpiryHours = 12;

    public async Task<ProviderStandardsData> GetCoursesData(long providerId)
    {
        var providerDetails = await GetTrainingProviderDetails(providerId);

        if (serviceParameters.CallingParty == Party.Employer || !providerDetails.IsMainProvider)
        {
            return new ProviderStandardsData
            {
                IsMainProvider = providerDetails.IsMainProvider,
                Standards = await GetAllStandards()
            };
        }

        return new ProviderStandardsData
        {
            IsMainProvider = providerDetails.IsMainProvider,
            Standards = await GetStandardsForProvider(providerId)
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
        
        var result = await commitmentsV2ApiClient.Get<GetAllStandardsResponse>(new InnerApi.CommitmentsV2Api.Requests.Courses.GetAllStandardsRequest());
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

public class ProviderCoursesService(
    ServiceParameters serviceParameters,
    ITrainingProviderService trainingProviderService,
    IProviderCoursesApiClient<ProviderCoursesApiConfiguration> providerCoursesApiClient,
    ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
    ICacheStorageService cacheStorageService,
    ILogger<ProviderStandardsService> logger)
    : IProviderCoursesOrStandardsService
{
    public const string AllCoursesCacheKey = "ProviderCoursesService.GetAllCoursesResponse";
    public const string ProviderDetailsCacheKey = "ProviderCoursesService.TrainingProviderResponse";
    public const int CacheExpiryHours = 12;

    public async Task<ProviderStandardsData> GetCoursesData(long providerId)
    {
        var providerDetails = await GetTrainingProviderDetails(providerId);

        if (serviceParameters.CallingParty == Party.Employer || !providerDetails.IsMainProvider)
        {
            return new ProviderStandardsData
            {
                IsMainProvider = providerDetails.IsMainProvider,
                Standards = await GetAllCourses()
            };
        }

        return new ProviderStandardsData
        {
            IsMainProvider = providerDetails.IsMainProvider,
            Standards = await GetCoursesForProvider(providerId)
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

    private async Task<IEnumerable<Standard>> GetAllCourses()
    {
        var cacheResult =
            await cacheStorageService.RetrieveFromCache<GetCoursesListResponse>(AllCoursesCacheKey);

        if (cacheResult != null)
        {
            return cacheResult.Courses.Select(x => new Standard(x.LarsCode, x.Title, x.Level)).OrderBy(x => x.Name);
        }

        var result = await coursesApiClient.Get<GetCoursesListResponse>(new GetCoursesExportRequest());
        await cacheStorageService.SaveToCache(AllCoursesCacheKey, result, CacheExpiryHours);
        return result.Courses.Select(x => new Standard(x.LarsCode, x.Title, x.Level)).OrderBy(x => x.Name);
    }

    private async Task<IEnumerable<Standard>> GetCoursesForProvider(long providerId)
    {
        try
        {
            var providerCourseResponse =
                await providerCoursesApiClient.Get<GetCoursesForProviderResponse>(
                    new GetCoursesForProviderRequest(providerId));

            var allCourses = providerCourseResponse.CourseTypes
                .SelectMany(ct => ct.Courses)
                .ToList();

            if (allCourses.Any() != true)
            {
                logger.LogWarning($"No Courses declared For Provider {providerId}");
                return [];
            }
            return allCourses.Select(
                x => new Standard(x.LarsCode, null, null)).OrderBy(x => x.Name);
        }
        catch (Exception e)
        {
            logger.LogError($"No Courses Declared For Provider {providerId}, due to exception", e);
            return [];
        }
    }
}