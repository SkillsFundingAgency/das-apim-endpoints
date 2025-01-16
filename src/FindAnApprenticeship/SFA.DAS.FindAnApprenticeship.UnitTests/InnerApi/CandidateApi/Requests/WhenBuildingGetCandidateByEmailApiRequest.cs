using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;

public class WhenBuildingGetCandidateByEmailApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Built_Correctly(string emailValue)
    {
        var email = $"{emailValue}@£$^£@!@{emailValue}.com";

        var actual = new GetCandidateByEmailApiRequest(email);

        actual.GetUrl.Should().Be($"api/candidates/email/{HttpUtility.UrlEncode(email)}");
    }
}