using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.ApiProducts.Queries.GetApiProduct;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.ApiProducts.Queries
{
    public class WhenHandlingGetApiProductQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Data_Returned(
            GetApiProductQuery query, 
            GetAvailableApiProductsResponse apiResponse,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> apiClient,
            GetApiProductQueryHandler handler)
        {
            apiResponse.Products.First().Name = query.ProductName.ToLower();
            apiClient.Setup(x =>
                    x.Get<GetAvailableApiProductsResponse>(
                        It.Is<GetAvailableApiProductsRequest>(c => c.GetUrl.EndsWith("?group=Documentation"))))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.Product.Should().BeEquivalentTo(apiResponse.Products.First());
        }
    }
}