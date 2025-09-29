using System;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetAlertsByUkprn;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Providers
{
    [TestFixture]
    public class WhenGettingAlertsVacanciesByUkprn
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            int ukprn,
            string userId,
            GetAlertsByUkprnQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAlertsByUkprnQuery>(c => c.Ukprn.Equals(ukprn) &&
                                                      c.UserId.Equals(userId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProviderAlerts(ukprn, userId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAlertsByUkprnQueryResult;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int ukprn,
            string userId,
            GetAlertsByUkprnQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAlertsByUkprnQuery>(c => c.Ukprn.Equals(ukprn) &&
                                                          c.UserId.Equals(userId)),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetProviderAlerts(ukprn, userId) as BadRequestResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
