using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.FindApprenticeshipApi;

[TestFixture]
public class WhenBuildingGetCandidateSavedSearchesApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(Guid candidateId)
    {
        var actual = new GetCandidateSavedSearchesApiRequest(candidateId);

        actual.GetUrl.Should().Be($"api/Users/{candidateId}/SavedSearches");
    }
}