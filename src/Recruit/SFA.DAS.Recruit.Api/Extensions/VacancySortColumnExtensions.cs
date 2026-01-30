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
            null => SortEnumType.Asc,
            _ => throw new ArgumentOutOfRangeException(nameof(sortParams))
        };

        var sortColumn = sortParams.SortColumn switch
        {
            VacancySortColumn.ClosingDate => new VacancyEntitySortInput { ClosingDate = so }, // default
            VacancySortColumn.Id => new VacancyEntitySortInput { Id = so },
            VacancySortColumn.VacancyReference => new VacancyEntitySortInput { VacancyReference = so },
            _ => throw new ArgumentOutOfRangeException(nameof(sortParams))
        };

        return [sortColumn];
    }
}