using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Funding.Application.Queries.GetProviderEarningsSummary;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Funding.UnitTests.Application.Queries.GetProviderEarningsSummary
{
    public class WhenHandlingGetProviderEarningsSummaryQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Application_Is_Returned(
            GetProviderEarningsSummaryQuery query,
            ProviderEarningsSummaryDto summaryResponse,
            [Frozen] Mock<IFundingProviderEarningsService> earningsService,
            GetProviderEarningsSummaryHandler handler
            )
        {
            earningsService.Setup(x => x.GetSummary(query.Ukprn)).ReturnsAsync(summaryResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Summary.Should().BeEquivalentTo(summaryResponse, opts => opts.ExcludingMissingMembers());
        }
    }
}