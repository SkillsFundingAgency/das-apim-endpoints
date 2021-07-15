using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Articles;
using SFA.DAS.Campaign.Application.Queries.Menu;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.Articles
{
    public class WhenGettingArticles
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Valid_Request_Parameters_And_The_Article_Is_Returned(
            GetArticleByHubAndSlugQuery query,
            GetMenuQueryResult menuResult,
            MenuPageModel.MenuPageContent menuContent, 
            CmsContent apiResponse,
            CmsPageModel response,
            [Frozen] Mock<IReliableCacheStorageService> service,
            [Frozen] Mock<IMediator> mediator,
            GetArticleByHubAndSlugQueryHandler handler)
        {
            service.Setup(o =>
                    o.GetData<CmsContent>(
                        It.Is<GetArticleEntriesRequest>(c =>
                            c.GetUrl.Contains($"fields.hubType={query.Hub.ToTitleCase()}&fields.slug={query.Slug}")),
                        $"{query.Hub.ToTitleCase()}_{query.Slug}_article"))
                .ReturnsAsync(apiResponse);

            mediator.SetupMenu(menuResult, menuContent);

            var actual = await handler.Handle(query, CancellationToken.None);

            var cmsPageModel = response.Build(apiResponse, menuContent);
            
            actual.PageModel.Should().BeEquivalentTo(cmsPageModel);
            service.Verify(x=>x.GetData<CmsContent>(It.IsAny<GetLandingPageRequest>(), It.IsAny<string>()), Times.Never);
        }
    }
}
