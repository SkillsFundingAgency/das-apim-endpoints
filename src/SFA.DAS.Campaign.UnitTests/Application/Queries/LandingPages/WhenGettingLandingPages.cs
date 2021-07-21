﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Application.Queries.LandingPage;
using SFA.DAS.Campaign.Application.Queries.Menu;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.LandingPages
{
    public class WhenGettingLandingPages
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Valid_Request_Parameters_And_The_Landing_Page_Is_Returned(
            GetLandingPageQuery query,
            GetMenuQueryResult menuResult,
            MenuPageModel.MenuPageContent menuContent,
            CmsContent apiResponse,
            HubPageModel response,
            [Frozen] Mock<IReliableCacheStorageService> service,
            [Frozen] Mock<IMediator> mediator,
            GetLandingPageQueryHandler handler)
        {
            service.Setup(o =>
                    o.GetData<CmsContent>(
                        It.Is<GetLandingPageRequest>(c =>
                            c.GetUrl.Contains($"fields.hubType={query.Hub.ToTitleCase()}&fields.slug={query.Slug}")),
                        $"{query.Hub.ToTitleCase()}_{query.Slug}_landingPage"))
                .ReturnsAsync(apiResponse);

            mediator.SetupMenu(menuResult, menuContent);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.PageModel.Should().BeEquivalentTo(response.Build(apiResponse, menuContent ));
        }
    }
}
