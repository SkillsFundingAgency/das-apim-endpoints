using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WireMock.Server;

namespace SFA.DAS.EmployerRequestApprenticeTraining.MockApis
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        private const int PortAssessorApi = 59023;
        
        private static WireMockServer _fakeRatApi;
        
        static void Main(string[] args)
        {
            if (args.Contains("--h"))
            {
                Console.WriteLine("Optional parameters (!rat) will exclude that fake API");
                Console.WriteLine("examples:");
                Console.WriteLine("SFA.DAS.EmployerRequestApprenticeTraining.MockApis --h                 <-- shows this page");
                Console.WriteLine("SFA.DAS.EmployerRequestApprenticeTraining.MockApis !assessor           <-- excludes fake assessor api");

                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Currently supported values");

                return;
            }

            try
            {
                if (!args.Contains("!rat", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeRatApi = RequestApprenticeTrainingInnerApiBuilder.Create(PortAssessorApi)
                        .WithPing()
                        .Build();
                }

                Console.WriteLine(("Please RETURN to stop server"));
                Console.ReadLine();
            }
            finally
            {
                _fakeRatApi?.Stop();
                _fakeRatApi?.Dispose();
            }
        }
    }
}
