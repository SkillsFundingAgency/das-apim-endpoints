using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.ActivateUser;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.Users.Commands
{
    public class WhenHandlingActivateUserCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Endpoint_Called(
            ActivateUserCommand command,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> apimDeveloperApiClient,
            ActivateUserCommandHandler handler)
        {
            await handler.Handle(command, CancellationToken.None);
            
            apimDeveloperApiClient.Verify(x=>x.Put(It.Is<PutUpdateUserRequest>(c=>
                c.PutUrl.Contains(command.Id.ToString())
                && ((UserRequestData)c.Data).State.Equals(1)
                )), Times.Once);
        }
    }
}