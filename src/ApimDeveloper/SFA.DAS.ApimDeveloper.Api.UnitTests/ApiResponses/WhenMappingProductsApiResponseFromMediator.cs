using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProductSubscriptions;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.ApiResponses
{
    public class WhenMappingProductsApiResponseFromMediator
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetApiProductSubscriptionsQueryResult source, string name)
        {
            source.Subscriptions.First().Name = name; 
            source.Products.First().Id = name;
            
            var actual = (ProductSubscriptionsApiResponse)source;
            
            actual.Should().BeEquivalentTo(source, options=> options.Excluding(c=>c.Subscriptions).ExcludingMissingMembers());
            actual.Products.FirstOrDefault(c => c.Id.Equals(name))?.Key.Should().Be(source.Subscriptions.First().Key);
        }
    }
}