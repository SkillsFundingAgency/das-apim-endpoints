using SFA.DAS.RecruitQa.Domain.Enums;
using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

[TestFixture]
internal class WhenBuildingGetBannedPhrasesApiRequest
{
    [Test]
    public void Then_The_Request_Is_Correctly_Built()
    {
        // Arrange
        var expectedUrl = $"api/prohibitedcontent/{ProhibitedContentType.BannedPhrases}";
        // Act
        var actual = new GetBannedPhrasesListApiRequest();
        // Assert
        actual.GetUrl.Should().Be(expectedUrl);
    }
}