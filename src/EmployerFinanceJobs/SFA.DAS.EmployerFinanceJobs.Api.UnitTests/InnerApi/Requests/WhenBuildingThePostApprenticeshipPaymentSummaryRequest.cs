using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingThePostApprenticeshipPaymentSummaryRequest
{
    [Test, MoqAutoData]
    public void WhenBuildingTheRequest_ThenTheCorrectUrlIsGenerated(
        long accountId,
        long apprenticeshipId,
        PostApprenticeshipPaymentSummaryRequestData payload)
    {
        // Act
        var request = new PostApprenticeshipPaymentSummaryRequest(accountId, apprenticeshipId, payload);

        // Assert
        request.PostUrl.Should().Be($"api/employer/{accountId}/apprenticeship-payment-summary/{apprenticeshipId}");
    }

    [Test, MoqAutoData]
    public void WhenBuildingTheRequest_ThenThePayloadIsUsedAsData(
        long accountId,
        long apprenticeshipId,
        PostApprenticeshipPaymentSummaryRequestData payload)
    {
        // Act
        var request = new PostApprenticeshipPaymentSummaryRequest(accountId, apprenticeshipId, payload);

        // Assert
        request.Data.Should().BeSameAs(payload);
    }
}
