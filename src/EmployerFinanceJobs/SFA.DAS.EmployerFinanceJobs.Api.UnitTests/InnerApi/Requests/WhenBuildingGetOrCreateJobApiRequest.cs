using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.Domain.Enums;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetOrCreateJobApiRequest
{
    [Test, MoqAutoData]
    public void Build_ReturnsCorrectRequest(JobName jobName)
    {
        // Act
        var request = new GetOrCreateJobApiRequest(jobName);
        // Assert
        request.Should().NotBeNull();
        request.PostUrl.Should().Be($"api/jobs/{jobName}");
        request.Data.Should().BeNull();
    }
}