using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;

namespace SFA.DAS.ApimDeveloper.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetAccountTeamMembersRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string accountId)
        {
            var actual = new GetAccountTeamMembersRequest(accountId);

            actual.GetAllUrl.Should().Be($"api/accounts/{accountId}/users");
        }
    }
}