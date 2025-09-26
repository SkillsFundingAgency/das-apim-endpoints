

using SFA.DAS.AdminRoatp.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProvidersAllowedList;
public class GetProvidersAllowedListQueryResponse
{
    public List<AllowedProvider> Providers { get; set; } = new List<AllowedProvider>();
}