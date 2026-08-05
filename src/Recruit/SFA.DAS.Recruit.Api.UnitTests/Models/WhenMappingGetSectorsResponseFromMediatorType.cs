using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.UnitTests.Models
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
            actual.Id.Should().Be(source.Id);
        }
    }
}