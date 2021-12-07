using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Application.ApiProducts.Queries.GetApiProduct;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.ApiResponses
{
    public class WhenMappingProductApiResponseItemFromMediator
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetApiProductQueryResult source)
        {
            var actual = (ProductApiResponseItem)source.Product;
            
            actual.Should().BeEquivalentTo(source.Product);
        }

        [Test]
        public void Then_If_Null_Then_Null_Returned()
        {
            var source = new GetApiProductQueryResult
            {
                Product = null
            };
            
            var actual = (ProductApiResponseItem)source.Product;
            
            actual.Should().BeNull();
        }
    }
}