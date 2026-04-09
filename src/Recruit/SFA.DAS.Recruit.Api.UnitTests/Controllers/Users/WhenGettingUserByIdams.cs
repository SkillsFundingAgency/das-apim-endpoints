using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.User.Queries.GetUserByIdamsId;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Users
{
    public class WhenGettingUserByIdams
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Accounts_By_User_From_Mediator(
            string idams,
            GetUserByIdamsIdQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] UsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetUserByIdamsIdQuery>(c => c.IdamsId.Equals(idams)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetByIdams(idams) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetUserByIdamsIdQueryResult;
            Assert.That(model, Is.Not.Null);
            model.User.Should().BeEquivalentTo(mediatorResult.User);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string idams,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] UsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetUserByIdamsIdQuery>(c => c.IdamsId.Equals(idams)),
                    It.IsAny<CancellationToken>()))
               .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetByIdams(idams) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}