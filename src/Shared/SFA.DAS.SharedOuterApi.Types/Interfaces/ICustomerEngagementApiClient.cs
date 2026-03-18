using System.Net;

using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Interfaces
{
    public interface ICustomerEngagementApiClient<T> : IGetApiClient<T>
    {
        Task<HttpStatusCode> GetResponseCode(IGetApiRequest request);
    }
}
