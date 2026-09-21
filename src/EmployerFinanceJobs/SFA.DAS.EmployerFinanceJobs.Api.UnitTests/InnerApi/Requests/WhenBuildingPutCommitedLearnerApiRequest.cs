using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingPutCommitedLearnerApiRequest
{
    [Test, MoqAutoData]
    public void Build_ReturnsCorrectRequest(Guid id, long accountId, PutCommittedLearnerApiRequestData requestData)
    {
        // Act
        var request = new PutCommittedLearnerApiRequest(accountId, id, requestData);
        // Assert
        request.Should().NotBeNull();
        request.PutUrl.Should().Be($"api/employer/{accountId}/learners/{id}");
        request.Data.Should().BeEquivalentTo(requestData);
    }
}
