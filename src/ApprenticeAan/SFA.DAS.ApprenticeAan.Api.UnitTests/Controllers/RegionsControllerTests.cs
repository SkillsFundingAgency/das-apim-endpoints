using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers
{
    public class RegionsControllerTests
    {
        [Test]
        [MoqAutoData]
        public async Task And_MediatorCommandSuccessful_Then_ReturnOk(GetRegionsQueryResult response,
            [Frozen] Mock<IMediator> mockMediator)
        {
            mockMediator.Setup(m => m.Send(It.IsAny<GetRegionsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var controller = new RegionsController(mockMediator.Object);

            var result = await controller.GetRegions() as OkObjectResult;

            result.Should().NotBeNull();

            var model = result?.Value;

            model.Should().BeEquivalentTo(response);
        }

        [Test]
        [MoqAutoData]
        public async Task And_NoRegionsReturnedFromMediator_Then_Return500(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] RegionsController controller)
        {
            mockMediator.Setup(m => m.Send(It.IsAny<GetRegionsQuery>(), CancellationToken.None))
                .ReturnsAsync((GetRegionsQueryResult)null!);

            var controllerResult = await controller.GetRegions() as NotFoundResult;

            controllerResult?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}