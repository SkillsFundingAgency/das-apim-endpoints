using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests.Roatp;
public class GetAllowedProvidersListRequest : IGetApiRequest
{
    public string? SortColumn { get; }
    public string? SortOrder { get; }

    public GetAllowedProvidersListRequest(string? sortColumn, string? sortOrder)
    {
        SortColumn = sortColumn;
        SortOrder = sortOrder;
    }

    public string GetUrl => $"AllowedProviders?sortColumn={SortColumn}&sortOrder={SortOrder}";
}
