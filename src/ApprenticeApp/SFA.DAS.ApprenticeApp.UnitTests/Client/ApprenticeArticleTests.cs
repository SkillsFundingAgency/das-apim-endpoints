using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Models.Contentful;

namespace SFA.DAS.ApprenticeApp.UnitTests.Client
{
    public class ApprenticeArticleTests
    {
        [Test]
        public void ApprenticeArticleTests_PropsMatch()
        {
            var sut = new ApprenticeArticle
            {
                EntryId = "1234",
                IsSaved = true,
                LikeStatus = true
            };

            Assert.That(sut.EntryId, Is.EqualTo("1234"));
            Assert.That(sut.IsSaved, Is.EqualTo(true));
            Assert.That(sut.LikeStatus, Is.EqualTo(true));
        }
    }
}