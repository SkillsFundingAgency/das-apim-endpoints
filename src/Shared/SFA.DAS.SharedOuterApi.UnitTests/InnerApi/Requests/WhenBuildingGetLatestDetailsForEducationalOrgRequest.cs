using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetLatestDetailsForEducationalOrgRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(string identifier)
        {
            var actual = new GetLatestDetailsForEducationalOrgRequest(identifier);

            var expected = $"/api/EducationalOrganisations/LatestDetails?identifier={HttpUtility.UrlEncode(identifier)}";

            actual.GetUrl.Should().Be(expected);
        }
    }
}
