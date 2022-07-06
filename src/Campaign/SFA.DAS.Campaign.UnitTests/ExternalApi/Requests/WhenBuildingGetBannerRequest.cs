using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Requests;

namespace SFA.DAS.Campaign.UnitTests.ExternalApi.Requests
{
    public class WhenBuildingGetBannerRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built()
        {
            //Act
            var actual = new GetBannerRequest();
            
            //Assert
            actual.GetUrl.Should().Be("entries?content_type=banner");
        }
    }
}