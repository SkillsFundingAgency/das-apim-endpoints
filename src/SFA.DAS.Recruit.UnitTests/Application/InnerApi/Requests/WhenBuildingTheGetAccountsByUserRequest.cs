using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests
{
    public class WhenBuildingTheGetAccountsByUserRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string userId)
        {
            //Act
            var actual = new GetAccountsByUserRequest(userId);
            
            //Assert
            actual.GetAllUrl.Should().Be($"api/user/{userId}/accounts");
        }
    }
}