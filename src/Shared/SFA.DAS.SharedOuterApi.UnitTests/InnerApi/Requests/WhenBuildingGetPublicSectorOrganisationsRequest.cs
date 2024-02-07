using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetPublicSectorOrganisationsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(string searchTerm, int pageSize, int pageNumber)
        {
            var actual = new GetPublicSectorOrganisationsRequest(searchTerm, pageSize, pageNumber);

            var expected = $"publicsectorbodies?searchTerm={HttpUtility.UrlEncode(searchTerm)}&pageSize={pageSize}&pageNumber={pageNumber}";

            actual.GetPagedUrl.Should().Be(expected);
        }
    }
}
