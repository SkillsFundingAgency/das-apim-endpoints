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
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.Articles
{
    public class WhenGettingArticles
    {
        [Test, RecursiveMoqAutoData] 
        public async Task ThenTheApiIsCalledWithTheValidRequestParametersAndTheArticleIsReturned(
            GetArticleByHubAndSlugQuery query, 
            CmsContent response, 
            [Frozen]Mock<IReliableCacheStorageService> service, 
            GetArticleByHubAndSlugQueryHandler handler)
        {
            service.Setup(o =>
                    o.GetData<CmsContent>(
                        It.Is<GetArticleEntriesRequest>(c =>
                            c.GetUrl.Contains($"fields.hubType={query.Hub.ToTitleCase()}&fields.slug={query.Slug}")),
                        $"{query.Hub.ToTitleCase()}_{query.Slug}"))
                .ReturnsAsync(response);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Article.Should().BeEquivalentTo(response);
        }
    }
}
