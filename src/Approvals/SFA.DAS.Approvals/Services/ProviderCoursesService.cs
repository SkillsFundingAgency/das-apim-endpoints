﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Requests;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Interfaces;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;

namespace SFA.DAS.Approvals.Services
{
    public interface IProviderStandardsService
    {
        Task<ProviderStandardsData> GetStandardsData(long providerId);
    }

    public class ProviderStandardsService : IProviderStandardsService
    {
        private readonly ServiceParameters _serviceParameters;
        private readonly ITrainingProviderService _trainingProviderService;
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _providerCoursesApiClient;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;
        private readonly ICacheStorageService _cacheStorageService;

        public const string AllStandardsCacheKey = "ProviderCoursesService.GetAllStandardsResponse";
        public const string ProviderDetailsCacheKey = "ProviderCoursesService.TrainingProviderResponse";
        public const int CacheExpiryHours = 12;

        public ProviderStandardsService(ServiceParameters serviceParameters,
            ITrainingProviderService trainingProviderService,
            IProviderCoursesApiClient<ProviderCoursesApiConfiguration> providerCoursesApiClient,
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
            ICacheStorageService cacheStorageService)
        {
            _serviceParameters = serviceParameters;
            _trainingProviderService = trainingProviderService;
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
            _cacheStorageService = cacheStorageService;
            _providerCoursesApiClient = providerCoursesApiClient;
        }

        public async Task<ProviderStandardsData> GetStandardsData(long providerId)
        {
            var providerDetails = await GetTrainingProviderDetails(providerId);

            if (_serviceParameters.CallingParty == Party.Employer || !providerDetails.IsMainProvider)
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

        private async Task<TrainingProviderResponse> GetTrainingProviderDetails(long providerId)
        {
            var cacheKey = $"{ProviderDetailsCacheKey}-{providerId}";

            var cacheResult = await _cacheStorageService.RetrieveFromCache<TrainingProviderResponse>(cacheKey);

            if (cacheResult != null)
            {
                return cacheResult;
            }

            var result = await _trainingProviderService.GetTrainingProviderDetails(providerId);
            await _cacheStorageService.SaveToCache(cacheKey, result, CacheExpiryHours);
            return result;
        }

        private async Task<IEnumerable<Standard>> GetAllStandards()
        {
            var cacheResult =
                await _cacheStorageService.RetrieveFromCache<GetAllStandardsResponse>(AllStandardsCacheKey);

            if (cacheResult != null)
            {
                return cacheResult.TrainingProgrammes.Select(x => new Standard(x.CourseCode, x.Name));
            }

            var result = await _commitmentsV2ApiClient.Get<GetAllStandardsResponse>(new GetAllStandardsRequest());
            await _cacheStorageService.SaveToCache(AllStandardsCacheKey, result, CacheExpiryHours);
            return result.TrainingProgrammes.Select(x => new Standard(x.CourseCode, x.Name));
        }

        private async Task<IEnumerable<Standard>> GetStandardsForProvider(long providerId)
        {
            var providerStandards =
                await _providerCoursesApiClient.Get<IEnumerable<GetProviderStandardsResponse>>(
                    new GetProviderStandardsRequest(providerId));

            return providerStandards.Select(
                x => new Standard(x.LarsCode.ToString(), x.CourseNameWithLevel));
        }

        public async Task<ProviderStandardResults> GetProviderCourses(long providerId)
        {
            var providerDetails = await _trainingProviderService.GetTrainingProviderDetails(providerId);
            ProviderStandardResults providerStandardResults = new ProviderStandardResults { IsMainProvider = providerDetails.IsMainProvider };

            if (providerDetails.IsMainProvider == false) return providerStandardResults;

            var standards =
                    await _providerCoursesApiClient.Get<IEnumerable<GetProviderStandardsResponse>>(
                        new GetProviderStandardsRequest(providerId));

            if (standards == null) return providerStandardResults;
            providerStandardResults.ProviderStandards = standards.Select(x => new Standard(x.LarsCode.ToString(), x.CourseNameWithLevel)).ToList();

            return providerStandardResults;
        }
    }
}
