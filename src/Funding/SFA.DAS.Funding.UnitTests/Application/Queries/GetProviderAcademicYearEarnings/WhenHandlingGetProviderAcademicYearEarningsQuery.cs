using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Funding.Application.Queries.GetProviderAcademicYearEarnings;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Funding.UnitTests.Application.Queries.GetProviderAcademicYearEarnings
{
    public class WhenHandlingGetProviderAcademicYearEarningsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Earnings_Are_Returned(
            GetProviderAcademicYearEarningsQuery query,
            AcademicYearEarningsDto earningsResponse,
            [Frozen] Mock<IFundingProviderEarningsService> earningsService,
            GetProviderAcademicYearEarningsHandler handler
            )
        {
            earningsService.Setup(x => x.GetAcademicYearEarnings(query.Ukprn)).ReturnsAsync(earningsResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.AcademicYearEarnings.Should().BeEquivalentTo(earningsResponse, opts => opts.ExcludingMissingMembers());
        }
    }
}