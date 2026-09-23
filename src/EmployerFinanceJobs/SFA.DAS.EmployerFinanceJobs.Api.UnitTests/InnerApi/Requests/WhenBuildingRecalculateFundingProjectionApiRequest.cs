using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingRecalculateFundingProjectionApiRequest
{
    [Test, MoqAutoData]
    public void Then_The_Url_Is_Correct(DateTime cutOffDateTime)
    {
        // Arrange
        var request = new RecalculateFundingProjectionApiRequest(cutOffDateTime);
        // Act
        var url = request.PostUrl;
        // Assert
        url.Should().Be("api/employer/funding-projection/re-calculate");
        request.Data.Should().Be(cutOffDateTime.ToString("O"));
    }
}