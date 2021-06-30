using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
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
            CmsContent apiResponse,
            CmsPageModel response,
            [Frozen] Mock<IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration>> apiClient,
            GetPreviewArticleByHubAndSlugQueryHandler handler)
        {
            apiClient.Setup(o =>
                    o.Get<CmsContent>(
                        It.Is<GetArticleEntriesRequest>(c =>
                            c.GetUrl.Contains($"fields.hubType={query.Hub.ToTitleCase()}&fields.slug={query.Slug}"))))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.PageModel.Should().BeEquivalentTo(response.Build(apiResponse));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_If_The_Article_Is_Not_Found_The_Landing_Page_Is_Requested(
            GetPreviewArticleByHubAndSlugQuery query,
            CmsContent apiResponse,
            CmsPageModel response,
            [Frozen] Mock<IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration>> apiClient,
            GetPreviewArticleByHubAndSlugQueryHandler handler)
        {
            apiClient.Setup(o =>
                    o.Get<CmsContent>(
                        It.Is<GetArticleEntriesRequest>(c =>
                            c.GetUrl.Contains($"fields.hubType={query.Hub.ToTitleCase()}&fields.slug={query.Slug}"))))
                .ReturnsAsync(new CmsContent
                {
                    Total = 0
                });
            apiClient.Setup(o =>
                    o.Get<CmsContent>(
                        It.Is<GetLandingPageRequest>(c =>
                            c.GetUrl.Contains($"fields.hubType={query.Hub.ToTitleCase()}&fields.slug={query.Slug}"))))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.PageModel.Should().BeEquivalentTo(response.Build(apiResponse));
        }
    }
}