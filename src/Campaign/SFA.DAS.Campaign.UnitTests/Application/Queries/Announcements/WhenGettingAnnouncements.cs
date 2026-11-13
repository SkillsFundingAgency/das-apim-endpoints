using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Campaign.Application.Queries.Announcements;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.Announcements
{
    public class WhenGettingAnnouncements
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Api_Is_Called_And_The_Announcements_Are_Returned(
            GetAnnouncementsQuery query,
            CmsContent apiResponse,
            [Frozen] Mock<IContentService> contentService,
            [Frozen] Mock<IReliableCacheStorageService> service,
            GetAnnouncementsQueryHandler handler)
        {
            contentService.Setup(x => x.HasContent(It.IsAny<ApiResponse<CmsContent>>())).Returns(true);
            service.Setup(o =>
                    o.GetData<CmsContent>(
                        It.Is<GetAnnouncementsRequest>(c =>
                            c.GetUrl.Contains("entries?content_type=announcements") && c.GetUrl.Contains("order=-fields.date")),
                        GetAnnouncementsQueryHandler.CacheKey, contentService.Object.HasContent))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.PageModel.Should().BeEquivalentTo(new AnnouncementsPageModel().Build(apiResponse));
        }
    }
}
