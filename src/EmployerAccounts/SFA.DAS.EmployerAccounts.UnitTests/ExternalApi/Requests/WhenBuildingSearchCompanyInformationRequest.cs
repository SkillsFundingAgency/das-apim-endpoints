using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.ExternalApi.Requests;

namespace SFA.DAS.EmployerAccounts.UnitTests.ExternalApi.Requests
{
    public class WhenBuildingSearchCompanyInformationRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(string searchTerm, int maximumResults)
        {
            var actual = new SearchCompanyInformationRequest(searchTerm, maximumResults);

            var expected = $"search/companies?q={HttpUtility.UrlEncode(searchTerm.ToUpper())}&items_per_page={maximumResults}";

            actual.GetUrl.Should().Be(expected);
        }
    }
}
