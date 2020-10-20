using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPatchVendorRegistrationFormRequest
    {
        [Test, AutoData]
        public void Then_The_Patch_Url_Is_Correctly_Built(long legalEntityId, string baseUrl, UpdateVendorRegistrationFormRequest data)
        {
            var actual = new PatchVendorRegistrationFormRequest(legalEntityId) { BaseUrl = baseUrl, Data = data };

            actual.PatchUrl.Should().Be($"{baseUrl}legalentities/{legalEntityId}/vendorregistrationform");
            actual.Data.Should().BeEquivalentTo(data);
        }
    }
}