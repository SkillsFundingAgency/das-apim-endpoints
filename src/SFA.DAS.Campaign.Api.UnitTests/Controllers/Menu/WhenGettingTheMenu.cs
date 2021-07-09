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
            string menuType,
            GetMenuQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] MenuController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(It.Is<GetMenuQuery>(o => o.MenuType == menuType), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetMenuAsync(menuType, CancellationToken.None) as OkObjectResult;

            var actualResult = controllerResult.Value as GetMenuResponse;
            Assert.IsNotNull(actualResult);
            actualResult.Menu.MainContent.Items.Should().NotBeNull();
        }
    }
}
