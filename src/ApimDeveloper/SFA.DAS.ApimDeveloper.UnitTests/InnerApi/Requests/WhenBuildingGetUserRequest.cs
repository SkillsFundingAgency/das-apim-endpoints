using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;

namespace SFA.DAS.ApimDeveloper.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetUserRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string email)
        {
            email = $"{email}!@£$%£$%+15";
            
            var actual = new GetUserRequest(email);

            actual.GetUrl.Should().Be($"api/users?email={HttpUtility.UrlEncode(email)}");
        }
    }
}