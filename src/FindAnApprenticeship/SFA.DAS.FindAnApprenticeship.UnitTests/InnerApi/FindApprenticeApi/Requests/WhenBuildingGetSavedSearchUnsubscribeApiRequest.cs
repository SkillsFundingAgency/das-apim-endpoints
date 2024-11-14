using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.FindApprenticeApi.Requests;

public class WhenBuildingGetSavedSearchUnsubscribeApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Built(Guid searchId)
    {
        var actual = new GetSavedSearchUnsubscribeApiRequest(searchId);
        
        actual.GetUrl.Should().Be($"api/savedsearches/{searchId}");
    }
}