using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Contentful;

namespace SFA.DAS.Campaign.UnitTests.Application.Contentful
{
    public class ContentfulServiceTests
    {
        private Mock<IContentfulClient> _contentfulClient;
        private ContentfulService _service;
        const string ARTICLE_TITLE = "A Title";
        private const string ENTRY_ID = "entryid";
        private const string HUB_TYPE = "home";
        private const string SLUG = "slug-name";


        [SetUp]
        public void Setup()
        {
            _contentfulClient = new Mock<IContentfulClient>();
            _service = new ContentfulService(_contentfulClient.Object);
        }

        [Test]
        public void Constructor_WhenGivenNullContentfulClient_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new ContentfulService(null);
            });

            exception.ParamName.Should().Be("client");
        }

        [Test]
        public async Task GetEntryForAsync_GivenAValidEntry_ReturnsTheContentType()
        {
            _contentfulClient.Setup(o => o.GetEntry<Article>(It.IsAny<string>(), It.IsAny<QueryBuilder<Article>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Article
                {
                    Title = ARTICLE_TITLE
                });

            var actual = await _service.GetEntryForAsync<Article>(ENTRY_ID, new QueryBuilder<Article>());

            actual.Title.Should().Be(ARTICLE_TITLE);
        }

        [Test]
        public async Task GetArticleForAsync_GivenAValidId_ReturnsTheArticle()
        {
            _contentfulClient.Setup(o => o.GetEntries<Article>(It.IsAny<QueryBuilder<Article>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ContentfulCollection<Article>()
                {
                    Items = new List<Article>
                    {
                        new Article
                        {
                            Title = ARTICLE_TITLE,
                            Sys = new SystemProperties
                            {
                                Id = ENTRY_ID
                            }
                        }
                    }
                });

            var actual = await _service.GetArticleForAsync(ENTRY_ID);

            actual.Title.Should().Be(ARTICLE_TITLE);
        }

        [Test]
        public async Task GetArticleForAsync_GivenAValidHubTypeAndSlug_ReturnsArticle()
        {
            _contentfulClient.Setup(o => o.GetEntries<Article>(It.IsAny<QueryBuilder<Article>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ContentfulCollection<Article>()
                {
                    Items = new List<Article>
                    {
                        new Article
                        {
                            Title = ARTICLE_TITLE,
                            Sys = new SystemProperties
                            {
                                Id = ENTRY_ID
                            },
                            HubType = HUB_TYPE,
                            Slug = SLUG
                        }
                    }
                });

            var actual = await _service.GetArticleForAsync(HUB_TYPE, SLUG);

            actual.Title.Should().Be(ARTICLE_TITLE);
        }
    }
}
