using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ILepsNeApiClient<T> : ILepsNeExternalApiClient<T>
    {
        Task<HttpStatusCode> GetResponseCode(IGetApiRequest request);
    }
}
