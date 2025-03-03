﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.Api.UnitTests
{
    public class ApprenticePortalOuterApi
    {
        public HttpClient Client { get; private set; }
        public HttpResponseMessage Response { get; set; }
        public object ReturnValue { get; set; }
        public Uri BaseAddress { get; private set; }
        private bool isDisposed;

        public ApprenticePortalOuterApi(HttpClient client)
        {
            Client = client;
            BaseAddress = client.BaseAddress;
        }

        public async Task Post<T>(string url, T data)
        {
            Response = await Client.PostValueAsync(url, data);
        }

        public async Task Put<T>(string url, T data)
        {
            var result = await Client.PutValueAsync(url, data);
            ReturnValue = result.Item2;
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