using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.Services;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.Services
{
    public class WhenGettingAvailableProducts
    {
        [Test, MoqAutoData]
        public async Task Then_The_Endpoint_Is_Called_And_Cached_If_Not_In_Cache(
            string accountType,
            GetAvailableApiProductsResponse apiResponse,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> apimDeveloperApiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            ApimApiService apimApiService)
        {
            //Arrange
            var expectedGet = new GetAvailableApiProductsRequest(accountType);
            cacheStorageService
                .Setup(x => x.RetrieveFromCache<GetAvailableApiProductsResponse>(
                    $"{accountType}-{nameof(GetAvailableApiProductsResponse)}"))
                .ReturnsAsync((GetAvailableApiProductsResponse)null);
            apimDeveloperApiClient
                .Setup(x => x.Get<GetAvailableApiProductsResponse>(
                    It.Is<GetAvailableApiProductsRequest>(c => c.GetUrl.Equals(expectedGet.GetUrl))))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await apimApiService.GetAvailableProducts(accountType);
            
            //Assert
            actual.Should().BeEquivalentTo(apiResponse);
            cacheStorageService.Verify(x=>x.SaveToCache($"{accountType}-{nameof(GetAvailableApiProductsResponse)}", apiResponse, 1));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Retrieved_From_Cache_If_Available_And_Api_Not_Called(
            string accountType,
            GetAvailableApiProductsResponse cacheResponse,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> apimDeveloperApiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            ApimApiService apimApiService)
        {
            //Arrange
            cacheStorageService
                .Setup(x => x.RetrieveFromCache<GetAvailableApiProductsResponse>(
                    $"{accountType}-{nameof(GetAvailableApiProductsResponse)}"))
                .ReturnsAsync(cacheResponse);
            
            //Act
            var actual = await apimApiService.GetAvailableProducts(accountType);
            
            //Assert
            actual.Should().BeEquivalentTo(cacheResponse);
            apimDeveloperApiClient
                .Verify(x => x.Get<GetAvailableApiProductsResponse>(
                    It.IsAny<GetAvailableApiProductsRequest>()), Times.Never);
        }
    }
}