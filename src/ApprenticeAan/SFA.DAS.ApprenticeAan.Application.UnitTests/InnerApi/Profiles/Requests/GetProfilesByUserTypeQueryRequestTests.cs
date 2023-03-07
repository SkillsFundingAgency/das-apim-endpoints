using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Profiles.Requests;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.InnerApi.Profiles.Requests
{
    public class GetProfilesByUserTypeQueryRequestTests
    {
        [Test]
        public void CheckRequestUrl()
        {
            var request = new GetProfilesByUserTypeQueryRequest();
            request.GetUrl.Should().Be("/api/profiles?userType=");
        }
    }
}
