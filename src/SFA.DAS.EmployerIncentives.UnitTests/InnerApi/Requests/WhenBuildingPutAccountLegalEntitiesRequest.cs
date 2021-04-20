using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPutAccountLegalEntitiesRequest
    {
        [Test, AutoData]
        public void Then_The_PutUrl_Is_Correctly_Built(long accountId, AccountLegalEntityCreateRequest data)
        {
            var actual = new PutAccountLegalEntityRequest(accountId){Data = data};

            actual.PutUrl.Should().Be($"accounts/{accountId}/legalentities");
            actual.Data.Should().BeEquivalentTo(data);
        }
    }
}