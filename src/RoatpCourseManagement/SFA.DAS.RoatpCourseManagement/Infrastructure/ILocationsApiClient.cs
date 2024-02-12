using System.Threading.Tasks;
using RestEase;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

namespace SFA.DAS.RoatpCourseManagement.Infrastructure;
public interface ILocationApiClient
{
    [Get("api/addresses")]
    Task<GetAddressesListResponse> GetExactMatchAddresses([Query] string query, [Query] double minMatch);
}
