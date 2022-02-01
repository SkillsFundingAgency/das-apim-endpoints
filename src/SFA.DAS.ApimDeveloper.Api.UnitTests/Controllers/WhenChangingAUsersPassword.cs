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
using SFA.DAS.ApimDeveloper.Application.Users.Commands.ChangePassword;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.Controllers
{
    public class WhenChangingAUsersPassword
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_No_Content_Result_Returned(
            Guid id,
            ChangePasswordRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] UsersController controller)
        {
            var actual = await controller.ChangePassword(id, request) as NoContentResult;
            
            actual!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            mockMediator.Verify(mediator => 
                    mediator.Send(It.Is<ChangePasswordCommand>(c => 
                            c.Id.Equals(id)
                            && c.Password.Equals(request.Password)), 
                        CancellationToken.None),
                Times.Once);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_InternalServerError_Returned(
            Guid id,
            ChangePasswordRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]UsersController controller)
        {
            mockMediator
                .Setup(mediator => 
                    mediator.Send(It.IsAny<ChangePasswordCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.ChangePassword(id, request) as StatusCodeResult;
            
            actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Validation_Error_BadRequest_Returned(
            Guid id,
            ChangePasswordRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]UsersController controller)
        {
            mockMediator.Setup(mediator => 
                    mediator.Send(It.IsAny<ChangePasswordCommand>(), CancellationToken.None))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.NotFound));

            var actual = await controller.ChangePassword(id, request) as ObjectResult;
            
            actual!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}