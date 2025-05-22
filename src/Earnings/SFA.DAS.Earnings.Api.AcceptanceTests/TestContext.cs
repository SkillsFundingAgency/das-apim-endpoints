﻿using SFA.DAS.Earnings.Api.AcceptanceTests.Bindings;
using SFA.DAS.Earnings.Api.AcceptanceTests.Models;

namespace SFA.DAS.Earnings.Api.AcceptanceTests
{
    public static class MockServers
    {
        public static MockApi EarningsApi { get; set; }
        public static MockApi ApprenticeshipsApi { get; set; }
        public static MockApi CollectionCalendarApi { get; set; }
    }

    public class TestContext : IDisposable
    {
        public ApprenticeshipModel Apprenticeship { get; set; } = new ApprenticeshipModel();

        public MockApi EarningsApi
        {
            get => MockServers.EarningsApi;
            set
            {
                MockServers.EarningsApi = value;
                CleanUpOuterApi();
            }
        }

        public MockApi ApprenticeshipsApi
        {
            get => MockServers.ApprenticeshipsApi;
            set { 
                MockServers.ApprenticeshipsApi = value;
                CleanUpOuterApi();
            }
        }

        public MockApi CollectionCalendarApi
        {
            get => MockServers.CollectionCalendarApi;
            set
            {
                MockServers.CollectionCalendarApi = value;
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
                EarningsApi?.Reset();
                ApprenticeshipsApi?.Reset();
                CollectionCalendarApi?.Reset();
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
