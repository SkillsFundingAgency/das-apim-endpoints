using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetUkprnsForStandardAndLocation
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(int standardId, double lat, double lon)
        {
            //Arrange Act
            var actual = new GetTotalProvidersForStandardRequest(standardId);
            
            //Assert
            actual.GetUrl.Should().Be($"/api/courses/{standardId}/providers/count");
        }
    }
}