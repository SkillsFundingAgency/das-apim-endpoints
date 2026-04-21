using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.InnerApi.Responses;
using System.Net;
using System.Threading;
using SFA.DAS.Recruit.Application.Queries.GetGeoPoint;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Locations
{
    public class WhenGettingGeoPoint
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_GeoPoint_From_Mediator(
            GetGeoPointQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetGeoPointQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetGeopoint("AB1 2CD") as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetGeoPointResponse;
            Assert.That(model, Is.Not.Null);
            model.GeoPoint.Should().BeEquivalentTo(mediatorResult.GetPointResponse.GeoPoint);
        }
    }
}
