using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.Models;
public class WhenMappingFromMediatorResponseToGetLiveVacanciesModel
{
    [Test, MoqAutoData]
    public void Then_The_Fields_Are_Correctly_Mapped(GetLiveVacanciesQueryResult source)
    {
        source.Vacancies.ToList().ForEach(x => x.IsRecruitVacancy = true);
        var actual = (GetLiveVacanciesApiResponse)source;

        using (new AssertionScope())
        {
            actual.Vacancies.Should().BeEquivalentTo(source.Vacancies);
            actual.PageSize.Should().Be(source.PageSize);
            actual.PageNo.Should().Be(source.PageNo);
            actual.TotalLiveVacanciesReturned.Should().Be(source.TotalLiveVacanciesReturned);
            actual.TotalLiveVacancies.Should().Be(source.TotalLiveVacancies);
            actual.TotalPages.Should().Be(source.TotalPages);
        }
    }
}
