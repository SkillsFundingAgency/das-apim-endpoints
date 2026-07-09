using FluentAssertions;
using Moq;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Rofjaa;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Rofjaa;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class FjaaAgenciesServiceTests
{
    [Test, MoqAutoData]
    public async Task GetAgencies_Returns_From_Cache_When_Present(
        GetAgenciesResponse cachedResponse,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
        [Greedy] FjaaAgenciesService sut)
    {
        // Arrange
        cacheStorageService
            .Setup(x => x.RetrieveFromCache<GetAgenciesResponse>("Fjaa.GetAgenciesResponse"))
            .ReturnsAsync(cachedResponse);

        // Act
        var result = await sut.GetAgencies(CancellationToken.None);

        // Assert
        result.Should().BeSameAs(cachedResponse);
        fjaaApiClient.Verify(x => x.Get<GetAgenciesResponse>(It.IsAny<GetAgenciesQuery>()), Times.Never);
        cacheStorageService.Verify(x => x.SaveToCache(It.IsAny<string>(), It.IsAny<GetAgenciesResponse>(), It.IsAny<int>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task GetAgencies_Fetches_And_Caches_When_Missing(
        GetAgenciesResponse apiResponse,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
        [Greedy] FjaaAgenciesService sut)
    {
        // Arrange
        cacheStorageService
            .Setup(x => x.RetrieveFromCache<GetAgenciesResponse>("Fjaa.GetAgenciesResponse"))
            .ReturnsAsync((GetAgenciesResponse?)null);

        fjaaApiClient
            .Setup(x => x.Get<GetAgenciesResponse>(It.Is<GetAgenciesQuery>(q => q.GetUrl == "agencies")))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await sut.GetAgencies(CancellationToken.None);

        // Assert
        result.Should().BeSameAs(apiResponse);
        cacheStorageService.Verify(x => x.SaveToCache("Fjaa.GetAgenciesResponse", apiResponse, 2), Times.Once);
    }
}
