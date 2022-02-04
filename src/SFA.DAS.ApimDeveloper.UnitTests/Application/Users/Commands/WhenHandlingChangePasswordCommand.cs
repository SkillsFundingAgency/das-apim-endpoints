using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.ChangePassword;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.Users.Commands
{
    public class WhenHandlingChangePasswordCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Endpoint_Called(
            ChangePasswordCommand command,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> apimDeveloperApiClient,
            ChangePasswordCommandHandler handler)
        {
            await handler.Handle(command, CancellationToken.None);
            
            apimDeveloperApiClient.Verify(x=>x.Put(It.Is<PutUpdateUserRequest>(c=>
                c.PutUrl.Contains(command.Id.ToString())
                && ((UserRequestData)c.Data).Password.Equals(command.Password)
            )), Times.Once);
        }
    }
}