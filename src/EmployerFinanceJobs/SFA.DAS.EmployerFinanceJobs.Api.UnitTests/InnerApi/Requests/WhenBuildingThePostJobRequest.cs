using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingThePostJobRequest
{
    [Test, MoqAutoData]
    public void Then_The_PostUrl_Is_Correctly_Formed(
        long id,
        PostJobRequestData payload)
    {
        // Act
        var request = new PostJobRequest(id, payload);

        // Assert
        request.PostUrl.Should().Be($"api/jobs/{id}");
        request.Data.Should().BeSameAs(payload);
    }
}
