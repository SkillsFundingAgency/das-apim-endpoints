using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Charities;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetCharityByRegistrationNumberRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(int registrationNumber)
        {
            var actual = new GetCharityByRegistrationNumberRequest(registrationNumber);

            var expected = $"/api/Charities/{registrationNumber}";

            actual.GetUrl.Should().Be(expected);
        }
    }
}
