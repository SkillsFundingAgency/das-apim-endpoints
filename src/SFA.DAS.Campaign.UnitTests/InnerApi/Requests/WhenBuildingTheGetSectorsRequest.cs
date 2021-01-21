using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.InnerApi.Requests;

namespace SFA.DAS.Campaign.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetSectorsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Created()
        {
            //Arrange
            var actual = new GetSectorsListRequest();
            
            //Assert
            actual.GetUrl.Should().Be("api/courses/sectors");
        }
    }
}