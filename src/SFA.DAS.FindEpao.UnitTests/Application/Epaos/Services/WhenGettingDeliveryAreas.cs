using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindEpao.Application.Epaos.Services;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindEpao.UnitTests.Application.Epaos.Services
{
    public class WhenGettingDeliveryAreas
    {
        [Test, MoqAutoData]
        public async Task And_DeliveryAreas_Cached_Then_Gets_DeliveryAreas_From_Cache(
            List<GetDeliveryAreaListItem> areasFromCache,
            [Frozen] Mock<ICacheStorageService> mockCacheService,
            CachedDeliveryAreasService service)
        {
            mockCacheService
                .Setup(service => service.RetrieveFromCache<IReadOnlyList<GetDeliveryAreaListItem>>(nameof(GetDeliveryAreasRequest)))
                .ReturnsAsync(areasFromCache);

            var result = await service.GetDeliveryAreas();

            result.Should().BeEquivalentTo(areasFromCache);
        }

        [Test, MoqAutoData]
        public async Task And_DeliveryAreas_Not_Cached_Then_Gets_From_Api_And_Stores_In_Cache(
            List<GetDeliveryAreaListItem> areasFromApi,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            [Frozen] Mock<ICacheStorageService> mockCacheService,
            CachedDeliveryAreasService service)
        {
            var expectedExpirationInHours = 1;
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetDeliveryAreaListItem>(
                    It.IsAny<GetDeliveryAreasRequest>()))
                .ReturnsAsync(areasFromApi);
            mockCacheService
                .Setup(service => service.RetrieveFromCache<IReadOnlyList<GetDeliveryAreaListItem>>(nameof(GetDeliveryAreasRequest)))
                .ReturnsAsync((IReadOnlyList<GetDeliveryAreaListItem>)null);

            var result = await service.GetDeliveryAreas();

            result.Should().BeEquivalentTo(areasFromApi);
            mockCacheService.Verify(service =>
                service.SaveToCache<IReadOnlyList<GetDeliveryAreaListItem>>(
                    nameof(GetDeliveryAreasRequest),
                    areasFromApi,
                    expectedExpirationInHours));
        }
    }
}
