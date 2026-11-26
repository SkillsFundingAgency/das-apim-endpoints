using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Extensions;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.VacanciesController;

public class WhenGettingVacancyStatistics
{
    [Test, MoqAutoData]
    public async Task Then_The_Data_Returned_From_The_Cache_If_Stored(
        GetSearchIndexStatisticsResponse statsResponse,
        Mock<ICacheStorageService> cacheStorageService,
        Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
        [Greedy] Api.Controllers.VacanciesController sut)
    {
        // arrange
        cacheStorageService
            .Setup(x => x.RetrieveFromCache<GetSearchIndexStatisticsResponse>(nameof(GetSearchIndexStatisticsResponse)))
            .ReturnsAsync(statsResponse);

        // act
        var response = await sut.GetSearchIndexStatistics(cacheStorageService.Object, apiClient.Object) as OkObjectResult;

        // assert
        response.Should().NotBeNull();
        response!.Value.Should().Be(statsResponse);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Data_Returned_And_Added_To_The_Cache(
        GetSearchIndexStatisticsResponse statsResponse,
        Mock<ICacheStorageService> cacheStorageService,
        Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
        [Greedy] Api.Controllers.VacanciesController sut)
    {
        // arrange
        cacheStorageService
            .Setup(x => x.RetrieveFromCache<GetSearchIndexStatisticsResponse>(nameof(GetSearchIndexStatisticsResponse)))
            .ReturnsAsync(() => null);
        
        apiClient
            .Setup(x => x.Get<GetSearchIndexStatisticsResponse>(It.IsAny<GetSearchIndexStatisticsRequest>()))
            .ReturnsAsync(statsResponse);
        
        TimeSpan? capturedTimeToLive = null;
        cacheStorageService
            .Setup(x => x.SaveToCache(nameof(GetSearchIndexStatisticsResponse), It.IsAny<GetSearchIndexStatisticsResponse>(), It.IsAny<TimeSpan>(), null))
            .Callback<string, GetSearchIndexStatisticsResponse, TimeSpan, string?>((_, _, timeToLive, _) => { capturedTimeToLive = timeToLive; })
            .Returns(() => Task.CompletedTask);

        // act
        var response = await sut.GetSearchIndexStatistics(cacheStorageService.Object, apiClient.Object) as OkObjectResult;

        // assert
        response.Should().NotBeNull();
        response!.Value.Should().Be(statsResponse);
        capturedTimeToLive.Should().BeCloseTo(DateTime.UtcNow.TimeUntilMinutesPastHour(5), TimeSpan.FromSeconds(5));
    }
}