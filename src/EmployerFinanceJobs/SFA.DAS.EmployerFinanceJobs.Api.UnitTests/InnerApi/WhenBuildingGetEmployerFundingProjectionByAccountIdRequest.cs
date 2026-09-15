using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi;

[TestFixture]
internal class WhenBuildingGetEmployerFundingProjectionByAccountIdRequest
{
    [Test]
    public void Then_The_Url_Is_Correctly_Built()
    {
        // Arrange
        var accountId = 123456;
        var request = new GetEmployerFundingProjectionByAccountIdRequest(accountId);
        // Act
        var url = request.GetUrl;
        // Assert
        Assert.That(url, Is.EqualTo($"api/employer/{accountId}/funding-projection"));
    }
}
