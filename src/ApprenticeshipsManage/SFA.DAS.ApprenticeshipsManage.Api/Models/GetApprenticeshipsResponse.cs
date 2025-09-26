using SFA.DAS.ApprenticeshipsManage.Application.Queries.GetApprenticeships;
using SFA.DAS.ApprenticeshipsManage.InnerApi.Responses;

namespace SFA.DAS.ApprenticeshipsManage.Api.Models;

public class GetApprenticeshipsResponse
{
    public List<Learning> Apprenticeships { get; set; } = [];
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }

    public static implicit operator GetApprenticeshipsResponse(GetApprenticeshipsQueryResult source)
    {
        if (source == null) return new GetApprenticeshipsResponse();

        return new GetApprenticeshipsResponse()
        {
            Total = source.TotalItems,
            Page = source.Page,
            PageSize = source.PageSize,
            TotalPages = source.TotalPages,
            Apprenticeships = source.Items
        };
    }
}
