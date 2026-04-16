using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models.Users;
using SFA.DAS.Recruit.Application.User.Queries.GetUserByEmail;
using SFA.DAS.Recruit.Enums;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Users
{
    public class WhenGettingUsersByEmail
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Accounts_By_User_From_Mediator(
            string email,
            UserType userType,
            GetUserByEmailQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] UsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetUserByEmailQuery>(c => c.Email.Equals(email) && c.UserType.Equals(userType)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetByEmailId(new GetUserRequest
            {
                Email = email, 
                UserType = userType
            }) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetUserByEmailQueryResult;
            Assert.That(model, Is.Not.Null);
            model.User.Should().BeEquivalentTo(mediatorResult.User);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string email,
            UserType userType,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] UsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetUserByEmailQuery>(c => c.Email.Equals(email) && c.UserType.Equals(userType)),
                    It.IsAny<CancellationToken>()))
               .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetByEmailId(new GetUserRequest
            {
                Email = email,
                UserType = userType
            }) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}