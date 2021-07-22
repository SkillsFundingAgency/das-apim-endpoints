﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MediatR;
using Moq;
using SFA.DAS.Campaign.Application.Queries.Menu;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries
{
    public static class MediatorExtensions
    {
        public static void SetupMenu(this Mock<IMediator> mediator, GetMenuQueryResult menuResult, MenuPageModel.MenuPageContent menuContent)
        {
            mediator.Setup(o => o.Send(It.IsAny<GetMenuQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                menuResult);

            menuContent.Apprentices = menuResult.PageModel.MainContent;
            menuContent.Employers = menuResult.PageModel.MainContent;
            menuContent.Influencers = menuResult.PageModel.MainContent;
            menuContent.TopLevel = menuResult.PageModel.MainContent;
        }
    }
}
