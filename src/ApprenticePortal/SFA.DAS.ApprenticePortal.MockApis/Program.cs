using System;
using System.Linq;
using SFA.DAS.ApprenticePortal.MockApis.Helpers;

namespace SFA.DAS.ApprenticePortal.MockApis
{
    public static class Program
    {
        private const int PortInnerApi = 5501;
        private const int PortCommitmentsV2InnerApi = 5011;
        private const int PortAccountsApi = 5801;
        private const int PortProviderInnerApi = 37951;
        private const int PortCoursesInnerApi = 5022;


        private static ApprenticeCommitmentsInnerApiMock _fakeApprenticeCommitmentsApi;
        private static ApprenticeAccountsInnerApiMock _fakeApprenticeAccountsApi;
        private static CommitmentsV2InnerApiMock _fakeCommitmentsV2InnerApi;
        private static TrainingProviderInnerApiMock _fakeTrainingProviderInnerApi;
        private static CoursesInnerApiMock _fakeCoursesInnerApi;

        static void Main(string[] args)
        {
            if (args.Contains("--h"))
            {
                Console.WriteLine("Optional parameters (!cmad, !accounts) will exclude that fake API");
                Console.WriteLine("examples:");
                Console.WriteLine("SFA.DAS.ApprenticePortal.MockApis --h                <-- shows this page");
                Console.WriteLine("SFA.DAS.ApprenticePortal.MockApis !cmad              <-- excludes fake inner ApprenticeCommitments api");
                Console.WriteLine("SFA.DAS.ApprenticePortal.MockApis !cmad !account     <-- excludes fake inner ApprenticeCommitments api and inner ApprenticeAccounts api");
                Console.WriteLine("SFA.DAS.ApprenticePortal.MockApis !commitments       <-- excludes Commitments V2 api");
                Console.WriteLine("SFA.DAS.ApprenticePortal.MockApis !provider          <-- excludes Provider Account api");
                Console.WriteLine("SFA.DAS.ApprenticePortal.MockApis !courses          <-- excludes Courses api");

                Console.WriteLine("");
                Console.WriteLine("");

                return;
            }

            try
            {
                var apprentice = Fake.Apprentice;
                var apprenticeship = Fake.CommitmentsApprenticeship;
                var myApprenticeship = Fake.MyApprenticeship;
                var provider = Fake.Provider;
                provider.Ukprn = apprenticeship.ProviderId;

                if (!args.Contains("!accounts", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeApprenticeAccountsApi = new ApprenticeAccountsInnerApiMock(PortAccountsApi, true)
                        .WithPing()
                        .WithMyApprenticeship(apprentice, myApprenticeship)
                        .WithApprentice(apprentice)
                        .WithPostMyApprenticeship(apprentice);
                }

                if (!args.Contains("!commitments", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeCommitmentsV2InnerApi = new CommitmentsV2InnerApiMock(PortCommitmentsV2InnerApi, true)
                        .WithPing()
                        .WithApprenticeshipsResponseForApprentice(apprenticeship.Id, apprenticeship);
                }

                if (!args.Contains("!cmad", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeApprenticeCommitmentsApi = new ApprenticeCommitmentsInnerApiMock(PortInnerApi, true)
                        .WithPing()
                        .WithExistingApprenticeshipsForApprentice(_fakeApprenticeAccountsApi?.AnyApprentice);
                }

                if (!args.Contains("!provider", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeTrainingProviderInnerApi = new TrainingProviderInnerApiMock(PortProviderInnerApi, true)
                        .WithPing()
                        .WithValidSearch(apprenticeship.ProviderId, provider);
                }

                if (!args.Contains("!courses", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeCoursesInnerApi = new CoursesInnerApiMock(PortCoursesInnerApi, true)
                        .WithPing()
                        .WithStandardCourse(myApprenticeship.StandardUId, Fake.StandardApiResponse);
                }

                Console.WriteLine($"Apprentice Id {apprentice.ApprenticeId}");
                Console.WriteLine($"Apprenticeship Id {apprenticeship.Id}");
                Console.WriteLine("Please RETURN to stop server");
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
