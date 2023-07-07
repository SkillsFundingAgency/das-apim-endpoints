using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
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
            apiClient.Setup(x => x.GetEmployerAccounts(query.employerAccontId, cancellationToken))
                .ReturnsAsync(new Response<AccountsSummary?>(string.Empty, new(HttpStatusCode.OK), () => expectedAccountsSummary));

            apiClient.Setup(x => x.GetApprenticeshipsSummaryForEmployer(query.employerAccontId, cancellationToken))
                .ReturnsAsync(new Response<ApprenticeshipsFilterValues?>(string.Empty, new(HttpStatusCode.OK), () => expectedApprenticeshipsFilterValues));

            GetEmployerMemberSummaryQueryResult expectedEmployerAccount =
                new GetEmployerMemberSummaryQueryResult()
                {
                    ActiveCount = expectedAccountsSummary.ApprenticeshipStatusSummaryResponse!.FirstOrDefault()!.ActiveCount,
                    StartDate = expectedApprenticeshipsFilterValues.StartDates!.Min(x => x.Date),
                    Sectors = expectedApprenticeshipsFilterValues.Sectors!
                };

            var actual = await handler.Handle(query, cancellationToken);

            actual.Should().BeEquivalentTo(expectedEmployerAccount);
        }
    }
}
