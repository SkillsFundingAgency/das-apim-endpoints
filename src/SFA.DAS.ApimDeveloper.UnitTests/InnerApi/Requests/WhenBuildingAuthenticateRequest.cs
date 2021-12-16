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
            var actual = new PostAuthenticateUserRequest(email, password);

            actual.PostUrl.Should().Be("api/users/authenticate");
            ((PostAuthenticateUserRequestData)actual.Data).Should().BeEquivalentTo(new PostAuthenticateUserRequestData
            {
                Email = email,
                Password = password
            });
        }
    }
}