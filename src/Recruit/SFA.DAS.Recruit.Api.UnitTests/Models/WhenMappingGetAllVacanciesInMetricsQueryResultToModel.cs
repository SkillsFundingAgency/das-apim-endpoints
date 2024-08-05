using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetAllVacanciesInMetrics;

namespace SFA.DAS.Recruit.Api.UnitTests.Models;

public class WhenMappingGetAllVacanciesInMetricsQueryResultToModel
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped_To_Long(List<long> vacancies1, List<long> vacancies2)
    {
        var vacancies = new List<string>();
        vacancies.AddRange(vacancies1.Select(c=>c.ToString()));
        vacancies.AddRange(vacancies2.Select(c=>$"VAC{c}"));

        var source = new GetAllVacanciesInMetricsQueryResult
        {
            Vacancies = vacancies
        };
        
        var actual = (GetAllVacanciesInMetricsApiResponse)source;
        var expectedList = new List<long>();
        expectedList.AddRange(vacancies1);
        expectedList.AddRange(vacancies2);
        actual.Vacancies.Should().BeEquivalentTo(expectedList);
    }
}