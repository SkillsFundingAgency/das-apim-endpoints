using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Campaign.Application.Queries.Redirects;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.Redirects
{
    public class WhenGettingTheRedirects
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Api_Is_Called_And_The_Redirects_Are_Returned(
            GetRedirectsQuery query,
            CmsContent apiResponse,
            [Frozen] Mock<IContentService> contentService,
            [Frozen] Mock<IReliableCacheStorageService> service,
            GetRedirectsQueryHandler handler)
        {
            contentService.Setup(x => x.HasContent(It.IsAny<ApiResponse<CmsContent>>())).Returns(true);
            service.Setup(o =>
                    o.GetData<CmsContent>(
                        It.Is<GetRedirectsRequest>(c => c.GetUrl.Equals($"entries?content_type=redirect&limit={GetRedirectsRequest.MaxRedirects}")),
                        GetRedirectsQueryHandler.CacheKey, contentService.Object.HasContent))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Redirects.Should().BeEquivalentTo(RedirectModel.BuildFrom(apiResponse));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_No_Redirects_Are_Returned_When_There_Is_No_Content(
            GetRedirectsQuery query,
            [Frozen] Mock<IContentService> contentService,
            [Frozen] Mock<IReliableCacheStorageService> service,
            GetRedirectsQueryHandler handler)
        {
            contentService.Setup(x => x.HasContent(It.IsAny<ApiResponse<CmsContent>>())).Returns(false);
            service.Setup(o => o.GetData<CmsContent>(It.IsAny<GetRedirectsRequest>(), It.IsAny<string>(), It.IsAny<System.Func<ApiResponse<CmsContent>, bool>>()))
                .ReturnsAsync((CmsContent)null);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Redirects.Should().BeEmpty();
        }
    }
}
