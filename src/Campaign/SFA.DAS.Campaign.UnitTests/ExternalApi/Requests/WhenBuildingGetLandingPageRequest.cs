using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Requests;

namespace SFA.DAS.Campaign.UnitTests.ExternalApi.Requests
{
    public class WhenBuildingGetLandingPageRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string hubType, string slug)
        {
            //Act
            var actual = new GetLandingPageRequest(hubType, slug);
            
            //Assert
            actual.GetUrl.Should().Be($"entries?content_type=landingPage&include=2&fields.hubType={hubType}&fields.slug={slug}");
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_For_An_Entry(string entryId)
        {
            //Act
            var actual = new GetLandingPageRequest(entryId);
            
            //Assert
            actual.GetUrl.Should().Be($"entries?content_type=landingPage&include=2&sys.id={entryId}");
        }
    }
}