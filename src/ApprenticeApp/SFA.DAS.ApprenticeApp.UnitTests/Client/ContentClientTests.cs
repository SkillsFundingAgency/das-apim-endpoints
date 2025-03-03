using System.Collections.Generic;
using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Client;

namespace SFA.DAS.ApprenticeApp.UnitTests.Client
{
    public class ContentClientTests
    {
        private Mock<IContentfulClient> _contentfulClient;
        private Mock<IConfiguration> _configuration;
        private ContentClient _contentClient;

        [SetUp]
        public void Setup()
        {
            _contentfulClient = new Mock<IContentfulClient>();
            _configuration = new Mock<IConfiguration>();
            _contentClient = new ContentClient(new TestEntityResolver(), _contentfulClient.Object,
                 _configuration.Object);
        }

        [Test]
        public async Task GetEntries()
        {
            var items = new List<TestPage>();

            _contentfulClient.Setup(x => x.GetEntries(It.IsAny<QueryBuilder<TestPage>>(), default))
                .ReturnsAsync(new ContentfulCollection<TestPage> { Items = items });
            var result = await _contentClient.GetEntries<TestPage>("", "", "");
            _contentfulClient.Verify(x => x.GetEntries(It.IsAny<QueryBuilder<TestPage>>(), default));
        }

        [Test]
        public async Task GetAllPages()
        {
            var items = new List<TestPage>();
            _contentfulClient.Setup(x => x.GetEntries(It.IsAny<QueryBuilder<TestPage>>(), default))
                .ReturnsAsync(new ContentfulCollection<TestPage> { Items = items });
            var result = await _contentClient.GetAllPages();
            result.Should().BeNull();
        }

        [Test]
        public async Task GetByEntryId()
        {
            var items = new List<TestPage>();
            _contentfulClient.Setup(x => x.GetEntries(It.IsAny<QueryBuilder<TestPage>>(), default))
                .ReturnsAsync(new ContentfulCollection<TestPage> { Items = items });
            var result = await _contentClient.GetByEntryId("123");
            result.Should().BeNull();
        }

        [Test]
        public async Task GetByEntryIdWithChildren()
        {
            var items = new List<TestPage>();
            _contentfulClient.Setup(x => x.GetEntries(It.IsAny<QueryBuilder<TestPage>>(), default))
                .ReturnsAsync(new ContentfulCollection<TestPage> { Items = items });
            var result = await _contentClient.GetByEntryIdWithChildren("123");
            result.Should().BeNull();
        }

        [Test]
        public async Task GetBySlugWithChildren()
        {
            var items = new List<TestPage>();
            _contentfulClient.Setup(x => x.GetEntries(It.IsAny<QueryBuilder<TestPage>>(), default))
                .ReturnsAsync(new ContentfulCollection<TestPage> { Items = items });
            var result = await _contentClient.GetBySlugWithChildren("123");
            result.Should().BeNull();
        }

        [Test]
        public async Task GetPagesByContentType()
        {
            var items = new List<TestPage>();
            _contentfulClient.Setup(x => x.GetEntries(It.IsAny<QueryBuilder<TestPage>>(), default))
                .ReturnsAsync(new ContentfulCollection<TestPage> { Items = items });
            var result = await _contentClient.GetPagesByContentType<TestPage>("123");
            result.Should().BeNull();
        }
    }
}