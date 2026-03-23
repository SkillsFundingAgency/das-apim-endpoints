using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests;

public class PostProviderEmailRequestTests
{
    [Test, AutoData]
    public void GetUrl_ReturnsExpectedUrl()
    {
        int ukprn = 10012002;
        var request = new PostProviderEmailRequest(ukprn, null);

        request.PostUrl.Should().Be("api/email/10012002/send");
    }
}
