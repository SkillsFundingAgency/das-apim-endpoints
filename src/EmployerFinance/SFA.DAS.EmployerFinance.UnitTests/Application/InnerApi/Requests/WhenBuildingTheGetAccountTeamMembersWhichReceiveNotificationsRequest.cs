using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests
{
    public class WhenBuildingTheGetAccountTeamMembersWhichReceiveNotificationsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Build(int accountId)
        {
            //Arrange
            var actual = new GetAccountTeamMembersWhichReceiveNotificationsRequest(accountId);
            
            //Assert
            actual.GetUrl.Should().Be($"api/accounts/internal/{accountId}/users/which-receive-notifications");
        }
    }
}