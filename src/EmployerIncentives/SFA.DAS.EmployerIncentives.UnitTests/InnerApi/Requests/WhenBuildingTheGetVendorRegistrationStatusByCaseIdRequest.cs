using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetVendorRegistrationStatusByCaseIdRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(string baseUrl, string caseId)
        {
            var actual = new GetVendorRegistrationStatusByCaseIdRequest(caseId)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}Finance/Registrations/{caseId}?api-version=2019-06-01");
        }
    }
}