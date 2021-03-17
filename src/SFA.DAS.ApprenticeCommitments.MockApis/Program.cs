using System;
using WireMock.Server;

namespace SFA.DAS.ApprenticeCommitments.MockApis
{
    public class Program
    {
        private static WireMockServer _fakeApprenticeCommitmentsInnerApi;
        private static WireMockServer _fakeCommitmentsV2Api;
        private static WireMockServer _fakeApprenticeLoginApi;
        private static WireMockServer _fakeTrainingProviderApi;

        static void Main(string[] args)
        {
            try
            {
                _fakeApprenticeCommitmentsInnerApi = WireMockServer.StartWithAdminInterface(5501, true);
                _fakeCommitmentsV2Api = WireMockServer.StartWithAdminInterface(5011, true);
                _fakeApprenticeLoginApi = WireMockServer.StartWithAdminInterface(5001, true);
                _fakeTrainingProviderApi = WireMockServer.StartWithAdminInterface(37951, true);

                Console.WriteLine(("Please RETURN to stop server"));
                Console.ReadLine();
            }
            finally
            {
                _fakeApprenticeCommitmentsInnerApi.Stop();
                _fakeApprenticeCommitmentsInnerApi.Dispose();

                _fakeCommitmentsV2Api.Stop();
                _fakeCommitmentsV2Api.Dispose();

                _fakeApprenticeLoginApi.Stop();
                _fakeApprenticeLoginApi.Dispose();

                _fakeTrainingProviderApi.Stop();
                _fakeTrainingProviderApi.Dispose();
            }
        }
    }
}
