using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetDashboardByUkprn;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Providers
{
    [TestFixture]
    public class WhenGettingDashboardByUkprn
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            int ukprn,
            string userId,
            GetDashboardByUkprnQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetDashboardByUkprnQuery>(c => c.Ukprn.Equals(ukprn)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetDashboard(ukprn, userId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetDashboardByUkprnQueryResult;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int ukprn,
            string userId,
            GetDashboardByUkprnQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetDashboardByUkprnQuery>(c => c.Ukprn.Equals(ukprn)),
                    It.IsAny<CancellationToken>()))
               .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetDashboard(ukprn, userId) as BadRequestResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
