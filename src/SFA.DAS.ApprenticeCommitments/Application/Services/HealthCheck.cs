using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Services
{
    public static class HealthCheck
    {
        public static Task<bool> IsHealthy<T>(IGetApiClient<T> client)
            => IsHealthy(client, new GetPingRequest());

        public static async Task<bool> IsHealthy<T>(IGetApiClient<T> client, IGetApiRequest request)
        {
            try
            {
                var status = await client.GetResponseCode(request);
                return status == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
    }
}