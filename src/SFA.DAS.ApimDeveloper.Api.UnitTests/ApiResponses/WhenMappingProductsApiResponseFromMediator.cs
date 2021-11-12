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
        public void Then_The_Fields_Are_Mapped(GetApiProductsQueryResult source)
        {
            var actual = (ProductsApiResponse)source;
            
            actual.Should().BeEquivalentTo(source);
        }
    }
}