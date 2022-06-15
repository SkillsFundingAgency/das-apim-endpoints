using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetVendorByApprenticeshipLegalEntityIdRequest
    {
     
        public WhenBuildingTheGetVendorByApprenticeshipLegalEntityIdRequest()
        {
        }

      
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly(string baseUrl, string companyName, string hashedLegalEntityId, string apiVersion)
        {
            var actual = new GetVendorByApprenticeshipLegalEntityId(companyName, hashedLegalEntityId, apiVersion)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}Finance/{companyName}/vendor/aleid={hashedLegalEntityId}?api-version={apiVersion}");
        }
    }
}