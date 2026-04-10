using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.User.Queries.GetUsersByEmployerAccountId;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Users
{
    public class WhenGettingUsersByEmployerAccountId
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Accounts_By_User_From_Mediator(
            long employerAccountId,
            GetUsersByEmployerAccountIdQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] UsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetUsersByEmployerAccountIdQuery>(c => c.EmployerAccountId.Equals(employerAccountId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetByEmployerAccountId(employerAccountId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetUsersByEmployerAccountIdQueryResult;
            Assert.That(model, Is.Not.Null);
            model.Users.Should().BeEquivalentTo(mediatorResult.Users);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long employerAccountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] UsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetUsersByEmployerAccountIdQuery>(c => c.EmployerAccountId.Equals(employerAccountId)),
                    It.IsAny<CancellationToken>()))
              .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetByEmployerAccountId(employerAccountId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}