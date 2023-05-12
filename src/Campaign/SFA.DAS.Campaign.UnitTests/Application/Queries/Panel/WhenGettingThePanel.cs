using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.Campaign.Application.Queries.Panel;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Campaign.ExternalApi.Requests;
using FluentAssertions;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.Panel
{
    public class WhenGettingThePanel
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Api_Is_Called_And_The_Panel_Is_Returned(
            GetPanelQuery query,
            CmsContent apiResponse,
            PanelModel response,
            [Frozen] Mock<IContentService> contentService,
            [Frozen] Mock<IReliableCacheStorageService> service,
            GetPanelQueryHandler handler)
        {
            contentService.Setup(x => x.HasContent(It.IsAny<ApiResponse<CmsContent>>())).Returns(true);
            service.Setup(o =>
                    o.GetData<CmsContent>(
                        It.Is<GetPanelRequest>(c =>
                            c.GetUrl.Contains($"entries?content_type=panel&fields.id={query.Id}")),
                        $"{query.Id}", contentService.Object.HasContent))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Panel.Should().BeEquivalentTo(response.Build(apiResponse));
        }
    }

}
