using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IGetApiClient<T>
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task<HttpStatusCode> GetResponseCode(IGetApiRequest request);
    }
}
