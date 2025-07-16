using System;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.User.Commands.UpsertUser;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Users;

public class WhenCallingPostUser
{
    [Test, MoqAutoData]
    public async Task Then_Request_Is_Handled_And_Mediator_Command_Sent(
        Guid id,
        UserDto user,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController controller)
    {
        var actual = await controller.UpsertUser(id, user) as CreatedResult;

        actual.Should().NotBeNull();
        mediator.Verify(x => x.Send(
                It.Is<UpsertUserCommand>(c => 
                    c.Id == id &&
                    c.User.Name == ((InnerApi.Requests.UserDto)user).Name), 
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Exception_Thrown_InternalServerError_Returned(
        Guid id,
        UserDto user,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController controller)
    {
        mediator.Setup(x => x.Send(
            It.Is<UpsertUserCommand>(c => 
                c.Id == id &&
                c.User.Name == ((InnerApi.Requests.UserDto)user).Name), 
            It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        
        var actual = await controller.UpsertUser(id, user) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        
    }
}