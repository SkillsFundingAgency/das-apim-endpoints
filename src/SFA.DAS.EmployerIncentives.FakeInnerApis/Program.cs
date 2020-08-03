using System;
using System.IO;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace SFA.DAS.EmployerIncentives.FakeInnerApis
{
    public class Program
    {
        private static WireMockServer _fakeCommitmentsApi;
        private static long _accountId = 100;
        private static long _accountLegalEntityId = 2000;

        static void Main(string[] args)
        {
            try
            {
                _fakeCommitmentsApi = WireMockServer.Start(new FluentMockServerSettings
                {
                    Urls = new[] {"http://*:6011"},
                    StartAdminInterface = true,
                });

                SetupHealthCheckResponse();
                SetupApprenticeshipSearchResponses();

                Console.WriteLine(("Please RETURN to stop server"));
                Console.ReadLine();
            }
            finally
            {
                _fakeCommitmentsApi.Stop();
                _fakeCommitmentsApi.Dispose();
            }
        }

        static void SetupHealthCheckResponse()
        {
            _fakeCommitmentsApi.Given(
                    Request.Create().WithPath("/health")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody("{ \"Status\" : \"Healthy\" }"));
        }

        static void SetupApprenticeshipSearchResponses()
        {
            // Return an Empty result
            _fakeCommitmentsApi.Given(
                Request.Create().WithPath("/api/apprenticeships")
                    .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(GetSampleJsonResponse("ApprenticeshipSearchNoData.json")));

            // Return sample values for Account 100 and ALE 2000 
            _fakeCommitmentsApi.Given(
                    Request.Create().WithPath("/api/apprenticeships")
                        .WithParam("accountId", true, _accountId.ToString())
                        .WithParam("accountLegalEntityId", true, _accountLegalEntityId.ToString())
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(GetSampleJsonResponse("ApprenticeshipSearchSampleData.json")));
        }

        static string GetSampleJsonResponse(string file)
        {
            var json = File.ReadAllText(file);
            return json;
        }
    }
}
