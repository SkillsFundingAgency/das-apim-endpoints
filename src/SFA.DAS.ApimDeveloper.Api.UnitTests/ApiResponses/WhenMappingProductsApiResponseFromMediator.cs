using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.ApiResponses
{
    public class WhenMappingProductsApiResponseFromMediator
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetApiProductsQueryResult source, string name)
        {
            source.Subscriptions.First().Name = name; 
            source.Products.First().Id = name;
            
            var actual = (ProductsApiResponse)source;
            
            actual.Should().BeEquivalentTo(source, options=> options.Excluding(c=>c.Subscriptions));
            actual.Products.FirstOrDefault(c => c.Id.Equals(name))?.Key.Should().Be(source.Subscriptions.First().Key);
        }
    }
}