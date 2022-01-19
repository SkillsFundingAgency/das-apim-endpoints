using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmploymentCheck.Clients
{
    public interface IEmploymentCheckApiClient<T> : IInternalApiClient<T>
    {
        Task<HttpStatusCode> GetResponseCode(GetPingRequest getPingRequest);
    }
}