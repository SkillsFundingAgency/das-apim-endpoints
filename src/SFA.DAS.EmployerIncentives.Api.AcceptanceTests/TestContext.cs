using System;
using System.Net.Http;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests
{
    public class TestContext : IDisposable
    {
        public MockApi InnerApi { get; set; }
        public MockApi CommitmentsV2InnerApi { get; set; }
        public MockApi FinanceApiV1 { get; set; }
        public HttpClient OuterApiClient { get; set; }
        public LocalWebApplicationFactory<Startup> Factory { get; set; }

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
                Factory?.Dispose();
                OuterApiClient?.Dispose();
                InnerApi?.Dispose();
                CommitmentsV2InnerApi?.Dispose();
                FinanceApiV1?.Dispose();
            }

            _isDisposed = true;
        }
    }
}
