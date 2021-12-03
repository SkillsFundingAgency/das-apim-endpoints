using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProducts;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.ApiSubscriptions.Queries
{
    public class WhenHandlingGetApiProductsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Data_Returned(
            GetApiProductsQuery query, 
            GetAvailableApiProductsResponse apiResponse,
            GetApiProductSubscriptionsResponse apiSubscriptionsResponse,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> apiClient,
            GetApiProductsQueryHandler handler)
        {
            apiClient.Setup(x =>
                x.Get<GetAvailableApiProductsResponse>(
                    It.Is<GetAvailableApiProductsRequest>(c => c.GetUrl.EndsWith($"?group={query.AccountType}"))))
                .ReturnsAsync(apiResponse);
            apiClient.Setup(x =>
                    x.Get<GetApiProductSubscriptionsResponse>(
                        It.Is<GetApiProductSubscriptionsRequest>(c => c.GetUrl.EndsWith($"/{query.AccountIdentifier}"))))
                .ReturnsAsync(apiSubscriptionsResponse);

            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.Products.Should().BeEquivalentTo(apiResponse.Products);
            actual.Subscriptions.Should().BeEquivalentTo(apiSubscriptionsResponse.Subscriptions);
        }
    }
}