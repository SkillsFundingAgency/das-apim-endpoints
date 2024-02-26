using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.LegacyApi.Requests
{
    public class WhenBuildingGetLegacyUserByEmailApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            string email)
        {
            var actual = new GetLegacyUserByEmailApiRequest(email);

            actual.GetUrl.Should().Be($"api/user/{email}");
        }
    }
}
