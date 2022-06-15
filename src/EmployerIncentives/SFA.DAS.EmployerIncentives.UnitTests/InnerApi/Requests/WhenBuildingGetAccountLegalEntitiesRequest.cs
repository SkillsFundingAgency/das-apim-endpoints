using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetAccountLegalEntitiesRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(long accountId)
        {
            var actual = new GetAccountLegalEntitiesRequest(accountId);

            actual.GetAllUrl.Should().Be($"accounts/{accountId}/legalentities");
        }
    }
}