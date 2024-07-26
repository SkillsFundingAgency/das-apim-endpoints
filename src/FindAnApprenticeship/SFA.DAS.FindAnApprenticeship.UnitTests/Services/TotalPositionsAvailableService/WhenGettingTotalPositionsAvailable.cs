using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Services.TotalPositionsAvailableService
{
    [TestFixture]
    public class WhenGettingTotalPositionsAvailable
    {
        [Test, MoqAutoData]
        public async Task Then_The_Count_Is_Retrieved_From_The_Recruit_Api(
            long totalPositionsAvailable,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            FindAnApprenticeship.Services.TotalPositionsAvailableService service
            )
        {
            cacheStorageService.Setup(x => x.RetrieveFromCache<long?>(It.IsAny<string>()))
                .ReturnsAsync(() =>null);

            recruitApiClient.Setup(x => x.Get<long>(It.IsAny<GetTotalPositionsAvailableRequest>()))
                .ReturnsAsync(totalPositionsAvailable);

            var result = await service.GetTotalPositionsAvailable();

            result.Should().Be(totalPositionsAvailable);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Count_Is_Cached(
            long totalPositionsAvailable,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            FindAnApprenticeship.Services.TotalPositionsAvailableService service
        )
        {
            cacheStorageService.Setup(x => x.RetrieveFromCache<long?>(It.IsAny<string>()))
            .ReturnsAsync(() => null);

            recruitApiClient.Setup(x => x.Get<long>(It.IsAny<GetTotalPositionsAvailableRequest>()))
                .ReturnsAsync(totalPositionsAvailable);

            var result = await service.GetTotalPositionsAvailable();

            cacheStorageService.Verify(x =>
                x.SaveToCache(It.Is<string>(key => key == nameof(GetTotalPositionsAvailableRequest)),
                    It.Is<long>(item => item == totalPositionsAvailable),
                    It.Is<TimeSpan>(expiry => expiry == TimeSpan.FromHours(1))));
        }


        [Test, MoqAutoData]
        public async Task Then_The_Count_Is_Retrieved_From_The_Cache(
            long totalPositionsAvailable,
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
}
