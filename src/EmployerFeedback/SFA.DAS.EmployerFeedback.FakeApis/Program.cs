using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;

namespace SFA.DAS.EmployerFeedback.FakeApis
{
    public static class Program
    {
        private static Dictionary<string, FakeApiBuilder> _fakeApis = [];

        static void Main(string[] args)
        {
            var commitmentsV2Api = CommitmentsV2FakeApiBuilder.Create();
            var roatpV2Api = RoatpV2FakeApiBuilder.Create();

            _fakeApis[commitmentsV2Api.Name] = commitmentsV2Api;
            _fakeApis[roatpV2Api.Name] = roatpV2Api;

            if (args.Contains("--h"))
            {
                var ns = "EmployerFeedback";

                Console.WriteLine("examples:");
                Console.WriteLine($"SFA.DAS.{ns}.FakeApis --h                 <-- shows this page");
                Console.WriteLine($"SFA.DAS.{ns}.FakeApis                     <-- runs {string.Join(" and ", _fakeApis.Select(fakeApi => fakeApi.Value.Name))}");
                Console.WriteLine();

                foreach (var fakeApiName in _fakeApis.Select(fakeApi => fakeApi.Value.Name))
                {
                    Console.WriteLine($"SFA.DAS.{ns}.MockApis !{fakeApiName}     <-- exclude {fakeApiName}");
                }

                Console.WriteLine();
                Console.WriteLine($"To exclude multiple APIs use any combination of {string.Join(" ", _fakeApis.Select(fakeApi => "!" + fakeApi.Value.Name))} (parameters are case insensitive)");

                return;
            }

            try
            {
                if (!args.Contains("!" + commitmentsV2Api.Name, StringComparer.CurrentCultureIgnoreCase))
                {
                    commitmentsV2Api
                        .WithPing()
                        .With(
                            new GetAccountProvidersCourseStatusRequest(80641, 3, 60, 2),
                            new GetAccountProvidersCourseStatusResponse()
                            {
                                Active = [new AccountProviderCourse { Ukprn = 12345678, CourseCode = "123"}],
                                Completed = [],
                                NewStart = []
                            })
                        .With(
                            new GetAccountProvidersCourseStatusRequest(80642, 3, 60, 2),
                            new GetAccountProvidersCourseStatusResponse()
                            {
                                Active = [],
                                Completed = [new AccountProviderCourse { Ukprn = 12345678, CourseCode = "123" }],
                                NewStart = []
                            })
                        .With(
                            new GetAccountProvidersCourseStatusRequest(80643, 3, 60, 2),
                            new GetAccountProvidersCourseStatusResponse()
                            {
                                Active = [],
                                Completed = [],
                                NewStart = [new AccountProviderCourse { Ukprn = 12345678, CourseCode = "123" }],
                            })
                        .Build();

                    Console.WriteLine("");

                }

                if (!args.Contains("!" + roatpV2Api.Name, StringComparer.CurrentCultureIgnoreCase))
                {
                    roatpV2Api
                        .WithPing()
                        .With(
                            new GetRoatpProvidersRequest
                            { 
                                Live = true
                            },
                            new GetProvidersResponse
                            {
                                RegisteredProviders = new List<Provider>
                                {
                                    new Provider 
                                    {
                                        Name = "Fake Provider",
                                        Ukprn = 12345678,
                                        ProviderTypeId = 1
                                    }
                                }
                            })
                        .Build();

                    Console.WriteLine("");
                }

                Console.WriteLine(("Please RETURN to stop server"));
                Console.ReadLine();
            }
            finally
            {
                foreach (var fakeApi in _fakeApis.Values)
                    fakeApi.Stop();
            }
        }
    }
}
