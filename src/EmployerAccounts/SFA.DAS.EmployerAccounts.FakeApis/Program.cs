using System;
using System.Diagnostics.CodeAnalysis;
using WireMock.Server;

namespace SFA.DAS.EmployerAccounts.FakeApis
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        private const string AccountId = "1";
        private const int PortReservationsApi = 5604;
        private static WireMockServer _fakeReservationsApi;
        static void Main(string[] args)
        {
            try
            {
                _fakeReservationsApi = ReservationsApiBuilder.Create(PortReservationsApi)
                    .WithPing()
                    .WithReservations(AccountId)
                    .Build();

                Console.WriteLine("Press any key to stop ...");
                Console.ReadKey();
            }
            finally
            {
                _fakeReservationsApi?.Stop();
                _fakeReservationsApi?.Dispose();
            }
        }
    }
}
