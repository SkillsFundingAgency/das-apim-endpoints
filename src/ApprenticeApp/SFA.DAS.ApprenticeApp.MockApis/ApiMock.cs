using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SFA.DAS.ApprenticeApp.MockApis;
using WireMock.Logging;
using WireMock.Server;

namespace SFA.DAS.ApprenticeApp.MockApis
{
    [ExcludeFromCodeCoverageAttribute]
    public abstract class ApiMock : IDisposable, IResettable
    {
        private bool _disposed = false;

        public string BaseAddress { get; }

        protected WireMockServer MockServer { get; }

        protected ApiMock(int port = 0, bool ssl = false)
        {
            MockServer = WireMockServer.Start(port, ssl);
            BaseAddress = MockServer.Urls[0];
        }

        public string SingleLogBody => MockServer.LogEntries?.SingleOrDefault()?.RequestMessage?.Body;

        public IEnumerable<ILogEntry> LogEntries => MockServer.LogEntries;

        public void Reset()
        {
            MockServer.Reset();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                MockServer?.Stop();
                MockServer?.Dispose();
            }

            _disposed = true;
        }
    }
}