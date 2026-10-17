using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingThePutJobRequest
{
    [Test, MoqAutoData]
    public void Then_The_PutUrl_Is_Correctly_Formed(
        Guid id,
        PutJobRequestData payload)
    {
        // Act
        var request = new PutJobRequest(id, payload);

        // Assert
        request.PutUrl.Should().Be($"api/jobs/{id}");
        request.Data.Should().BeSameAs(payload);
    }
}
