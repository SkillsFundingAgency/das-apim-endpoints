using System;
using SFA.DAS.Recruit.Api.Models.Requests;

namespace SFA.DAS.Recruit.Api.Extensions;

public static class PagingParamsExtensions
{
    private const int MaxPage = 100;
    private const int MaxPageSize = 50;
    private const int MaxSkip = MaxPage * MaxPageSize;
    
    public static int Skip(this PageParams pageParams)
    {
        var skip = Convert.ToInt32((pageParams.PageNumber - 1) * pageParams.PageSize);
        return Math.Min(skip, MaxSkip);
    }
    
    public static int Take(this PageParams pageParams)
    {
        var take = Convert.ToInt32(pageParams.PageSize);
        return Math.Min(take, MaxPageSize);
    }
}