using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Profiles.Requests;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.InnerApi.Profiles.Requests
{
    public class GetProfilesByUserTypeQueryRequestTests
    {
        [Test]
        public void CheckRequestUrl()
        {
            const string userType = "apprentice";
            var request = new GetProfilesByUserTypeQueryRequest(userType);
            var expectedURL = Constants.AanHubApiUrls.GetProfilesUrl + userType;
            request.GetUrl.Should().Be(expectedURL);
        }
    }
}