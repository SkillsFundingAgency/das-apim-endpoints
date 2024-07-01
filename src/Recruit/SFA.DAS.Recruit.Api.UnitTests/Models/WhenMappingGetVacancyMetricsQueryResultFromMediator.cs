using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetVacancyMetrics;

namespace SFA.DAS.Recruit.Api.UnitTests.Models
{
    public class WhenMappingGetVacancyMetricsQueryResultFromMediator
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetVacancyMetricsQueryResult source)
        {
            var actual = (GetVacancyMetricsResponse)source;

            actual.SearchResultsCount.Should().Be(source.SearchResultsCount);
            actual.ViewsCount.Should().Be(source.ViewsCount);
            actual.ApplicationStartedCount.Should().Be(source.ApplicationStartedCount);
            actual.ApplicationSubmittedCount.Should().Be(source.ApplicationSubmittedCount);
        }
    }
}
