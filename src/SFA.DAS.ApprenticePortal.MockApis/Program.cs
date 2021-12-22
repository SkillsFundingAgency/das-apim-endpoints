using System;
using System.Linq;
using WireMock.Server;

namespace SFA.DAS.ApprenticePortal.MockApis
{
    public class Program
    {
        private const int PortInnerApi = 5501;
        private const int PortAccountsApi = 5801;

        private static WireMockServer _fakeApprenticeCommitmentsApi;
        private static WireMockServer _fakeApprenticeAccountsApi;

        static void Main(string[] args)
        {
            if (args.Contains("--h"))
            {
                Console.WriteLine("Optional parameters (!cmad, !accounts) will exclude that fake API");
                Console.WriteLine("examples:");
                Console.WriteLine("SFA.DAS.ApprenticePortal.MockApis --h                 <-- shows this page");
                Console.WriteLine("SFA.DAS.ApprenticePortal.MockApis !cmad              <-- excludes fake inner ApprenticeCommitments api");
                Console.WriteLine("SFA.DAS.ApprenticePortal.MockApis !cmad !account     <-- excludes fake inner ApprenticeCommitments api and inner ApprenticeAccounts api");

                Console.WriteLine("");
                Console.WriteLine("");

                return;
            }

            try
            {
                if (!args.Contains("!cmad", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeApprenticeCommitmentsApi = ApprenticeCommitmentsInnerApiBuilder.Create(PortInnerApi)
                        .WithPing()
                        .WithExistingApprenticeships()
                        .Build();
                }

                if (!args.Contains("!accounts", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeApprenticeAccountsApi = ApprenticeAccountsInnerApiBuilder.Create(PortAccountsApi)
                        .WithPing()
                        .WithAnyApprentice()
                        .Build();
                }

                Console.WriteLine(("Please RETURN to stop server"));
                Console.ReadLine();
            }
            finally
            {
                _fakeApprenticeCommitmentsApi?.Stop();
                _fakeApprenticeCommitmentsApi?.Dispose();

                _fakeApprenticeAccountsApi?.Stop();
                _fakeApprenticeAccountsApi?.Dispose();
            }
        }
    }
}
