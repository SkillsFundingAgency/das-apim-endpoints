﻿using SFA.DAS.ApprenticeshipsManage.Infrastructure;
using SFA.DAS.ApprenticeshipsManage.InnerApi.Responses;

namespace SFA.DAS.ApprenticeshipsManage.Application.Queries.GetApprenticeships;
public class GetApprenticeshipsQueryResult : PagedQueryResult<Apprenticeship>
{

    public static implicit operator GetApprenticeshipsQueryResult(PagedApprenticeshipsResponse source)
    {
        if (source == null) return new GetApprenticeshipsQueryResult();

        return new GetApprenticeshipsQueryResult()
        {
            TotalItems = source.TotalItems,
            Page = source.Page,
            PageSize = source.PageSize,
            Items = source.Apprenticeships
        };
    }
}