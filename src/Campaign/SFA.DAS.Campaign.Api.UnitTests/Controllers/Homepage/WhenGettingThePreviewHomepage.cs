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
using SFA.DAS.Campaign.Application.Queries.PreviewHomepage;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.Homepage
{
    public class WhenGettingThePreviewHomepage
    {
        [Test, MoqAutoData]
        public async Task Then_The_Preview_Homepage_Is_Returned(
            GetPreviewHomepageQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomepageController controller)
        {
            mockMediator.Setup(m => m.Send(It.IsAny<GetPreviewHomepageQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetPreviewHomepageAsync(CancellationToken.None) as ObjectResult;
            var actual = controllerResult.Value as GetHomepageResponse;

            controllerResult.StatusCode.Should().Be(200);
            actual.Homepage.Should().BeEquivalentTo(mediatorResult.PageModel);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Not_Found_A_Not_Found_Result_Is_Returned(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomepageController controller)
        {
            mockMediator.Setup(m => m.Send(It.IsAny<GetPreviewHomepageQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetPreviewHomepageQueryResult { PageModel = null });

            var controllerResult = await controller.GetPreviewHomepageAsync(CancellationToken.None) as ObjectResult;
            var actual = controllerResult.Value as NotFoundResponse;

            controllerResult.StatusCode.Should().Be(404);
            actual.Message.Should().Be("Preview homepage content not found.");
        }
    }
}
