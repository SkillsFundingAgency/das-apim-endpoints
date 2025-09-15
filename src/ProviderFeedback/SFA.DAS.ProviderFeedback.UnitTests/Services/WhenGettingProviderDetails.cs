using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderFeedback.UnitTests.Services
{
    public class WhenGettingProviderDetails
    {
        [Test, MoqAutoData]
        public async Task Then_The_ProviderDetails_Are_Returned_From_Cache_If_Exists(
            long providerId,
            TrainingProviderResponse cachedResponse,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            ProviderService providerService)
        {
            var cacheKey = $"{ProviderService.ProviderDetailsCacheKey}-{providerId}";
            cacheStorageService
                .Setup(x => x.RetrieveFromCache<TrainingProviderResponse>(cacheKey))
                .ReturnsAsync(cachedResponse);

            var actual = await providerService.GetTrainingProviderDetails(providerId);

            actual.Should().BeEquivalentTo(cachedResponse);
            cacheStorageService.Verify(x => x.RetrieveFromCache<TrainingProviderResponse>(cacheKey), Times.Once);
            trainingProviderService.Verify(x => x.GetTrainingProviderDetails(It.IsAny<long>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_The_ProviderDetails_Are_Fetched_And_Cached_If_Not_In_Cache(
            long providerId,
            TrainingProviderResponse providerResponse,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            ProviderService providerService)
        {
            var cacheKey = $"{ProviderService.ProviderDetailsCacheKey}-{providerId}";
            cacheStorageService
                .Setup(x => x.RetrieveFromCache<TrainingProviderResponse>(cacheKey))
                .ReturnsAsync((TrainingProviderResponse)null);

            trainingProviderService
                .Setup(x => x.GetTrainingProviderDetails(providerId))
                .ReturnsAsync(providerResponse);

            var actual = await providerService.GetTrainingProviderDetails(providerId);

            actual.Should().BeEquivalentTo(providerResponse);
            cacheStorageService.Verify(x => x.RetrieveFromCache<TrainingProviderResponse>(cacheKey), Times.Once);
            trainingProviderService.Verify(x => x.GetTrainingProviderDetails(providerId), Times.Once);
            cacheStorageService.Verify(x => x.SaveToCache(cacheKey, providerResponse, ProviderService.CacheExpiryHours, null), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_TrainingProviderService_Returns_Data_It_Is_Saved_To_Cache(
            long providerId,
            TrainingProviderResponse providerResponse,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            ProviderService providerService)
        {
            var cacheKey = $"{ProviderService.ProviderDetailsCacheKey}-{providerId}";
            cacheStorageService
                .Setup(x => x.RetrieveFromCache<TrainingProviderResponse>(cacheKey))
                .ReturnsAsync((TrainingProviderResponse)null);

            trainingProviderService
                .Setup(x => x.GetTrainingProviderDetails(providerId))
                .ReturnsAsync(providerResponse);

            var actual = await providerService.GetTrainingProviderDetails(providerId);

            cacheStorageService.Verify(x => x.SaveToCache(cacheKey, providerResponse, ProviderService.CacheExpiryHours, null), Times.Once);
        }
    }
}
