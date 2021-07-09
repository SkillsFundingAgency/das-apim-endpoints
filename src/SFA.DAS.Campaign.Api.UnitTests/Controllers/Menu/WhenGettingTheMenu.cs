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
            Assert.IsNotNull(actualResult);
            actualResult.Menu.MainContent.Apprentices.Should().NotBeNullOrEmpty();
            actualResult.Menu.MainContent.Employers.Should().NotBeNullOrEmpty();
            actualResult.Menu.MainContent.Influencers.Should().NotBeNullOrEmpty();
            actualResult.Menu.MainContent.TopLevel.Should().NotBeNullOrEmpty();
        }
    }
}
