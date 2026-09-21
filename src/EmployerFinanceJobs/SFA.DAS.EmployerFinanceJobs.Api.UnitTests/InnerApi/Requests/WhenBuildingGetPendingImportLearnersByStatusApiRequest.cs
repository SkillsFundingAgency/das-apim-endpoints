using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetPendingImportLearnersByStatusApiRequest
{
    [Test, MoqAutoData]
    public void Build_ReturnsCorrectRequest(string status)
    {
        // Act
        var request = new GetPendingImportLearnersByStatusApiRequest(status);
        // Assert
        request.Should().NotBeNull();
        request.GetUrl.Should().Be($"api/employer/learners/by/status?importStatus={status}");
    }
}