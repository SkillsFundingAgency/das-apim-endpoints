using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests.Roatp;
public class GetOrganisationStatusHistoryRequestTests
{
    [Test]
    public void GetOrganisationStatusHistoryRequest_ShouldConstructCorrectUrl()
    {
        // Arrange
        var ukprn = 12345678;
        var request = new GetOrganisationStatusHistoryRequest(ukprn);
        // Act
        var url = request.GetUrl;
        // Assert
        Assert.That($"organisations/{ukprn}/status-history", Is.EqualTo(url));
    }
}
