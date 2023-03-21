using System;
using System.Linq;

namespace SFA.DAS.ApprenticePortal.MockApis
{
    public static class Program
    {
        private const int PortInnerApi = 5501;
        private const int PortAccountsApi = 5801;

        private static ApprenticeCommitmentsInnerApiMock _fakeApprenticeCommitmentsApi;
        private static ApprenticeAccountsInnerApiMock _fakeApprenticeAccountsApi;

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

                if (!args.Contains("!accounts", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeApprenticeAccountsApi = new ApprenticeAccountsInnerApiMock(PortAccountsApi, true)
                        .WithPing()
                        .WithoutCurrentApprenticeship()
                        .WithAnyApprentice();
                }

                if (!args.Contains("!cmad", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeApprenticeCommitmentsApi = new ApprenticeCommitmentsInnerApiMock(PortInnerApi, true)
                        .WithPing()
                        .WithExistingApprenticeshipsForApprentice(_fakeApprenticeAccountsApi?.AnyApprentice);
                }

                Console.WriteLine(("Please RETURN to stop server"));
                Console.ReadLine();
            }
            finally
            {
                _fakeApprenticeCommitmentsApi?.Dispose();
                _fakeApprenticeAccountsApi?.Dispose();
            }
        }
    }
}
