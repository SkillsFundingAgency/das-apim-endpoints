using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;

namespace SFA.DAS.Aodp.UnitTests.Request;

[TestFixture]
public class GetMatchingQualificationsQueryApiRequestTests
{
    private const string Endpoint = "api/qualifications/GetMatchingQualifications?SearchTerm";

    [Test]
    public void GetUrl_IncludesSkipAndTake_WhenProvided()
    {
        // Arrange
        var request = new GetMatchingQualificationsQueryApiRequest("term", 0, 10);

        // Act
        var url = request.GetUrl;

        // Assert
        Assert.That(url, Is.EqualTo($"{Endpoint}=term&Skip=0&Take=10"));
    }

    [Test]
    public void GetUrl_OmitsSkip_WhenSkipIsNull()
    {
        // Arrange
        var request = new GetMatchingQualificationsQueryApiRequest("term", null, 10);

        // Act
        var url = request.GetUrl;

        // Assert
        Assert.That(url, Is.EqualTo($"{Endpoint}=term&Take=10"));
    }

    [Test]
    public void GetUrl_OmitsTake_WhenTakeIsNull()
    {
        // Arrange
        var request = new GetMatchingQualificationsQueryApiRequest("term", 5, null);

        // Act
        var url = request.GetUrl;

        // Assert
        Assert.That(url, Is.EqualTo($"{Endpoint}=term&Skip=5"));
    }

    [Test]
    public void GetUrl_OnlySearchTerm_WhenSkipAndTakeAreNull_AndSearchTermCanBeNull()
    {
        // Arrange
        var request = new GetMatchingQualificationsQueryApiRequest(null, null, null);

        // Act
        var url = request.GetUrl;

        // Assert
        Assert.That(url, Is.EqualTo($"{Endpoint}="));
    }
}