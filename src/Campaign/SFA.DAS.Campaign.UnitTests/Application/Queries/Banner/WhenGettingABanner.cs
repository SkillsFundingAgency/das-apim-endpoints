using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Application.Queries.Banner;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.Banner
{
    public class WhenGettingABanner
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Api_Is_Called_And_The_Banner_Is_Returned(
            GetBannerQuery query,
            CmsContent apiResponse,
            BannerPageModel response,
            [Frozen] Mock<IContentService> contentService,
            [Frozen] Mock<IReliableCacheStorageService> service,
            GetBannerQueryHandler handler)
        {
            contentService.Setup(x => x.HasContent(It.IsAny<ApiResponse<CmsContent>>())).Returns(true);
            service.Setup(o =>
                    o.GetData<CmsContent>(
                        It.Is<GetBannerRequest>(c =>
                            c.GetUrl.Contains($"entries?content_type=banner")),
                        $"Banners", contentService.Object.HasContent))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.PageModel.Should().BeEquivalentTo(response.Build(apiResponse));
        }
    }
}
