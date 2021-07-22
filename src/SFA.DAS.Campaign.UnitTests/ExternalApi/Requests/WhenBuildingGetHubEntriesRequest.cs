using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Requests;

namespace SFA.DAS.Campaign.UnitTests.ExternalApi.Requests
{
    public class WhenBuildingGetHubEntriesRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string hubType)
        {
            //Act
            var actual = new GetHubEntriesRequest(hubType);
            
            //Assert
            actual.GetUrl.Should().Be($"entries?content_type=hub&include=2&fields.hubType={hubType}");
        }
    }
}