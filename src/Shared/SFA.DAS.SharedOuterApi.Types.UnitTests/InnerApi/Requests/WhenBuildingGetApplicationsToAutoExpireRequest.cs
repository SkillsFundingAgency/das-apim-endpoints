using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetApplicationsToAutoExpireRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed()
    {
        var actual = new GetApplicationsToAutoExpireRequest();

        actual.GetUrl.Should().Be("/applications-auto-expire");
    }
}
