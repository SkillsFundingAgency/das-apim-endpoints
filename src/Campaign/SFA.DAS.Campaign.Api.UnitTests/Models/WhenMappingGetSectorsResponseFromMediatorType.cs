using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Campaign.Api.UnitTests.Models
{
    public class WhenMappingGetSectorsResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetRoutesListItem source)
        {
            //Act
            var actual = (GetRouteResponseItem) source;
            
            //Assert
            actual.Route.Should().Be(source.Name);
        }
    }
}