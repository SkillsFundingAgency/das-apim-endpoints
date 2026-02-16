using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Services
{
    public class ResponseReturningApiClient
    {
        private readonly ApimClient _client;

        public ResponseReturningApiClient(ApimClient client)
            => _client = client;

        public async Task<IActionResult> Get(string url)
            => await Request(new HttpRequestMessage(HttpMethod.Get, url));

        public async Task<IActionResult> Post<T>(string url, T data)
            => await Request(new HttpRequestMessage(HttpMethod.Post, url) { Content = CreateJsonContent(data) });

        public async Task<IActionResult> Patch<T>(string url, T data)
            => await Request(new HttpRequestMessage(HttpMethod.Patch, url) { Content = CreateJsonContent(data) });

        private async Task<IActionResult> Request(HttpRequestMessage request)
        {
            var response = await _client.Send(request);

            return new HttpResponseMessageResult(response);
        }

        private static HttpContent CreateJsonContent<T>(T data)
            => data == null ? null : new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
    }
}