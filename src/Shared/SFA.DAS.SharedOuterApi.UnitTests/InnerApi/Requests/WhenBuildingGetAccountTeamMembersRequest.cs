using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetAccountTeamMembersRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_For_HashedAccountId(string accountId)
        {
            var actual = new GetAccountTeamMembersRequest(accountId);

            actual.GetAllUrl.Should().Be($"api/accounts/{accountId}/users");
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_For_AccountId(long accountId)
        {
            var actual = new GetAccountTeamMembersRequest(accountId);

            actual.GetAllUrl.Should().Be($"api/accounts/internal/{accountId}/users");
        }
    }
}