using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Employer.Queries.GetEmployerMemberSummary
{
    public class GetEmployerMemberSummaryQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_ReturnEmployerMemberSummary(
            [Frozen] Mock<ICommitmentsV2ApiClient> apiClient,
            GetEmployerMemberSummaryQueryHandler handler,
            GetEmployerMemberSummaryQuery query,
            AccountsSummary expectedAccountsSummary,
            ApprenticeshipsFilterValues expectedApprenticeshipsFilterValues,
            CancellationToken cancellationToken)
        {
            apiClient.Setup(x => x.GetEmployerAccountSummary(query.EmployerAccountId, cancellationToken))
                .ReturnsAsync(expectedAccountsSummary);

            apiClient.Setup(x => x.GetApprenticeshipsSummaryForEmployer(query.EmployerAccountId, cancellationToken))
                .ReturnsAsync(expectedApprenticeshipsFilterValues);

            GetEmployerMemberSummaryQueryResult expectedEmployerAccount =
                new GetEmployerMemberSummaryQueryResult()
                {
                    ActiveCount = expectedAccountsSummary.ApprenticeshipStatusSummaryResponse!.FirstOrDefault()!.ActiveCount,
                    Sectors = expectedApprenticeshipsFilterValues.Sectors!
                };

            var actual = await handler.Handle(query, cancellationToken);

            actual.Should().BeEquivalentTo(expectedEmployerAccount);
        }

        [Test, MoqAutoData]
        public async Task Handle_ReturnEmployerMemberSummaryWhenNoAccoutSummaryAndNoApprenticeshipsFilterValues(
            [Frozen] Mock<ICommitmentsV2ApiClient> apiClient,
            GetEmployerMemberSummaryQueryHandler handler,
            GetEmployerMemberSummaryQuery query,
            CancellationToken cancellationToken)
        {
            AccountsSummary expectedAccountsSummary = new();

            apiClient.Setup(x => x.GetEmployerAccountSummary(query.EmployerAccountId, cancellationToken))
                .ReturnsAsync(expectedAccountsSummary);

            ApprenticeshipsFilterValues expectedApprenticeshipsFilterValues = new();

            apiClient.Setup(x => x.GetApprenticeshipsSummaryForEmployer(query.EmployerAccountId, cancellationToken))
                .ReturnsAsync(expectedApprenticeshipsFilterValues);

            GetEmployerMemberSummaryQueryResult expectedEmployerAccount =
                new GetEmployerMemberSummaryQueryResult()
                {
                    ActiveCount = 0,
                    Sectors = Enumerable.Empty<string>()
                };

            var actual = await handler.Handle(query, cancellationToken);

            actual.Should().BeEquivalentTo(expectedEmployerAccount);
        }
    }
}
