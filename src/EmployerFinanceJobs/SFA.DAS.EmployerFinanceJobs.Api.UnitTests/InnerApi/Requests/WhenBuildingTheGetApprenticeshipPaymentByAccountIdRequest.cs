using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingTheGetApprenticeshipPaymentByAccountIdRequest
{
    [Test, MoqAutoData]
    public void WhenBuildingTheRequest_ThenTheCorrectUrlIsGenerated(long accountId,
        long apprenticeshipId)
    {
        // Act
        var request = new GetApprenticeshipPaymentByAccountIdRequest(accountId, apprenticeshipId);

        // Assert
        request.GetUrl.Should().Be($"api/payments/account/{accountId}/apprenticeship/{apprenticeshipId}");
    }
}