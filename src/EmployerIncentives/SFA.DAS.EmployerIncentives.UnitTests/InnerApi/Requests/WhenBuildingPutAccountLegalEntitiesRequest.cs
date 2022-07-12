using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPutAccountLegalEntitiesRequest
    {
        [Test, AutoData]
        public void Then_The_PutUrl_Is_Correctly_Built(AccountLegalEntityCreateRequest data)
        {
            var actual = new PutAccountLegalEntityRequest{Data = data};

            actual.PutUrl.Should().Be($"accounts/{data.AccountId}/legalentities");
            actual.Data.Should().BeEquivalentTo(data);
        }
    }
}