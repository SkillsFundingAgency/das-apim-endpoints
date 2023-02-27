using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.DeleteSubscriptionKey;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.ApiSubscriptions.Commands
{
    public class WhenHandlingDeleteSubscriptionKeyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called(
            DeleteSubscriptionKeyCommand command,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> mockApiClient,
            DeleteSubscriptionKeyCommandHandler handler)
        {
            //Act
            await handler.Handle(command, CancellationToken.None);

            //Assert
            mockApiClient.Verify(x => x.Delete(
                    It.Is<DeleteSubscriptionKeyRequest>(c =>
                        c.DeleteUrl.Contains($"api/subscription/{command.AccountIdentifier}/delete/{command.ProductId}"))),
                Times.Once);
        }
    }
}