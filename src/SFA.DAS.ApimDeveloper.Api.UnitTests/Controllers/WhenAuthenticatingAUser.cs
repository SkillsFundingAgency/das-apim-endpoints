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
using SFA.DAS.ApimDeveloper.Application.Users.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.Controllers
{
    public class WhenAuthenticatingAUser
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            string email, 
            string password,
            AuthenticateUserQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            mediator.Setup(x =>
                x.Send(It.Is<AuthenticateUserQuery>(c => c.Email.Equals(email) && c.Password.Equals(password)),
                    CancellationToken.None)).ReturnsAsync(mediatorResult);

            var actual = await controller.AuthenticateUser(email, password) as OkObjectResult;

            Assert.IsNotNull(actual);
            var actualModel = actual.Value as UserApiResponse;
            Assert.IsNotNull(actualModel);
            actualModel.User.Should().BeEquivalentTo(mediatorResult.User);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Null_Then_NotFound_Returned(
            string email, 
            string password,
            AuthenticateUserQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            mediator.Setup(x =>
                x.Send(It.Is<AuthenticateUserQuery>(c => c.Email.Equals(email) && c.Password.Equals(password)),
                    CancellationToken.None)).ReturnsAsync(new AuthenticateUserQueryResult
            {
                User = null
            });
            
            var actual = await controller.AuthenticateUser(email, password) as NotFoundResult;

            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_InternalServerError_Returned(
            string email,
            string password,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]UsersController controller)
        {
            mediator.Setup(x =>
                x.Send(It.Is<AuthenticateUserQuery>(c => c.Email.Equals(email) && c.Password.Equals(password)),
                    CancellationToken.None)).ThrowsAsync(new Exception());

            var actual = await controller.AuthenticateUser(email, password) as StatusCodeResult;
            
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}