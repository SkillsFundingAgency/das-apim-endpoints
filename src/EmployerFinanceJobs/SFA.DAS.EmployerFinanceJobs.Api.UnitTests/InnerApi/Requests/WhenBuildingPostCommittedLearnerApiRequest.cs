using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingPostCommittedLearnerApiRequest
{
    [Test, MoqAutoData]
    public void Build_ReturnsCorrectRequest(long accountId,
        PostCommittedLearnerApiRequestData requestData)
    {
        // Act
        var request = new PostCommittedLearnerApiRequest(accountId, requestData);

        // Assert
        request.Should().NotBeNull();
        request.PostUrl.Should().Be($"api/employer/{accountId}/learners");
        request.Data.Should().BeEquivalentTo(requestData);
    }
}