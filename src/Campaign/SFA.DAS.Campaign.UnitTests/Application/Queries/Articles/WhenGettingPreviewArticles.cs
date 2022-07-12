using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Application.Queries.Banner;
using SFA.DAS.Campaign.Application.Queries.Menu;
using SFA.DAS.Campaign.Application.Queries.PreviewArticles;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.Articles
{
    public class WhenGettingPreviewArticles
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Valid_Request_Parameters_And_The_Article_Is_Returned(
            GetPreviewArticleByHubAndSlugQuery query,
            GetMenuQueryResult menuResult,
            GetBannerQueryResult bannerResult,
            BannerPageModel bannerContent,
            MenuPageModel.MenuPageContent menuContent,
            CmsContent apiResponse,
            CmsPageModel response,
            [Frozen] Mock<IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration>> apiClient,
            [Frozen] Mock<IMediator> mediator,
            GetPreviewArticleByHubAndSlugQueryHandler handler)
        {
            apiClient.Setup(o =>
                    o.Get<CmsContent>(
                        It.Is<GetArticleEntriesRequest>(c =>
                            c.GetUrl.Contains($"fields.hubType={query.Hub.ToTitleCase()}&fields.slug={query.Slug}"))))
                .ReturnsAsync(apiResponse);

            mediator.SetupMenu(menuResult, menuContent);
            mediator.SetupBanners(bannerResult, bannerContent);
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.PageModel.Should().BeEquivalentTo(response.Build(apiResponse, menuContent, bannerContent));
        }
    }
}