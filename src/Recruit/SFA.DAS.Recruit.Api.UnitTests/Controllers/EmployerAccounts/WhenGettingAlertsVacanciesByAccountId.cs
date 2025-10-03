using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetAlertsByAccountId;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.EmployerAccounts
{
    [TestFixture]
    public class WhenGettingAlertsVacanciesByUkprn
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            long accountId,
            string userId,
            GetAlertsByAccountIdQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAlertsByAccountIdQuery>(c => c.AccountId.Equals(accountId) &&
                                                         c.UserId.Equals(userId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetEmployerAlerts(accountId, userId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAlertsByAccountIdQueryResult;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long accountId,
            string userId,
            GetAlertsByAccountIdQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAlertsByAccountIdQuery>(c => c.AccountId.Equals(accountId) &&
                                                          c.UserId.Equals(userId)),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetEmployerAlerts(accountId, userId) as BadRequestResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
