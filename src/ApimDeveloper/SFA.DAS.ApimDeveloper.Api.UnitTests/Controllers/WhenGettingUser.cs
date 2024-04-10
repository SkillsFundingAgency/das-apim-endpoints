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
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Api.Controllers;
using SFA.DAS.ApimDeveloper.Application.Users.Queries.GetUser;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.Controllers
{
    public class WhenGettingUser
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            string email,
            GetUserQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            mediator
                .Setup(x => x.Send(
                    It.Is<GetUserQuery>(c => c.Email.Equals(email)),
                    CancellationToken.None))
                .ReturnsAsync(mediatorResult);

            var actual = await controller.GetUser(email) as OkObjectResult;

            var actualModel = actual!.Value as UserApiResponse;
            actualModel.Should().BeEquivalentTo((UserApiResponse)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Null_Then_NotFound_Returned(
            string email,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            mediator
                .Setup(x => x.Send(
                    It.Is<GetUserQuery>(c => c.Email.Equals(email)),
                    CancellationToken.None))
                .ReturnsAsync(new GetUserQueryResult());
            
            var actual = await controller.GetUser(email) as NotFoundResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_InternalServerError_Returned(
            string email,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]UsersController controller)
        {
            mediator
                .Setup(x => x.Send(
                    It.IsAny<GetUserQuery>(),
                    CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetUser(email) as StatusCodeResult;
            
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_A_HttpRequestException_Then_Returned(
            string email,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]UsersController controller)
        {
            mediator
                .Setup(x => x.Send(
                    It.IsAny<GetUserQuery>(),
                    CancellationToken.None))
                .ThrowsAsync(new HttpRequestContentException("error", HttpStatusCode.BadRequest, "error"));

            var actual = await controller.GetUser(email) as ObjectResult;
            
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}