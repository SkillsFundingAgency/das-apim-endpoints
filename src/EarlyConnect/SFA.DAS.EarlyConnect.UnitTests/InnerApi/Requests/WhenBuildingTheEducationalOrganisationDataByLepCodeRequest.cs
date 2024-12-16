using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using System.Web;

namespace SFA.DAS.EarlyConnect.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheEducationalOrganisationDataByLepCodeRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string lepCode, string? searchTerm,int page,int pageSize)
        {
            var actual = new GetEducationalOrganisationsByLepCodeRequest(lepCode, searchTerm, page, pageSize);

            actual.GetUrl.Should().Be($"api/educational-organisations-data/?LepCode={HttpUtility.UrlEncode(lepCode)}&SearchTerm={HttpUtility.UrlEncode(searchTerm)}&Page={page}&PageSize={pageSize}");
        }
    }
}