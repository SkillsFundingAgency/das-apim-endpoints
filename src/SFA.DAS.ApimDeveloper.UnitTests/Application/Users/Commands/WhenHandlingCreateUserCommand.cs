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
                x => x.Put(It.Is<PutCreateUserRequest>(c =>
                    c.PutUrl.Contains(command.Id.ToString())
                    && ((PutCreateUserRequestData)c.Data).Email.Equals(command.Email) 
                    && ((PutCreateUserRequestData)c.Data).Password.Equals(command.Password)
                    && ((PutCreateUserRequestData)c.Data).FirstName.Equals(command.FirstName)
                    && ((PutCreateUserRequestData)c.Data).LastName.Equals(command.LastName)
                    && ((PutCreateUserRequestData)c.Data).ConfirmationEmailLink.Equals(command.ConfirmationEmailLink)
                )), Times.Once);

        }
    }
}