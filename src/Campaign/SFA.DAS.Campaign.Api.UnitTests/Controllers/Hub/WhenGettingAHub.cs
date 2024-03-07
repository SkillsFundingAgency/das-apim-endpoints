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
using SFA.DAS.Campaign.Application.Queries.Hub;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.Hub
{
    public class WhenGettingAHub
    {
        [Test, RecursiveMoqAutoData]
        public async Task And_Given_A_Valid_Hub_Then_The_Hub_Is_Returned(
            string hubName,
            GetHubQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HubController controller)
        {
            SetupMediator(mediatorResult, mockMediator, hubName);

            var controllerResult = await InstantiateController<OkObjectResult>(controller, hubName);

            var actualResult = controllerResult.Value as GetHubResponse;
            Assert.That(actualResult, Is.Not.Null);
            actualResult.Hub.Should().BeEquivalentTo(mediatorResult.PageModel);
        }
        
        [Test, RecursiveMoqAutoData]
        public async Task And_Given_Invalid_Hub_And_A_Slug_Then_The_Article_Is_Not_Returned(
            string hubName,
            GetHubQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HubController controller)
        {
            mediatorResult.PageModel = null;

            SetupMediator(new GetHubQueryResult(), mockMediator, hubName);

            var controllerResult = await InstantiateController<NotFoundObjectResult>(controller, hubName);

            var actualResult = controllerResult.Value as NotFoundResponse;
            Assert.That(actualResult, Is.Not.Null);
            actualResult.Message.Should().Be($"Hub not found for {hubName}");
        }

        private static async Task<T> InstantiateController<T>(HubController controller, string hubName)
        {
            var controllerResult = (T)await controller.GetHubAsync(hubName, CancellationToken.None);

            return controllerResult;
        }

        private static void SetupMediator(GetHubQueryResult mediatorResult, Mock<IMediator> mockMediator, string hubName)
        {
            mockMediator
                .Setup(mediator => mediator.Send(It.Is<GetHubQuery>(c => c.Hub.Equals(hubName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
        }
    }
}
