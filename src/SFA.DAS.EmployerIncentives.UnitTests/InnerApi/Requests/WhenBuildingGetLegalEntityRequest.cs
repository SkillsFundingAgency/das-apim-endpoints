using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetLegalEntityRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(long accountId, long accountLegalEntityId, string baseUrl)
        {
            var actual = new GetLegalEntityRequest(accountId, accountLegalEntityId)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}accounts/{accountId}/legalentities/{accountLegalEntityId}");
        }
    }
}