using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
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
            => await Request(client => client.GetAsync(url));

        public async Task<IActionResult> Post<T>(string url, T data)
            => await Request(client => client.PostAsync(url, CreateJsonContent(data)));

        public async Task<IActionResult> Patch<T>(string url, T data)
            => await Request(client => client.PatchAsync(url, CreateJsonContent(data)));

        private async Task<IActionResult> Request(Func<HttpClient, Task<HttpResponseMessage>> request)
        {
            var client = await _client.PrepareClient();

            var response = await request(client);

            return new HttpResponseMessageResult(response);
        }

        private static HttpContent CreateJsonContent<T>(T data)
            => data == null ? null : new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
    }
}