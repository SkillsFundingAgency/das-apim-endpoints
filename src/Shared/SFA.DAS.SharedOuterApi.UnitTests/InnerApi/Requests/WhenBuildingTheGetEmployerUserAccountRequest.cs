using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetEmployerUserAccountRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Encoded_And_Returned(string id)
        {
            id = $"{id} ++$%^{id}";
            
            var actual = new GetEmployerUserAccountRequest(id);

            actual.GetUrl.Should().Be($"api/users/{HttpUtility.UrlEncode(id)}");
        }
    }
}