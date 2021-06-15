using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Articles;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.Articles
{
    public class WhenGettingArticles
    {
        [Test, RecursiveMoqAutoData]
        public async Task ThenTheApiIsCalledWithTheValidRequestParametersAndTheArticleIsReturned(
            GetArticleByHubAndSlugQuery query, 
            GetArticleResponse response, 
            [Frozen]Mock<IContentfulService> service, GetArticleByHubAndSlugQueryHandler handler)
        {
            service.Setup(o =>
                    o.GetArticleForAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response.Article);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Article.Should().BeEquivalentTo(response.Article);
        }
    }
}
