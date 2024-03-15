using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetLatestDetailsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(string searchTerm, OrganisationType organisationType)
        {
            var actual = new GetLatestDetailsRequest(searchTerm, organisationType);

            var expected = $"api/organisation/get?identifier={HttpUtility.UrlEncode(searchTerm)}&organisationType={organisationType}";

            actual.GetUrl.Should().Be(expected);
        }
    }
}
