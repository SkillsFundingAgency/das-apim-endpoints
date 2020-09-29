using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts;
using System;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetAccountLegalEntityRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(string accountId, long legalEntityId, DateTime startDate)
        {
            var actual = new GetLegalEntityRequest(accountId, legalEntityId);

            actual.GetUrl.Should()
                .Be($"api/accounts/{accountId}/legalentities/{legalEntityId}?includeAllAgreements=true");

        }
    }
}