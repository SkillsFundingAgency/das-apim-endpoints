using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Api.Controllers;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Menu;

using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.Menu
{
    public class WhenGettingTheMenu
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Menu_Is_Returned(
            GetMenuQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] MenuController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(It.IsAny<GetMenuQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetMenuAsync(CancellationToken.None) as OkObjectResult;

            var actualResult = controllerResult.Value as GetMenuResponse;
            Assert.That(actualResult, Is.Not.Null);
            mockMediator.Verify(x => x.Send(It.Is<GetMenuQuery>(c => c.MenuType.Equals("TopLevel")), CancellationToken.None), Times.Once);
            mockMediator.Verify(x => x.Send(It.Is<GetMenuQuery>(c => c.MenuType.Equals("Apprentices")), CancellationToken.None), Times.Once);
            mockMediator.Verify(x => x.Send(It.Is<GetMenuQuery>(c => c.MenuType.Equals("Employers")), CancellationToken.None), Times.Once);
            mockMediator.Verify(x => x.Send(It.Is<GetMenuQuery>(c => c.MenuType.Equals("Influencers")), CancellationToken.None), Times.Once);
        }
    }
}
