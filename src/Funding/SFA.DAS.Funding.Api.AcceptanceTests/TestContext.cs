using System;
using System.Net.Http;
using SFA.DAS.Funding.Api.AcceptanceTests.Bindings;

namespace SFA.DAS.Funding.Api.AcceptanceTests
{
    public static class MockServers
    {
        public static MockApi InnerApi { get; set; }
    }
    
    public class TestContext : IDisposable
    {
        public MockApi InnerApi
        {
            get => MockServers.InnerApi;
            set
            {
                MockServers.InnerApi = value;
                CleanUpOuterApi();
            }
        }
        
        public HttpClient OuterApiClient { get; set; }

        private bool _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                InnerApi?.Reset();
            }

            _isDisposed = true;
        }

        private void CleanUpOuterApi()
        {
            OuterApi.Factory?.Dispose();
            OuterApi.Client?.Dispose();

            OuterApi.Factory = null;
            OuterApi.Client = null;
        }
    }
}
