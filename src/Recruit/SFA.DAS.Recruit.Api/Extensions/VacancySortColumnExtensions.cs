using System;
using System.Collections.Generic;
using SFA.DAS.Recruit.Api.Models.Requests;
using SFA.DAS.Recruit.GraphQL;

namespace SFA.DAS.Recruit.Api.Extensions;

public static class VacancySortColumnExtensions
{
    public static List<VacancyEntitySortInput> Build(this SortParams<VacancySortColumn> sortParams)
    {
        var so = sortParams.SortOrder switch
        {
            SortOrder.Asc => SortEnumType.Asc,
            SortOrder.Desc => SortEnumType.Desc,
            _ => SortEnumType.Asc,
        };

        var sortColumn = sortParams.SortColumn switch
        {
            VacancySortColumn.ClosingDate => new VacancyEntitySortInput { ClosingDate = so },
            _ => new VacancyEntitySortInput { CreatedDate = so }
        };

        return [sortColumn];
    }
}