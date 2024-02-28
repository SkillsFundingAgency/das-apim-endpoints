using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Api.Controllers;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Panel;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.Panel
{
    public class WhenGettingPanel
    {
        [Test, MoqAutoData]
        public async Task GivenIdIsValid_ThenPanelIsReturned(
            int panelId, 
            GetPanelQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PanelController controller)
        {
            mediatorResult.Panel.MainContent.Id = panelId;
            mockMediator.Setup(mediator => mediator.Send(It.Is<GetPanelQuery>(p => p.Id.Equals(panelId)), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetPanelAsync(panelId, CancellationToken.None) as ObjectResult;
            var actualResult = controllerResult.Value as GetPanelResponse;

            Assert.Multiple(() =>
            {
                Assert.That(controllerResult, Is.Not.Null);
                Assert.That(actualResult, Is.Not.Null);
                Assert.That(controllerResult.StatusCode.Value.Equals(200));
                Assert.That(actualResult.Panel.Equals(mediatorResult.Panel));
            });
        }

        [Test, MoqAutoData]
        public async Task GivenIdIsInvalid_ThenNotFoundObjectResultReturned
            (int panelId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PanelController controller)
        {
            mockMediator.Setup(mediator => mediator.Send(It.Is<GetPanelQuery>(p => p.Id.Equals(panelId)), It.IsAny<CancellationToken>())).ReturnsAsync(new GetPanelQueryResult() { Panel = null});

            var controllerResult = await controller.GetPanelAsync(panelId, CancellationToken.None) as ObjectResult;
            var actualResult = controllerResult.Value as NotFoundResponse;

            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.Not.Null);
                Assert.That(controllerResult.StatusCode.Equals(404));
                Assert.That(actualResult.Message.Equals($"Panel not found for panel id {panelId}."));
            });
        }
    }
}
