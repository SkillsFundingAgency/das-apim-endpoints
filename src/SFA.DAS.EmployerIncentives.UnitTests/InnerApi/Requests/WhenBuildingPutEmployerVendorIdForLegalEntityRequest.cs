using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPutEmployerVendorIdForLegalEntityRequest
    {
        [Test, AutoData]
        public void Then_The_Put_Url_Is_Correctly_Built(PutEmployerVendorIdForLegalEntityRequestData data)
        {
            var actual = new PutEmployerVendorIdForLegalEntityRequest { Data = data};

            actual.PutUrl.Should().Be($"legalentities/{data.HashedLegalEntityId}/employervendorid");
        }
    }
}