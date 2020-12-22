using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetLegalEntityByHashedIdRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(string hashedLegalEntityId)
        {
            var actual = new GetLegalEntityByHashedIdRequest(hashedLegalEntityId);

            actual.GetUrl.Should().Be($"/legalentities?hashedLegalEntityId={hashedLegalEntityId}");
        }
    }
}
