using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetApprenticeshipsApiRequest
{
    [Test, MoqAutoData]
    public void Build_ReturnsCorrectRequest(long apprenticeshipId)
    {
        // Act
        var request = new GetApprenticeshipsApiRequest(apprenticeshipId);
        // Assert
        request.Should().NotBeNull();
        request.GetUrl.Should().Be($"api/apprenticeships/{apprenticeshipId}");
    }
}