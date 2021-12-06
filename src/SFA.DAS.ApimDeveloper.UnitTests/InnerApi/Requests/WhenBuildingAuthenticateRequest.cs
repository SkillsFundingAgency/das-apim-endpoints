using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;

namespace SFA.DAS.ApimDeveloper.UnitTests.InnerApi.Requests
{
    public class WhenBuildingAuthenticateRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Constructed_And_Encoded(string email, string password)
        {
            email = $@"{email}$%^&*\`'""{email}";
            password = $@"{password}$%^&*\`'""{password}";
            
            var actual = new GetAuthenticateUserRequest(email, password);

            actual.GetUrl.Should().Be($"api/users/authenticate?email={HttpUtility.UrlEncode(email)}&password={HttpUtility.UrlEncode(password)}");
        }
    }
}