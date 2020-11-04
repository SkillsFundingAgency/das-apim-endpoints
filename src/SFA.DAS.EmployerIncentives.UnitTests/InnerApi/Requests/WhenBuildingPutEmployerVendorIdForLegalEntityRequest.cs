using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPutEmployerVendorIdForLegalEntityRequest
    {
        [Test, AutoData]
        public void Then_The_Put_Url_Is_Correctly_Built(string hashedAccountLegalEntityId)
        {
            var actual = new PutEmployerVendorIdForLegalEntityRequest(hashedAccountLegalEntityId);

            actual.PutUrl.Should().Be($"legalentities/{hashedAccountLegalEntityId}/employervendorid");
        }
    }
}