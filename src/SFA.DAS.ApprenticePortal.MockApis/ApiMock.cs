using System;
using System.Collections.Generic;
using System.Linq;
using WireMock.Logging;
using WireMock.Server;

namespace SFA.DAS.ApprenticePortal.MockApis
{
    public abstract class ApiMock : IDisposable, IResettable
    {
        public string BaseAddress { get; }

        protected WireMockServer MockServer { get; }

        protected ApiMock(int? port = 0)
        {
            MockServer = WireMockServer.Start(port, true);
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
            MockServer?.Stop();
            MockServer?.Dispose();
        }
    }
}