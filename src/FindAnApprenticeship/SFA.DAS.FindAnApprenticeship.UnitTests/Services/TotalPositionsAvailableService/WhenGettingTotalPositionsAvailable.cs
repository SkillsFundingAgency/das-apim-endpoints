using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Services.TotalPositionsAvailableService;

[TestFixture]
public class WhenGettingTotalPositionsAvailable
{
    [Test, MoqAutoData]
    public async Task Then_The_Count_Is_Retrieved_From_The_Recruit_Api(
        GetTotalPositionsAvailableResponse response,
        GetApprenticeshipCountResponse apprenticeshipCountResponse,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitApiClient,
        FindAnApprenticeship.Services.TotalPositionsAvailableService service)
    {
        cacheStorageService.Setup(x => x.RetrieveFromCache<long?>(It.IsAny<string>()))
            .ReturnsAsync(() =>null);

        recruitApiClient.Setup(x => x.Get<GetTotalPositionsAvailableResponse>(It.IsAny<GetTotalPositionsAvailableRequest>()))
            .ReturnsAsync(response);

        var apprenticeCountRequest = new GetApprenticeshipCountRequest(
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            false,
            new List<VacancyDataSource>
            {
                VacancyDataSource.Nhs,
                VacancyDataSource.Csj,
            },
            null,
            null);

        findApprenticeshipApiClient.Setup(client =>
                client.Get<GetApprenticeshipCountResponse>(
                    It.Is<GetApprenticeshipCountRequest>(r => r.GetUrl == apprenticeCountRequest.GetUrl)))
            .ReturnsAsync(apprenticeshipCountResponse);

        var result = await service.GetTotalPositionsAvailable();

        result.Should().Be(response.TotalPositionsAvailable + apprenticeshipCountResponse.TotalVacancies);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Count_Is_Cached(
        GetTotalPositionsAvailableResponse response,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitApiClient,
        FindAnApprenticeship.Services.TotalPositionsAvailableService service
    )
    {
        cacheStorageService.Setup(x => x.RetrieveFromCache<long?>(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        recruitApiClient.Setup(x => x.Get<GetTotalPositionsAvailableResponse>(It.IsAny<GetTotalPositionsAvailableRequest>()))
            .ReturnsAsync(response);

        var result = await service.GetTotalPositionsAvailable();

        cacheStorageService.Verify(x =>
            x.SaveToCache(It.Is<string>(key => key == nameof(GetTotalPositionsAvailableRequest)),
                It.Is<long>(item => item == response.TotalPositionsAvailable),
                It.Is<TimeSpan>(expiry => expiry == TimeSpan.FromHours(1)), null));
    }

    [Test, MoqAutoData]
    public async Task Then_The_Count_Is_Retrieved_From_The_Cache(
        long cachedTotalPositionsAvailable,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        FindAnApprenticeship.Services.TotalPositionsAvailableService service
    )
    {
        cacheStorageService.Setup(x => x.RetrieveFromCache<long?>(It.IsAny<string>()))
            .ReturnsAsync(() => cachedTotalPositionsAvailable);

        var result = await service.GetTotalPositionsAvailable();

        result.Should().Be(cachedTotalPositionsAvailable);
    }
}