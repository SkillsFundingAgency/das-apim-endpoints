using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    public class GetRequest<TResponse> : IGetApiRequest
    {
        public string GetUrl { get; }

        public GetRequest(string url) => GetUrl = url;

        public async Task<IActionResult> Get<TClient>(IInternalApiClient<TClient> client)
        {
            var response = await client.GetWithResponseCode<TResponse>(this);

            if (!string.IsNullOrEmpty(response.ErrorContent))
            {
                return new ObjectResult(response.ErrorContent)
                {
                    StatusCode = (int)response.StatusCode
                };
            }

            return new ObjectResult(response.Body)
            {
                StatusCode = (int)response.StatusCode
            };
        }
    }
}