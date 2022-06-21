using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Reservations.InnerApi.Requests;

namespace SFA.DAS.Reservations.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetProviderRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(int ukPrn)
        {
            //Arrange Act
            var actual = new GetProviderRequest
            {
                Ukprn = ukPrn
            };
            
            //Assert
            actual.GetUrl.Should().Be($"api/providers/{ukPrn}");
        }
    }
}
