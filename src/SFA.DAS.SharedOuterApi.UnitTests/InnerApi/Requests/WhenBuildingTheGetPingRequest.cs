using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetPingRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(string baseUrl)
        {
            var actual = new GetPingRequest
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}ping");
        }
    }
}
