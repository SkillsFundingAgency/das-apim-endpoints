using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Requests;

namespace SFA.DAS.Campaign.UnitTests.ExternalApi.Requests
{
    public class WhenBuildingGetMenuRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string menuType)
        {
            //Act
            var actual = new GetMenuRequest(menuType);
            
            //Assert
            actual.GetUrl.Should().Be($"entries?content_type=navigationMenu&fields.type={menuType}");
        }
    }
}