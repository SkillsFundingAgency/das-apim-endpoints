using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi;

[TestFixture]
internal class WhenBuildingGetEmployerFundingProjectionByAccountIdRequest
{
    [Test, MoqAutoData]
    public void Then_The_Url_Is_Correctly_Built(int accountId)
    {
        // Arrange
        var request = new GetEmployerFundingProjectionByAccountIdRequest(accountId);
        // Act
        var url = request.GetUrl;
        // Assert
        url.Should().Be($"api/employer/{accountId}/funding-projection");
    }
}