using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ICustomerEngagementApiClient<T> : IGetApiClient<T>
    {
        Task<HttpStatusCode> GetResponseCode(IGetApiRequest request);
    }
}
