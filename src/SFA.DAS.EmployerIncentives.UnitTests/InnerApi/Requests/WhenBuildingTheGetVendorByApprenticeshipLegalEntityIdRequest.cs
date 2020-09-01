using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetVendorByApprenticeshipLegalEntityIdRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(string baseUrl,string companyName, string hashedLegalEntityId)
        {
            var actual = new GetVendorByApprenticeshipLegalEntityId(companyName, hashedLegalEntityId)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}Finance/{companyName}/vendor/{hashedLegalEntityId}");
        }
    }
}