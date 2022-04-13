using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests
{
    public class WhenBuildingTheGetAccountLegalEntitiesRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string accountId)
        {
            //Act
            var actual = new GetAccountLegalEntitiesRequest(accountId);
            
            //Assert
            actual.GetAllUrl.Should().Be($"api/accounts/{accountId}/legalentities?includeDetails=true");
        }
    }
}