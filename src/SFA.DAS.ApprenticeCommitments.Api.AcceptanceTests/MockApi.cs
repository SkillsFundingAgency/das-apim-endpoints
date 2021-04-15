using System;
using WireMock.Server;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests
{
    public class MockApi : IDisposable, IResettable
    {
        private bool _isDisposed;

        public string BaseAddress { get; private set; }

        public WireMockServer MockServer { get; private set; }

        public MockApi()
        {
            MockServer = WireMockServer.Start();
            BaseAddress = MockServer.Urls[0];
        }

        public void Reset()
        {
            MockServer.Reset();
        }

        public void Dispose()
        {
            MockServer.Stop();
            MockServer.Dispose();
        }
    }
}