using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProductSubscriptions;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.ApiSubscriptions.Queries
{
    public class WhenHandlingGetApiProductSubscriptionsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Data_Returned(
            GetApiProductSubscriptionsQuery query, 
            GetAvailableApiProductsResponse serviceResponse,
            GetApiProductSubscriptionsResponse apiSubscriptionsResponse,
            [Frozen] Mock<IApimApiService> apimApiService,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> apiClient,
            GetApiProductSubscriptionsQueryHandler handler)
        {
            apimApiService.Setup(x =>
                x.GetAvailableProducts(query.AccountType)).ReturnsAsync(serviceResponse);
            apiClient.Setup(x =>
                    x.Get<GetApiProductSubscriptionsResponse>(
                        It.Is<GetApiProductSubscriptionsRequest>(c => c.GetUrl.EndsWith($"/{query.AccountIdentifier}"))))
                .ReturnsAsync(apiSubscriptionsResponse);

            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.Products.Should().BeEquivalentTo(serviceResponse.Products);
            actual.Subscriptions.Should().BeEquivalentTo(apiSubscriptionsResponse.Subscriptions);
        }
    }
}