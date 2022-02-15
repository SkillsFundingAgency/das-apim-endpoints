using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Api.ApiRequests;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Api.Controllers;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.AuthenticateUser;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.Controllers
{
    public class WhenAuthenticatingAUser
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            AuthenticateUserRequest request,
            AuthenticateUserCommandResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            mediator.Setup(x =>
                x.Send(It.Is<AuthenticateUserCommand>(c => c.Email.Equals(request.Email) && c.Password.Equals(request.Password)),
                    CancellationToken.None)).ReturnsAsync(mediatorResult);

            var actual = await controller.AuthenticateUser(request) as OkObjectResult;

            Assert.IsNotNull(actual);
            var actualModel = actual.Value as UserAuthenticationApiResponse;
            Assert.IsNotNull(actualModel);
            actualModel.User.Should().BeEquivalentTo(mediatorResult.User);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Null_Then_NotFound_Returned(
            AuthenticateUserRequest request,
            AuthenticateUserCommandResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            mediator.Setup(x =>
                x.Send(It.Is<AuthenticateUserCommand>(c => c.Email.Equals(request.Email) && c.Password.Equals(request.Password)),
                    CancellationToken.None)).ReturnsAsync(new AuthenticateUserCommandResult
            {
                User = null
            });
            
            var actual = await controller.AuthenticateUser(request) as NotFoundResult;

            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_InternalServerError_Returned(
            AuthenticateUserRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]UsersController controller)
        {
            mediator.Setup(x =>
                x.Send(It.Is<AuthenticateUserCommand>(c => c.Email.Equals(request.Email) && c.Password.Equals(request.Password)),
                    CancellationToken.None)).ThrowsAsync(new Exception());

            var actual = await controller.AuthenticateUser(request) as StatusCodeResult;
            
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_A_HttpRequestException_Then_Returned(
            AuthenticateUserRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]UsersController controller)
        {
            mediator.Setup(x =>
                x.Send(It.Is<AuthenticateUserCommand>(c => c.Email.Equals(request.Email) && c.Password.Equals(request.Password)),
                    CancellationToken.None)).ThrowsAsync(new HttpRequestContentException("error", HttpStatusCode.BadRequest, "error"));

            var actual = await controller.AuthenticateUser(request) as ObjectResult;
            
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}