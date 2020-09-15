using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetPingRequest
    {
        [Test]
        public void Then_The_GetUrl_Is_Correctly_Built()
        {
            var actual = new GetPingRequest();

            actual.GetUrl.Should().Be("ping");
        }
    }
}