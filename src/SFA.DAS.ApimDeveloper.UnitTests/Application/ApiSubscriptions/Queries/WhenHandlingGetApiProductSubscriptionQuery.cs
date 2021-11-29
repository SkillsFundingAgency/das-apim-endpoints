using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProductSubscription;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.ApiSubscriptions.Queries
{
    public class WhenHandlingGetApiProductSubscriptionQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Product_And_Subscription_Is_Returned_For_The_Account(
            GetApiProductSubscriptionQuery subscriptionQuery,
            GetAvailableApiProductsResponse apiResponse,
            GetApiProductSubscriptionsResponse apiSubscriptionsResponse,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> client,
            GetApiProductSubscriptionQueryHandler handler)
        {
            apiResponse.Products.First().Id = subscriptionQuery.ProductId;
            apiSubscriptionsResponse.Subscriptions.First().Name = subscriptionQuery.ProductId;
            client.Setup(x =>
                    x.Get<GetAvailableApiProductsResponse>(
                        It.Is<GetAvailableApiProductsRequest>(c => c.GetUrl.EndsWith($"?group={subscriptionQuery.AccountType}"))))
                .ReturnsAsync(apiResponse);
            client.Setup(x =>
                    x.Get<GetApiProductSubscriptionsResponse>(
                        It.Is<GetApiProductSubscriptionsRequest>(c => c.GetUrl.EndsWith($"/{subscriptionQuery.AccountIdentifier}"))))
                .ReturnsAsync(apiSubscriptionsResponse);

            var actual = await handler.Handle(subscriptionQuery, CancellationToken.None);
            
            actual.Product.Should().BeEquivalentTo(apiResponse.Products.First());
            actual.Subscription.Should().BeEquivalentTo(apiSubscriptionsResponse.Subscriptions.First());
        }
    }
}