using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetCharityRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(int registrationNumber)
        {
            var actual = new GetCharityRequest(registrationNumber);

            var expected = $"charities/{registrationNumber}";

            actual.GetUrl.Should().Be(expected);
        }
    }
}
