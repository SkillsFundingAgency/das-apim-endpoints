using System.Linq;
using SFA.DAS.Recruit.Api.Extensions;
using SFA.DAS.Recruit.Api.Models.Requests;
using SFA.DAS.Recruit.GraphQL;

namespace SFA.DAS.Recruit.Api.UnitTests.Extensions;

public class WhenBuildingVacancySortColumn
{
    [TestCase(SortOrder.Asc, SortEnumType.Asc)]
    [TestCase(SortOrder.Desc, SortEnumType.Desc)]
    [TestCase(null, SortEnumType.Asc)]
    public void Then_The_Sort_Order_Is_Built_Correctly(SortOrder? sortOrder, SortEnumType expectedSortOrder)
    {
        // arrange
        var sortParams = new SortParams<VacancySortColumn>
        {
            SortOrder = sortOrder,
            SortColumn = VacancySortColumn.CreatedDate
        };

        // act
        var result = sortParams.Build().First();

        // assert
        result.CreatedDate.Should().NotBeNull();
        result.CreatedDate.Value.Should().Be(expectedSortOrder);
    }
    
    [Test]
    public void The_ClosingDate_Field_Has_The_Sort_Order_Assigned()
    {
        // arrange
        var sortParams = new SortParams<VacancySortColumn>
        {
            SortOrder = SortOrder.Asc,
            SortColumn = VacancySortColumn.ClosingDate
        };

        // act
        var result = sortParams.Build().First();

        // assert
        result.ClosingDate.Should().NotBeNull();
        result.ClosingDate.Value.Should().Be(SortEnumType.Asc);
    }
}