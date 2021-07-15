using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Requests;

namespace SFA.DAS.Campaign.UnitTests.ExternalApi.Requests
{
    public class WhenBuildingGetSiteMapRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string contentType)
        {
            //Act
            var actual = new GetSiteMapRequest(contentType);
            
            //Assert
            actual.GetUrl.Should().Be($"entries?select=sys.id,sys.contentType,fields.slug,fields.title,fields.hubType&content_type={contentType}");
        }
    }
}