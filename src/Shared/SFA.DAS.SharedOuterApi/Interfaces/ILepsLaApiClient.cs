using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ILepsLaApiClient<T> : ILepsLaExternalApiClient<T>
    {
        Task<HttpStatusCode> GetResponseCode(IGetApiRequest request);
    }
}
