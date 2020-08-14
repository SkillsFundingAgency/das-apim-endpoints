using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetAccountLegalEntityRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(string baseUrl,string accountId, long legalEntityId, DateTime startDate)
        {
            var actual = new GetAccountLegalEntityRequest(accountId, legalEntityId)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should()
                .Be($"{baseUrl}api/accounts/{accountId}/legalentities/{legalEntityId}");
        }
    }
}