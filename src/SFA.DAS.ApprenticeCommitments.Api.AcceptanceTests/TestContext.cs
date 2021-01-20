using System;
using System.Net.Http;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests
{
    public class TestContext : IDisposable
    {
        public HttpClient OuterApiClient { get; set; }
        public MockApi InnerApi { get; set; }

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
                InnerApi?.Dispose();
            }

            _isDisposed = true;
        }
    }
}
