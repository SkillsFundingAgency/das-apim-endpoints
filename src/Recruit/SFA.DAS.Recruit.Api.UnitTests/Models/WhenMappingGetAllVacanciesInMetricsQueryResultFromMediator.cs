using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetAllVacanciesInMetrics;

namespace SFA.DAS.Recruit.Api.UnitTests.Models
{
    public class WhenMappingGetAllVacanciesInMetricsQueryResultFromMediator
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetAllVacanciesInMetricsQueryResult source)
        {
            var actual = (GetAllVacanciesInMetricsApiResponse)source;

            actual.Vacancies.Should().BeEquivalentTo(source.Vacancies);
        }
    }
}