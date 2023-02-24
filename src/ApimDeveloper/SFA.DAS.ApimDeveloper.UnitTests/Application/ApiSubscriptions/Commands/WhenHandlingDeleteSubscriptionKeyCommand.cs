using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.RenewSubscriptionKey;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.ApiSubscriptions.Commands
{
    public class WhenHandlingRenewSubscriptionKeyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called(
            RenewSubscriptionKeyCommand command,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> mockApiClient,
            RenewSubscriptionKeyCommandHandler handler)
        {
            //Act
            await handler.Handle(command, CancellationToken.None);
            
            //Assert
            mockApiClient.Verify(x => x.PostWithResponseCode<string>(
                    It.Is<PostRenewSubscriptionKeyRequest>(c =>
                        c.PostUrl.Contains($"api/subscription/{command.AccountIdentifier}/renew/{command.ProductId}")),true),
                Times.Once);
        }
    }
}