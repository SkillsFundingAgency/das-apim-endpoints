using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.FindApprenticeshipApi;

[TestFixture]
public class WhenBuildingPutSavedSearchApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(Guid candidateId, Guid id, PutSavedSearchApiRequestData payload)
    {
        var actual = new PutSavedSearchApiRequest(candidateId, id, payload);

        actual.PutUrl.Should().Be($"api/Users/{candidateId}/SavedSearches/{id}");
    }
}