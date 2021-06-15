using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Application.Services;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Campaign.UnitTests.Builders;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Services
{
    public class ContentfulServiceTests
    {
        const string ARTICLE_TITLE = "A Title";
        private const string ENTRY_ID = "entryid";
        private const string HUB_TYPE = "home";
        private const string SLUG = "slug-name";

        [Test]
        public void WhenGivenNullContentfulClientInConstructorThenThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new ContentfulService(null);
            });

            exception.ParamName.Should().Be("client");
        }

        [Test, RecursiveMoqAutoData]
        public async Task WhenGetEntryForAsyncIsGivenAValidEntryThenReturnsTheContentType([Frozen] Mock<IContentfulClient> client, [Greedy] ContentfulService service)
        {
            client.Setup(o => o.GetEntry(It.IsAny<string>(), It.IsAny<QueryBuilder<Article>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ArticleBuilder.New().SetTitle(ARTICLE_TITLE).Build());

            var actual = await service.GetEntryForAsync<Article>(ENTRY_ID, new QueryBuilder<Article>());

            actual.Title.Should().Be(ARTICLE_TITLE);
        }

        [Test, RecursiveMoqAutoData]
        public async Task WhenGetArticleForAsyncIsGivenAValidIdThenReturnsTheArticle([Frozen] Mock<IContentfulClient> client, [Greedy] ContentfulService service)
        {
            SetupClientMockForEntriesMethodCall(client, 
                ArticleBuilder.New()
                    .SetTitle(ARTICLE_TITLE)
                    .SetId(ENTRY_ID)
                    .Build());

            var actual = await service.GetArticleForAsync(ENTRY_ID);

            actual.Title.Should().Be(ARTICLE_TITLE);
        }

        [Test, RecursiveMoqAutoData]
        public async Task WhenGetArticleForAsyncIsGivenAValidHubTypeAndSlugThenReturnsArticle([Frozen] Mock<IContentfulClient> client, [Greedy] ContentfulService service)
        {

            SetupClientMockForEntriesMethodCall(client,
                ArticleBuilder.New()
                    .SetTitle(ARTICLE_TITLE)
                    .SetId(ENTRY_ID)
                    .SetSlug(SLUG)
                    .SetHubType(HUB_TYPE)
                    .Build());

            var actual = await service.GetArticleForAsync(HUB_TYPE, SLUG);

            actual.Title.Should().Be(ARTICLE_TITLE);
        }

        private static void SetupClientMockForEntriesMethodCall(Mock<IContentfulClient> client, Article articleToReturn)
        {
            client.Setup(o => o.GetEntries(It.IsAny<QueryBuilder<Article>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ContentfulCollection<Article>()
                {
                    Items = new List<Article>
                    {
                        articleToReturn
                    }
                });
        }
    }
}
