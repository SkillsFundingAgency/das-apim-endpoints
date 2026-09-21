using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingPostUpdateJobStateApiRequest
{
    [Test, MoqAutoData]
    public void Build_ReturnsCorrectRequest(Guid jobId, PutImportJobStateRequestData requestData)
    {
        // Act
        var request = new PutUpdateJobStateApiRequest(jobId, requestData);
        // Assert
        request.Should().NotBeNull();
        request.PutUrl.Should().Be($"api/jobs/{jobId}");
        request.Data.Should().BeEquivalentTo(requestData);
    }
}