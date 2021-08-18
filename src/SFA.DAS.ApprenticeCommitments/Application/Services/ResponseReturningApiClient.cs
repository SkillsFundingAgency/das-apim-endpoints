using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Json;
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

        public async Task<IActionResult> Post(string url, object data)
            => await Request(client => client.PostAsync(url, CreateJsonContent(data)));

        public async Task<IActionResult> Patch(string url, object data)
            => await Request(client => client.PatchAsync(url, CreateJsonContent(data)));

        private async Task<IActionResult> Request(Func<HttpClient, Task<HttpResponseMessage>> request)
        {
            var client = await _client.PrepareClient();

            var response = await request(client);

            return new HttpResponseMessageResult(response);
        }

        private static JsonContent CreateJsonContent(object data)
            => data == null ? null : JsonContent.Create(data);
    }
}