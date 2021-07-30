using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetApprenticeshipIncentivesRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(long accountId, long accountLegalEntityId)
        {
            var actual = new GetApprenticeshipIncentivesRequest(accountId, accountLegalEntityId);

            actual.GetAllUrl.Should().Be($"/accounts/{accountId}/legalEntities/{accountLegalEntityId}/apprenticeshipIncentives");
        }
    }
}