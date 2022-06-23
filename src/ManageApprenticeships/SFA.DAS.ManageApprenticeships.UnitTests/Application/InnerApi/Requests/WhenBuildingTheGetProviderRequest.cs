using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.InnerApi.Requests;

namespace SFA.DAS.ManageApprenticeships.UnitTests.Application.InnerApi.Requests
{
    public class WhenBuildingTheGetProviderRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Build(int id)
        {
            //Arrange
            var actual = new GetProviderRequest(id);
            
            //Assert
            actual.GetUrl.Should().Be($"api/providers/{id}");
        }
    }
}