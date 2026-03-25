using System.Collections.Generic;
using SFA.DAS.Recruit.Api.Extensions;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.UnitTests.Extensions;

public class WhenAssigningVacancyStats
{
    [Test, MoqAutoData]
    public void Then_The_Stats_Data_Should_Be_Assigned(List<IGetPagedVacanciesList_PagedVacancies_Items> items, List<VacancyStatsItem> statsData)
    {
        // arrange
        var dict = new Dictionary<long, VacancyStatsItem>();
        for (var index = 0; index < items.Count; index++)
        {
            dict.Add(items[index].VacancyReference!.Value, statsData[index]);
        }

        // act
        var result = items.AssignStatsToVacancies(dict);

        // assert
        result.Should().NotBeNull();
        result.Count.Should().Be(items.Count);
        result.Should().AllSatisfy(x =>
        {
            x.Stats.Should().Be(dict[x.VacancyReference!.Value]);
        });
    }
    
    [Test, MoqAutoData]
    public void Then_Missing_Stats_Result_In_A_Null_Value_On_The_Vacancy(List<IGetPagedVacanciesList_PagedVacancies_Items> items)
    {
        // arrange
        var dict = new Dictionary<long, VacancyStatsItem>();
        
        // act
        var result = items.AssignStatsToVacancies(dict);

        // assert
        result.Should().NotBeNull();
        result.Count.Should().Be(items.Count);
        result.Should().AllSatisfy(x =>
        {
            x.Stats.Should().BeNull();
        });
    }
}