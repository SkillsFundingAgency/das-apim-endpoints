using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.ExternalApi.Requests;

namespace SFA.DAS.EmployerAccounts.UnitTests.ExternalApi.Requests
{
    public class WhenBuildingGetCompanyInformationRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(string id)
        {
            var actual = new GetCompanyInformationRequest(id);

            var expected = $"company/{id.ToUpper()}";

            actual.GetUrl.Should().Be(expected);
        }
    }
}
