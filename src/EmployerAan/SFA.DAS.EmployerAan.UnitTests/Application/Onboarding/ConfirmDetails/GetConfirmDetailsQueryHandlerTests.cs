using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.EmployerAan.Application.Onboarding.ConfirmDetails.Queries;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Onboarding.ConfirmDetails;

public class GetOnboardingConfirmDetailsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnsExpectedResult(
        GetOnboardingConfirmDetailsQuery query,
        AccountsSummary accountsSummaryResponse,
        ApprenticeshipsFilterValues filterValuesResponse,
        [Frozen] Mock<ICommitmentsV2ApiClient> commitmentsV2ApiClient,
        GetOnboardingConfirmDetailsQueryHandler handler)
    {
        commitmentsV2ApiClient
            .Setup(client => client.GetEmployerAccountSummary(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountsSummaryResponse);

        commitmentsV2ApiClient
            .Setup(client => client.GetApprenticeshipsSummaryForEmployer(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(filterValuesResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.NumberOfActiveApprentices.Should().Be(accountsSummaryResponse.ApprenticeshipStatusSummaryResponse.Sum(a => a.ActiveCount));
        result.Sectors.Should().BeEquivalentTo(filterValuesResponse.Sectors);
    }
}