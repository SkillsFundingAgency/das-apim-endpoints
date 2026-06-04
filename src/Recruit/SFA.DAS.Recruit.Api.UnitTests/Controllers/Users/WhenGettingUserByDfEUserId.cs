using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.User.Queries.GetUserByDfeUserId;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Users
{
    public class WhenGettingUserByDfEUserId
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Accounts_By_User_From_Mediator(
            string dfEUserId,
            GetUserByDfeUserIdQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] UsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetUserByDfeUserIdQuery>(c => c.DfeUserId.Equals(dfEUserId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetByDfEUserId(dfEUserId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetUserByDfeUserIdQueryResult;
            Assert.That(model, Is.Not.Null);
            model.User.Should().BeEquivalentTo(mediatorResult.User);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string dfEUserId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] UsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetUserByDfeUserIdQuery>(c => c.DfeUserId.Equals(dfEUserId)),
                    It.IsAny<CancellationToken>()))
              .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetByDfEUserId(dfEUserId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}