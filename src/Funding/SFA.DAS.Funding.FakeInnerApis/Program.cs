using System;
using WireMock.Server;
using WireMock.Settings;

namespace SFA.DAS.Funding.FakeInnerApis
{
    public class Program
    {
        private static WireMockServer _fakeAccountsApi;

        static void Main(string[] args)
        {
            try
            {
                _fakeAccountsApi = WireMockServer.Start(new FluentMockServerSettings
                {
                    Urls = new[] { "http://*:6012" },
                    StartAdminInterface = true,
                });

                Console.WriteLine(("Please RETURN to stop server"));
                Console.ReadLine();
            }
            finally
            {
                _fakeAccountsApi.Stop();
                _fakeAccountsApi.Dispose();
            }
        }
    }
}
