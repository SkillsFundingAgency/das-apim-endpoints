using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Models.Contentful;

namespace SFA.DAS.ApprenticeApp.UnitTests.Client
{
    public class ApprenticeAppArticlePageTests
    {
        [Test]
        public void ApprenticeAppArticlePageTests_PropsMatch()
        {
            var sut = new ApprenticeAppArticlePage
            {
                Heading = "heading",
                Content = "content",
                Id = "123",
                ParentPageEntityId = "1234",
                Slug = "slug",
                ParentPageTitle = "title",
                Sys = new Contentful.Core.Models.SystemProperties()
            };

            Assert.That(sut.Heading, Is.EqualTo("heading"));
            Assert.That(sut.Content, Is.EqualTo("content"));
            Assert.That(sut.Id, Is.EqualTo("123"));
            Assert.That(sut.ParentPageEntityId, Is.EqualTo("1234"));
            Assert.That(sut.Slug, Is.EqualTo("slug"));
            Assert.That(sut.ParentPageTitle, Is.EqualTo("title"));
        }
    }
}