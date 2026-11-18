using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Campaign.Application.Queries.Homepage;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.Homepage
{
    public class WhenGettingTheHomepage
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Api_Is_Called_And_The_Homepage_Is_Returned(
            GetHomepageQuery query,
            CmsContent apiResponse,
            [Frozen] Mock<IContentService> contentService,
            [Frozen] Mock<IReliableCacheStorageService> service,
            GetHomepageQueryHandler handler)
        {
            contentService.Setup(x => x.HasContent(It.IsAny<ApiResponse<CmsContent>>())).Returns(true);
            service.Setup(o =>
                    o.GetData<CmsContent>(
                        It.Is<GetHomepageRequest>(c => c.GetUrl.Contains("entries?content_type=homepage")),
                        GetHomepageQueryHandler.CacheKey, contentService.Object.HasContent))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.PageModel.Should().BeEquivalentTo(new HomepageModel().Build(apiResponse));
        }
    }
}
