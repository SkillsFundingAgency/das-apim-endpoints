﻿using System;
using WireMock.Server;

namespace SFA.DAS.EmploymentCheck.Api.AcceptanceTests
{
    public class MockApi : IDisposable
    {
        private bool _isDisposed;

        public string BaseAddress { get; }
        public string CompanyName { get; }
        public string ApiVersion { get; }

        public WireMockServer MockServer { get; }

        public MockApi()
        {
            MockServer = WireMockServer.Start();
            BaseAddress = MockServer.Urls[0];
            CompanyName = "104 - ESFA";
            ApiVersion = "2021-04-06";
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Reset()
        {
            MockServer.Reset();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                if (MockServer.IsStarted)
                {
                    MockServer.Stop();
                }
                MockServer.Dispose();
            }

            _isDisposed = true;
        }
    }
}