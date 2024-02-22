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
using SFA.DAS.Campaign.Application.Queries.SiteMap;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.SiteMap
{
    public class WhenGettingTheSiteMap
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Site_Map_Is_Returned(
            GetSiteMapQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SiteMapController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(It.IsAny<GetSiteMapQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetFullSiteMapAsync(CancellationToken.None) as OkObjectResult;

            var actualResult = controllerResult.Value as GetSiteMapResponse;
            Assert.That(actualResult, Is.Not.Null);
            actualResult.Map.MainContent.Pages.Should().NotBeNull();
        }
    }
}
