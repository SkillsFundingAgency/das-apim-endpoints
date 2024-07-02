using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Models.Contentful;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetUserSavedArticlesQueryResultTests
    {
        [Test, MoqAutoData]
        public async Task GetUserSavedArticlesQueryResultProps()
        {
            var sut = new GetUserSavedArticlesQueryResult
            {
                Articles = new System.Collections.Generic.List<Page>(),
                ApprenticeArticles = new ApprenticeArticleCollection()
            };

            Assert.That(sut.Articles, Is.Not.Null);
            Assert.That(sut.ApprenticeArticles, Is.Not.Null);
        }
    }
}