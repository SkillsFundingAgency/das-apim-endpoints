using System;
using System.Linq;
using WireMock.Server;

namespace SFA.DAS.Approvals.FakeApis
{
    public static class Program
    {
        private const int PortProviderCoursesApi = 5603;
        private const int PeterboroughCollege = 10005077;
        private const string CourseWithMultipleDeliveryModels = "244";
        private const string CourseWithOnlyRegularDeliveryModel = "245";
        private const string CourseWithNoDeliveryModelsFound = "246";


        private static WireMockServer _fakeProviderCoursesApi;

        static void Main(string[] args)
        {
            if (args.Contains("--h"))
            {
                Console.WriteLine("examples:");
                Console.WriteLine("SFA.DAS.ApprenticeCommitments.MockApis --h                 <-- shows this page");
                Console.WriteLine("SFA.DAS.ApprenticeCommitments.MockApis                     <-- runs Provider Courses API");
                
                return;
            }

            try
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
