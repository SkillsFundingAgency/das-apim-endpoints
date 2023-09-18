using System;
using System.Linq;
using WireMock.Server;

namespace SFA.DAS.ApprenticeFeedback.MockApis
{
    public class Program
    {
        private const int PortAssessorApi = 59022;
        
        private static WireMockServer _fakeAssessorApi;
        
        static void Main(string[] args)
        {
            if (args.Contains("--h"))
            {
                Console.WriteLine("Optional parameters (!assessor) will exclude that fake API");
                Console.WriteLine("examples:");
                Console.WriteLine("SFA.DAS.ApprenticeFeedback.MockApis --h                 <-- shows this page");
                Console.WriteLine("SFA.DAS.ApprenticeFeedback.MockApis !assessor           <-- excludes fake assessor api");

                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Currently supported values");

                return;
            }

            try
            {
                if (!args.Contains("!assessor", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeAssessorApi = AssessorInnerApiBuilder.Create(PortAssessorApi)
                        .WithPing()
                        .WithLearner()
                        .Build();
                }

                Console.WriteLine(("Please RETURN to stop server"));
                Console.ReadLine();
            }
            finally
            {
                _fakeAssessorApi?.Stop();
                _fakeAssessorApi?.Dispose();
            }
        }
    }
}
