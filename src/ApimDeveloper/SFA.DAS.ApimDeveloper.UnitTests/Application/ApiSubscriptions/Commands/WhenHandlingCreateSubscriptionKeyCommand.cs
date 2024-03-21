using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.CreateSubscriptionKey;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.ApiSubscriptions.Commands
{
    public class WhenHandlingCreateSubscriptionKeyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Sent_To_Create_The_Key_And_Product_Returned(
            CreateSubscriptionKeyCommand command,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> client,
            CreateSubscriptionKeyCommandHandler handler)
        {
            var createResponse = new ApiResponse<object>("", HttpStatusCode.Created, "");
            client.Setup(
                x => x.PostWithResponseCode<object>(It.Is<PostCreateSubscriptionKeyRequest>(c =>
                    ((CreateSubscriptionApiRequest)c.Data).AccountIdentifier.Equals(command.AccountIdentifier) &&
                    ((CreateSubscriptionApiRequest)c.Data).ProductId.Equals(command.ProductId)),true)).ReturnsAsync(createResponse);
            
            await handler.Handle(command, CancellationToken.None);
            client.Verify(
                x => x.PostWithResponseCode<object>(It.Is<PostCreateSubscriptionKeyRequest>(c =>
                    ((CreateSubscriptionApiRequest)c.Data).AccountIdentifier.Equals(command.AccountIdentifier) &&
                    ((CreateSubscriptionApiRequest)c.Data).ProductId.Equals(command.ProductId)),true), Times.Once);

        }

        [Test, MoqAutoData]
        public void Then_If_Error_Creating_A_HttpRequestContentException_Is_Thrown(
            CreateSubscriptionKeyCommand command,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> client,
            CreateSubscriptionKeyCommandHandler handler)
        {
            var createResponse = new ApiResponse<object>("", HttpStatusCode.BadRequest, "error");
            client.Setup(
                x => x.PostWithResponseCode<object>(It.Is<PostCreateSubscriptionKeyRequest>(c =>
                    ((CreateSubscriptionApiRequest)c.Data).AccountIdentifier.Equals(command.AccountIdentifier) &&
                    ((CreateSubscriptionApiRequest)c.Data).ProductId.Equals(command.ProductId)),true)).ReturnsAsync(createResponse);
            
            Assert.ThrowsAsync<HttpRequestContentException>(() => handler.Handle(command, CancellationToken.None));
        }

    }
}