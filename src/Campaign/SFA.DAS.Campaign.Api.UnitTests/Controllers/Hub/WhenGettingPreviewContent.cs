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
using SFA.DAS.Campaign.Application.Queries.PreviewHub;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.Hub
{
    public class WhenGettingPreviewContent
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Article_Is_Returned_From_The_Mediator_Query(
            string hubName,
            GetPreviewHubQueryResult mediatorResult, 
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HubController controller)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetPreviewHubQuery>(c => c.Hub.Equals(hubName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actual = await controller.GetPreviewHub(hubName) as OkObjectResult;
         
            Assert.That(actual, Is.Not.Null);
            var actualResult = actual.Value as GetHubResponse;
            Assert.That(actualResult, Is.Not.Null);
            actualResult.Hub.Should().BeEquivalentTo(mediatorResult.PageModel);
        }
        
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_NotFound_Is_Returned_If_No_Result(
            string hubName,
            GetPreviewHubQueryResult mediatorResult, 
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HubController controller)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetPreviewHubQuery>(c => c.Hub.Equals(hubName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetPreviewHubQueryResult
                {
                    PageModel = null
                });

            var actual = await controller.GetPreviewHub(hubName) as NotFoundObjectResult;
         
            Assert.That(actual, Is.Not.Null);
            var actualResult = actual.Value as NotFoundResponse;
            Assert.That(actualResult, Is.Not.Null);
            actualResult.Message.Should().Be($"Preview hub not found for {hubName}");
        }
    }
}