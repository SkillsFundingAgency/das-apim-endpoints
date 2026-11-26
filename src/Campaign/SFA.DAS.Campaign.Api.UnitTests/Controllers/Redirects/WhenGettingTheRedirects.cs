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
using SFA.DAS.Campaign.Application.Queries.Redirects;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.Redirects
{
    public class WhenGettingTheRedirects
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Redirects_Are_Returned(
            GetRedirectsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] RedirectsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(It.IsAny<GetRedirectsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetRedirectsAsync(CancellationToken.None) as OkObjectResult;

            var actualResult = controllerResult.Value as GetRedirectsResponse;
            Assert.That(actualResult, Is.Not.Null);
            actualResult.Redirects.Should().BeEquivalentTo(mediatorResult.Redirects);
        }
    }
}
