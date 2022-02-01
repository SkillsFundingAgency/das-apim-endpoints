using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests
{
    public class ApprenticeCommitmentsApi
    {
        public HttpClient Client { get; private set; }
        public HttpResponseMessage Response { get; set; }
        public Uri BaseAddress { get; private set; }
        private bool isDisposed;

        public ApprenticeCommitmentsApi(HttpClient client)
        {
            Client = client;
            BaseAddress = client.BaseAddress;
        }

        public async Task Post<T>(string url, T data)
        {
            Response = await Client.PostValueAsync(url, data);
        }

        public async Task Post(string url, string jsonData)
        {
            Response = await Client.PostJsonAsync(url, jsonData);
        }

        public async Task Put<T>(string url, T data)
        {
            Response = await Client.PutValueAsync(url, data);
        }

        public async Task Patch<T>(string url, T data)
        {
            Response = await Client.PatchValueAsync(url, data);
        }

        public async Task Delete(string url)
        {
            Response = await Client.DeleteAsync(url);
        }

        public async Task Get(string url)
        {
            Response = await Client.GetAsync(url);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                Response?.Dispose();
            }

            isDisposed = true;
        }
    }
}