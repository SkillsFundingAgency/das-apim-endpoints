using System;
using System.Net.Http;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests
{
    public class TestContext : IDisposable
    {
        public MockApi InnerApi { get; set; }
        public MockApi CommitmentsV2InnerApi { get; set; }
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
                OuterApiClient?.Dispose();
                InnerApi?.Dispose();
                CommitmentsV2InnerApi?.Dispose();
            }

            _isDisposed = true;
        }
    }
}
