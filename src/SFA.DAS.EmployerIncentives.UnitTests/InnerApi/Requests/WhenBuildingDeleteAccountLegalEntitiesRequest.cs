using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingDeleteAccountLegalEntitiesRequest
    {
        [Test, AutoData]
        public void Then_The_DeleteUrl_Is_Correctly_Build(long accountId,long accountLegalEntityId, string baseUrl)
        {
            var actual = new DeleteAccountLegalEntityRequest(accountId, accountLegalEntityId)
            {
                BaseUrl = baseUrl
            };

            actual.DeleteUrl.Should().Be($"/accounts/{accountId}/legalentities/{accountLegalEntityId}");
        }
    }
}