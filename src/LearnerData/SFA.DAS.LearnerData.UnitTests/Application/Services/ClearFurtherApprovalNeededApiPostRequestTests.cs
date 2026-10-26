using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Requests.LearningInner;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class ClearFurtherApprovalNeededApiPostRequestTests
{
    [TestCase(LearningType.Apprenticeship)]
    [TestCase(LearningType.FoundationApprenticeship)]
    [TestCase(LearningType.ApprenticeshipUnit)]
    public void PostUrl_Should_Address_The_Episode_And_Carry_The_LearningType(LearningType learningType)
    {
        // Arrange
        var learningKey = Guid.NewGuid();
        var episodeKey = Guid.NewGuid();

        // Act
        var sut = new ClearFurtherApprovalNeededApiPostRequest(learningKey, episodeKey, learningType);

        // Assert
        sut.PostUrl.Should().Be($"learning/{learningKey}/episodes/{episodeKey}/clear-further-approval-needed?learningType={learningType}");
    }

    [Test]
    public void Data_Should_Be_Null_As_The_Request_Has_No_Body()
    {
        var sut = new ClearFurtherApprovalNeededApiPostRequest(Guid.NewGuid(), Guid.NewGuid(), LearningType.Apprenticeship);

        sut.Data.Should().BeNull();
    }
}
