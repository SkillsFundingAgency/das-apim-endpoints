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


        private const long EmployerAccountId = 1876;
        private const long ApprenticeshipId = 986872;
        private const long TrainingProviderId = 100077078;



        private static WireMockServer _fakeInnerApi;
        private static WireMockServer _fakeCommitmentsV2Api;
        private static WireMockServer _fakeApprenticeLoginApi;
        private static WireMockServer _fakeTrainingProviderApi;

        static void Main(string[] args)
        {
            try
            {
                if (!args.Contains("!inner", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeInnerApi = ApprenticeCommitmentsInnerApiBuilder.Create(PortInnerApi)
                        .WithAnyNewApprenticeship()
                        .Build();
                }

                if (!args.Contains("!commitments", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeCommitmentsV2Api = CommitmentsV2ApiBuilder.Create(PortCommitmentsApi)
                        .WithAValidApprentice(EmployerAccountId, ApprenticeshipId)
                        .Build();
                }

                if (!args.Contains("!login", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeApprenticeLoginApi = ApprenticeLoginApiBuilder.Create(PortLoginApi)
                        .WithAnyInvitation()
                        .Build();
                }

                if (!args.Contains("!roatp", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeTrainingProviderApi = TrainingProviderApiBuilder.Create(PortRoatpApi)
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
