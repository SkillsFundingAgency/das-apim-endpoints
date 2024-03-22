using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingSearchEducationalOrganisationsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(string searchTerm, int maximumResults)
        {
            var actual = new SearchEducationalOrganisationsRequest(searchTerm, maximumResults);

            var expected = $"/api/EducationalOrganisations/search?searchTerm={HttpUtility.UrlEncode(searchTerm)}&maximumResults={maximumResults}";

            actual.GetUrl.Should().Be(expected);
        }
    }
}
