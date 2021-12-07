using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.CreateUser;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.Users.Commands
{
    public class WhenHandlingCreateUserCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Sent_To_Create_User(
            CreateUserCommand command,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> client,
            CreateUserCommandHandler handler)
        {
            await handler.Handle(command, CancellationToken.None);
            
            client.Verify(
                x => x.PostWithResponseCode<object>(It.Is<PostCreateUserRequest>(c =>
                    c.PostUrl.Contains(command.Id.ToString())
                    && ((UserRequestData)c.Data).Email.Equals(command.Email) 
                    && ((UserRequestData)c.Data).Password.Equals(command.Password)
                    && ((UserRequestData)c.Data).FirstName.Equals(command.FirstName)
                    && ((UserRequestData)c.Data).LastName.Equals(command.LastName)
                    && ((UserRequestData)c.Data).ConfirmationEmailLink.Equals(command.ConfirmationEmailLink)
                    && ((UserRequestData)c.Data).State.Equals(0)
                )), Times.Once);
        }

        [Test, MoqAutoData]
        public void Then_If_Error_HttpRequestContentException_Returned(
            CreateUserCommand command,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> client,
            CreateUserCommandHandler handler)
        {
            client.Setup(
                x => x.PostWithResponseCode<object>(It.Is<PostCreateUserRequest>(c =>
                    c.PostUrl.Contains(command.Id.ToString())
                    && ((UserRequestData)c.Data).Email.Equals(command.Email) 
                    && ((UserRequestData)c.Data).Password.Equals(command.Password)
                    && ((UserRequestData)c.Data).FirstName.Equals(command.FirstName)
                    && ((UserRequestData)c.Data).LastName.Equals(command.LastName)
                    && ((UserRequestData)c.Data).ConfirmationEmailLink.Equals(command.ConfirmationEmailLink)
                    && ((UserRequestData)c.Data).State.Equals(0)
                ))).ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.BadRequest, "Error"));

            Assert.ThrowsAsync<HttpRequestContentException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}