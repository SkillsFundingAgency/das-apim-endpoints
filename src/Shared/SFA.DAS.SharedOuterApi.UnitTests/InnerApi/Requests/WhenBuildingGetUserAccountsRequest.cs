using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetUserAccountsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string userId)
        {
            var actual = new GetUserAccountsRequest(userId);

            actual.GetAllUrl.Should().Be($"api/user/{userId}/accounts");
        }
    }
}