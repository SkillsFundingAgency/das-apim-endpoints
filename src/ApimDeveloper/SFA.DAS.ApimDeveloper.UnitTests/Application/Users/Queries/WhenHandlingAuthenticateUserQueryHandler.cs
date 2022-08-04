using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.AuthenticateUser;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.Users.Queries
{
    public class WhenHandlingAuthenticateUserQueryHandler
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_User_Returned(
            PostAuthenticateUserResult apiResponse,
            AuthenticateUserCommand command,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> apimDeveloperApiClient,
            AuthenticateUserCommandHandler handler)
        {
            apimDeveloperApiClient.Setup(x =>
                    x.PostWithResponseCode<PostAuthenticateUserResult>(It.Is<PostAuthenticateUserRequest>(c =>
                    ((PostAuthenticateUserRequestData)c.Data).Email.Equals(command.Email)
                    && ((PostAuthenticateUserRequestData)c.Data).Password.Equals(command.Password)),true))
                .ReturnsAsync(new ApiResponse<PostAuthenticateUserResult>(apiResponse, HttpStatusCode.OK, ""));
            
            var actual = await handler.Handle(command, CancellationToken.None);

            actual.User.Should().BeEquivalentTo(apiResponse);
        }

        [Test, MoqAutoData]
        public void Then_If_Error_A_HttpContextException_Is_Thrown(
            PostAuthenticateUserResult apiResponse,
            AuthenticateUserCommand command,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> apimDeveloperApiClient,
            AuthenticateUserCommandHandler handler)
        {
            apimDeveloperApiClient.Setup(x =>
                    x.PostWithResponseCode<PostAuthenticateUserResult>(It.Is<PostAuthenticateUserRequest>(c =>
                        ((PostAuthenticateUserRequestData)c.Data).Email.Equals(command.Email)
                        && ((PostAuthenticateUserRequestData)c.Data).Password.Equals(command.Password)),true))
                .ReturnsAsync(new ApiResponse<PostAuthenticateUserResult>(apiResponse, HttpStatusCode.BadRequest, "An Error"));
            
            Assert.ThrowsAsync<HttpRequestContentException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}