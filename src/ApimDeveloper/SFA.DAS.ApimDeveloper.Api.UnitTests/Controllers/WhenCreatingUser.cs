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
using SFA.DAS.ApimDeveloper.Api.Controllers;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.CreateUser;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.Controllers
{
    public class WhenCreatingUser
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Handled_And_Created_Returned(
            Guid id,
            CreateUserRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]UsersController controller)
        {
            var actual = await controller.CreateUser(id, request) as CreatedResult;
            
            mediator.Verify(x =>
                x.Send(It.Is<CreateUserCommand>(c => 
                    c.Email.Equals(request.Email)
                    && c.Id.Equals(id)
                    && c.FirstName.Equals(request.FirstName)
                    && c.LastName.Equals(request.LastName)
                    && c.Password.Equals(request.Password)
                    && c.ConfirmationEmailLink.Equals(request.ConfirmationEmailLink)
                ), CancellationToken.None), Times.Once);
            actual.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_InternalServerError_Returned(
            Guid id,
            CreateUserRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]UsersController controller)
        {
            mediator.Setup(x =>
                x.Send(It.Is<CreateUserCommand>(c => c.Email.Equals(request.Email)), CancellationToken.None)).ThrowsAsync(new Exception());

            var actual = await controller.CreateUser(id, request) as StatusCodeResult;
            
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Validation_Error_BadRequest_Returned(
            Guid id,
            CreateUserRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]UsersController controller)
        {
            mediator.Setup(x =>
                    x.Send(It.Is<CreateUserCommand>(c => c.Email.Equals(request.Email)), CancellationToken.None))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest));

            var actual = await controller.CreateUser(id, request) as ObjectResult;
            
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}