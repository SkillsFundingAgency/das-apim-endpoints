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
using SFA.DAS.ApimDeveloper.Api.Controllers;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.ActivateUser;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.Controllers
{
    public class WhenActivatingAUser
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_No_Content_Result_Returned(
            Guid id,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            var actual = await controller.ActivateAccount(id) as NoContentResult;
            
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            mediator.Verify(x => 
                    x.Send(It.Is<ActivateUserCommand>(c => c.Id.Equals(id)), CancellationToken.None),
                Times.Once);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_InternalServerError_Returned(
            Guid id,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]UsersController controller)
        {
            mediator.Setup(x => 
                    x.Send(It.Is<ActivateUserCommand>(c => c.Id.Equals(id)), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.ActivateAccount(id) as StatusCodeResult;
            
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Validation_Error_BadRequest_Returned(
            Guid id,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]UsersController controller)
        {
            mediator.Setup(x => 
                    x.Send(It.Is<ActivateUserCommand>(c => c.Id.Equals(id)), CancellationToken.None))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.NotFound));

            var actual = await controller.ActivateAccount(id) as ObjectResult;
            
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}