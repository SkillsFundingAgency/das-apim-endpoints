using System;
using System.Linq;
using WireMock.Server;

namespace SFA.DAS.ApprenticeCommitments.MockApis
{
    public class Program
    {
        private const int PortInnerApi = 5501;
        private const int PortCommitmentsApi = 5011;
        private const int PortLoginApi = 5001;
        private const int PortRoatpApi = 37951;

        private const long EmployerAccountId = 1000;
        private const long ApprenticeshipId = 20000;
        private const long TrainingProviderId = 1007777;

        private static WireMockServer _fakeInnerApi;
        private static WireMockServer _fakeCommitmentsV2Api;
        private static WireMockServer _fakeApprenticeLoginApi;
        private static WireMockServer _fakeTrainingProviderApi;

        static void Main(string[] args)
        {
            if (args.Contains("--h"))
            {
                Console.WriteLine("Optional parameters (!inner, !commitment, !login, !roatp) will exclude that fake API");
                Console.WriteLine("examples:");
                Console.WriteLine("SFA.DAS.ApprenticeCommitments.MockApis --h                 <-- shows this page");
                Console.WriteLine("SFA.DAS.ApprenticeCommitments.MockApis !inner              <-- excludes fake inner api");
                Console.WriteLine("SFA.DAS.ApprenticeCommitments.MockApis !inner !commitments <-- excludes fake inner and commitments api");
                Console.WriteLine("SFA.DAS.ApprenticeCommitments.MockApis !login !roatp       <-- excludes fake login and roatp api");

                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Currently supported values");
                Console.WriteLine($"EmployerAccountId : {EmployerAccountId}");
                Console.WriteLine($"ApprenticeshipId : {ApprenticeshipId}");
                Console.WriteLine($"TrainingProviderId : {TrainingProviderId}");

                return;
            }

            try
            {
                if (!args.Contains("!inner", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeInnerApi = ApprenticeCommitmentsInnerApiBuilder.Create(PortInnerApi)
                        .WithPing()
                        .WithAnyNewApprenticeship()
                        .Build();
                }

                if (!args.Contains("!commitments", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeCommitmentsV2Api = CommitmentsV2ApiBuilder.Create(PortCommitmentsApi)
                        .WithPing()
                        .WithAValidApprentice(EmployerAccountId, ApprenticeshipId)
                        .Build();
                }

                if (!args.Contains("!login", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeApprenticeLoginApi = ApprenticeLoginApiBuilder.Create(PortLoginApi)
                        .WithPing()
                        .WithAnyInvitation()
                        .Build();
                }

                if (!args.Contains("!roatp", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeTrainingProviderApi = TrainingProviderApiBuilder.Create(PortRoatpApi)
                        .WithPing()
                        .WithValidSearch(TrainingProviderId)
                        .Build();
                }

                Console.WriteLine(("Please RETURN to stop server"));
                Console.ReadLine();
            }
            finally
            {
                _fakeInnerApi?.Stop();
                _fakeInnerApi?.Dispose();

                _fakeCommitmentsV2Api?.Stop();
                _fakeCommitmentsV2Api?.Dispose();

                _fakeApprenticeLoginApi?.Stop();
                _fakeApprenticeLoginApi?.Dispose();

                _fakeTrainingProviderApi?.Stop();
                _fakeTrainingProviderApi?.Dispose();
            }
        }
    }
}
