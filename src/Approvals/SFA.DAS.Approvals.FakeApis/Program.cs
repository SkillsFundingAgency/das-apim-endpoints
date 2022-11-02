using System;
using System.Linq;
using WireMock.Server;

namespace SFA.DAS.Approvals.FakeApis
{
    public static class Program
    {
        private const int PortProviderCoursesApi = 5603;
        private const int PortCoursesApi = 6001;
        private const int PortReservationsApi = 5003;
        private const int PeterboroughCollege = 10005077;
        private const string CourseWithMultipleDeliveryModels = "244";
        private const string CourseWithOnlyRegularDeliveryModel = "245";
        private const string CourseWithNoDeliveryModelsFound = "246";


        private static WireMockServer _fakeProviderCoursesApi;
        private static WireMockServer _fakeCoursesApi;

        static void Main(string[] args)
        {
            if (args.Contains("--h"))
            {
                Console.WriteLine("examples:");
                Console.WriteLine("SFA.DAS.ApprenticeCommitments.MockApis --h                 <-- shows this page");
                Console.WriteLine("SFA.DAS.ApprenticeCommitments.MockApis                     <-- runs Provider Courses API and Courses API and Reservations API");
                Console.WriteLine("SFA.DAS.ApprenticeCommitments.MockApis !Courses            <-- exclude Courses API");
                Console.WriteLine("SFA.DAS.ApprenticeCommitments.MockApis !ProviderCourses    <-- exclude Provider Courses API");
                Console.WriteLine("SFA.DAS.ApprenticeCommitments.MockApis !Reservations       <-- exclude Reservations API");
                Console.WriteLine("To exclude multiple APIs use any combination of !Courses !ProviderCourses !Reservations (parameters are case insensitive)");

                return;
            }

            try
            {


                if (!args.Contains("!ProviderCourses", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeProviderCoursesApi = ProviderCoursesApiBuilder.Create(PortProviderCoursesApi)
                        .WithPing()
                        .WithHasPortableFlexiJobOption(PeterboroughCollege, CourseWithMultipleDeliveryModels)
                        .WithHasPortableFlexiJobOptionFalse(PeterboroughCollege, CourseWithOnlyRegularDeliveryModel)
                        .WithNoCoursesDeliveryModels404(PeterboroughCollege, CourseWithNoDeliveryModelsFound)
                        .Build();

                    Console.WriteLine("");
                    Console.WriteLine($"Provider (Peterborough College) : {PeterboroughCollege}");
                    Console.WriteLine($"Course (Course With Multiple Delivery Models) : {CourseWithMultipleDeliveryModels}");
                    Console.WriteLine($"Course (Course With Only Regular Delivery Model) : {CourseWithOnlyRegularDeliveryModel}");
                    Console.WriteLine($"Course (Course With No Delivery Model) : {CourseWithNoDeliveryModelsFound}");
                    Console.WriteLine("");
                }

                if (!args.Contains("!Courses", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeCoursesApi = CoursesApiBuilder.Create(PortCoursesApi)
                        .WithPing()
                        .WithAnyCourseStandard()
                        .Build();
                }

                if (!args.Contains("!Reservations", StringComparer.CurrentCultureIgnoreCase))
                {
                    _fakeCoursesApi = ReservationsApiBuilder.Create(PortReservationsApi)
                        .WithPing()
                        .WithBulkUploadValidate()
                        .Build();
                }


                Console.WriteLine(("Please RETURN to stop server"));
                Console.ReadLine();
            }
            finally
            {
                _fakeProviderCoursesApi?.Stop();
                _fakeProviderCoursesApi?.Dispose();
            }
        }
    }
}
