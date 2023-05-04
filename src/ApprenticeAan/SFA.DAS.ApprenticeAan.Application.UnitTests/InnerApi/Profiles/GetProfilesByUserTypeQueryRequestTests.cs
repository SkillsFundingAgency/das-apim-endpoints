using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Profiles;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.InnerApi.Profiles;

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