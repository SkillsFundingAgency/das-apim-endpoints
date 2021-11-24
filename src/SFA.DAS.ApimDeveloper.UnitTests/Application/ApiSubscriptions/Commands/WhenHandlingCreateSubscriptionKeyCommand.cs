using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.CreateSubscriptionKey;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.ApiSubscriptions.Commands
{
    public class WhenHandlingCreateSubscriptionKeyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Sent_To_Create_The_Key_And_Product_Returned(
            CreateSubscriptionKeyCommand command,
            GetAvailableApiProductsResponse apiResponse,
            GetApiProductSubscriptionsResponse apiSubscriptionsResponse,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> client,
            CreateSubscriptionKeyCommandHandler handler)
        {
            apiResponse.Products.First().Id = command.ProductId;
            apiSubscriptionsResponse.Subscriptions.First().Name = command.ProductId;
            client.Setup(x =>
                    x.Get<GetAvailableApiProductsResponse>(
                        It.Is<GetAvailableApiProductsRequest>(c => c.GetUrl.EndsWith($"?group={command.AccountType}"))))
                .ReturnsAsync(apiResponse);
            client.Setup(x =>
                    x.Get<GetApiProductSubscriptionsResponse>(
                        It.Is<GetApiProductSubscriptionsRequest>(c => c.GetUrl.EndsWith($"/{command.AccountIdentifier}"))))
                .ReturnsAsync(apiSubscriptionsResponse);
            
            var actual = await handler.Handle(command, CancellationToken.None);

            client.Verify(
                x => x.PostWithResponseCode<object>(It.Is<PostCreateSubscriptionKeyRequest>(c =>
                    ((CreateSubscriptionApiRequest)c.Data).AccountIdentifier.Equals(command.AccountIdentifier) &&
                    ((CreateSubscriptionApiRequest)c.Data).ProductId.Equals(command.ProductId))), Times.Once);
            actual.Product.Should().BeEquivalentTo(apiResponse.Products.First());
            actual.Subscription.Should().BeEquivalentTo(apiSubscriptionsResponse.Subscriptions.First());
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Matching_Product_Null_Returned(
            CreateSubscriptionKeyCommand command,
            GetAvailableApiProductsResponse apiResponse,
            GetApiProductSubscriptionsResponse apiSubscriptionsResponse,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> client,
            CreateSubscriptionKeyCommandHandler handler)
        {
            apiSubscriptionsResponse.Subscriptions.First().Name = command.ProductId;
            client.Setup(x =>
                    x.Get<GetAvailableApiProductsResponse>(
                        It.Is<GetAvailableApiProductsRequest>(c => c.GetUrl.EndsWith($"?group={command.AccountType}"))))
                .ReturnsAsync(apiResponse);
            client.Setup(x =>
                    x.Get<GetApiProductSubscriptionsResponse>(
                        It.Is<GetApiProductSubscriptionsRequest>(c => c.GetUrl.EndsWith($"/{command.AccountIdentifier}"))))
                .ReturnsAsync(apiSubscriptionsResponse);
            
            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().BeNull();
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_No_Matching_Subscription_Null_Returned(
            CreateSubscriptionKeyCommand command,
            GetAvailableApiProductsResponse apiResponse,
            GetApiProductSubscriptionsResponse apiSubscriptionsResponse,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> client,
            CreateSubscriptionKeyCommandHandler handler)
        {
            apiResponse.Products.First().Id = command.ProductId;
            client.Setup(x =>
                    x.Get<GetAvailableApiProductsResponse>(
                        It.Is<GetAvailableApiProductsRequest>(c => c.GetUrl.EndsWith($"?group={command.AccountType}"))))
                .ReturnsAsync(apiResponse);
            client.Setup(x =>
                    x.Get<GetApiProductSubscriptionsResponse>(
                        It.Is<GetApiProductSubscriptionsRequest>(c => c.GetUrl.EndsWith($"/{command.AccountIdentifier}"))))
                .ReturnsAsync(apiSubscriptionsResponse);
            
            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().BeNull();
        }
    }
}