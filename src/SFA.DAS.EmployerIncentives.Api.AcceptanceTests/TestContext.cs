using System;
using System.Net.Http;
using SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Bindings;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests
{
    public static class MockServers
    {
        public static MockApi InnerApi { get; set; }
        public static MockApi AccountsApi { get; set; }
        public static MockApi CommitmentsV2InnerApi { get; set; }
        public static MockApi FinanceApi { get; set; }
        public static MockApi EmploymentCheckApi { get; set; }
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

        public MockApi CommitmentsV2InnerApi
        {
            get => MockServers.CommitmentsV2InnerApi;
            set { 
                MockServers.CommitmentsV2InnerApi = value;
                CleanUpOuterApi();
            }
        }

        public MockApi FinanceApi
        {
            get => MockServers.FinanceApi;
            set
            {
                MockServers.FinanceApi = value;
                CleanUpOuterApi();
            }
        }

        public MockApi AccountsApi
        {
            get => MockServers.AccountsApi;
            set
            {
                MockServers.AccountsApi = value;
                CleanUpOuterApi();
            }
        }

        public MockApi EmploymentCheckApi
        {
            get => MockServers.EmploymentCheckApi;
            set
            {
                MockServers.EmploymentCheckApi = value;
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
                CommitmentsV2InnerApi?.Reset();
                FinanceApi?.Reset();
                AccountsApi?.Reset();
                EmploymentCheckApi?.Reset();
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
