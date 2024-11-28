using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.FindApprenticeApi.Requests;

public class WhenBuildingDeleteSavedSearchRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Built(Guid id)
    {
        var actual = new DeleteSavedSearchRequest(id);
        
        actual.DeleteUrl.Should().Be($"api/SavedSearches/{id}");
    }
}