using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests
{
    public class WhenBuildingTheGetAccountByIdRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(long accountId)
        {
            //Act
            var actual = new GetAccountByIdRequest(accountId);
            
            //Assert
            actual.GetUrl.Should().Be($"api/accounts/internal/{accountId}");
        }
    }
}