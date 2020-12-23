using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.InnerApi.Requests;

namespace SFA.DAS.ManageApprenticeships.UnitTests.Application.InnerApi.Requests
{
    public class WhenBuildingTheGetProvidersRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built()
        {
            //Arrange
            var actual = new GetProvidersRequest();
            
            //Assert
            actual.GetUrl.Should().Be("api/providers");
        }
    }
}