using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.ApiProducts.Queries.GetApiProduct;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.ApiProducts.Queries
{
    public class WhenHandlingGetApiProductQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_Data_Returned(
            GetApiProductQuery query, 
            GetAvailableApiProductsResponse serviceResponse,
            [Frozen] Mock<IApimApiService> apimApiService,
            GetApiProductQueryHandler handler)
        {
            //Arrange
            serviceResponse.Products.First().Name = query.ProductName.ToLower();
            apimApiService.Setup(x => x.GetAvailableProducts("Documentation")).ReturnsAsync(serviceResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.Product.Should().BeEquivalentTo(serviceResponse.Products.First());
        }
    }
}