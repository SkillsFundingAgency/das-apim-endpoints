using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{

    public class PostRequest : IGetApiRequest
    {
        public string GetUrl { get; }

        public PostRequest(string url) => GetUrl = url;

        public async Task<IActionResult> Post<TClient, TRequest>(IInternalApiClient<TClient> client, TRequest request)
        {
            var response = await client.PostWithResponseCode<object>(new PostRequestObj
            {
                PostUrl = GetUrl,
                Data = request,
            });

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

        private class PostRequestObj : IPostApiRequest
        {
            public string PostUrl { get; set; }
            public object Data { get; set; }
        }
    }
}