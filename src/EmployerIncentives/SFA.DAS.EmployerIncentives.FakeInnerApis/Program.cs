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
        private static WireMockServer _fakeAccountsApi;
        private static WireMockServer _fakeCustomerEngagementApi;
        private static long _accountId = 100;
        private static long _accountLegalEntityId = 2000;
        private static long _legalEntityId = 2000;

        private static string _customerEngagementCompanyName = "ESFA";
        private static string _hashedLegalEntityId = "JRML7V"; // Legal entity id 1
        private static string _caseId = "AF1042409";

        private static string _hashedAccountId = "LVX89V"; // Account ID 100

        static void Main(string[] args)
        {
            try
            {
                _fakeCommitmentsApi = WireMockServer.Start(new FluentMockServerSettings
                {
                    Urls = new[] {"http://*:6011"},
                    StartAdminInterface = true,
                });

                _fakeAccountsApi = WireMockServer.Start(new FluentMockServerSettings
                {
                    Urls = new[] { "http://*:6012" },
                    StartAdminInterface = true,
                });

                _fakeCustomerEngagementApi = WireMockServer.Start(new FluentMockServerSettings
                {
                    Urls = new[] { "http://*:6013" },
                    StartAdminInterface = true,
                });

                SetupHealthCheckResponse();
                SetupApprenticeshipSearchResponses();
                SetupGetApprenticeshipResponses();

                SetupGetLegalEntityResponse();

                SetupGetVendorDetailsResponse();
                SetupGetVendorCaseStatusResponse();

                Console.WriteLine(("Please RETURN to stop server"));
                Console.ReadLine();
            }
            finally
            {
                _fakeCommitmentsApi.Stop();
                _fakeCommitmentsApi.Dispose();

                _fakeAccountsApi.Stop();
                _fakeAccountsApi.Dispose();

                _fakeCustomerEngagementApi.Stop();
                _fakeCustomerEngagementApi.Dispose();
            }
        }

        private static void SetupGetVendorDetailsResponse()
        {
            // Return sample values for Account 100 and ALE 2000 
            _fakeCustomerEngagementApi.Given(
                    Request.Create().WithPath($"/Finance/{_customerEngagementCompanyName}/vendor/aleid={_hashedLegalEntityId}")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(GetSampleJsonResponse("VendorData.json")));
        }

        private static void SetupGetVendorCaseStatusResponse()
        {
            _fakeCustomerEngagementApi.Given(
                    Request.Create().WithPath($"/Finance/Registrations/{_caseId}")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(GetSampleJsonResponse("VendorStatus.json")));
        }

        static void SetupHealthCheckResponse()
        {
            _fakeCommitmentsApi.Given(
                    Request.Create().WithPath("/api/ping")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int) HttpStatusCode.OK)
                    );

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
                        .WithParam("startDateRangeFrom", true)
                        .WithParam("startDateRangeTo", true)
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(GetSampleJsonResponse("ApprenticeshipSearchSampleData.json")));
        }

        static void SetupGetApprenticeshipResponses()
        {
            _fakeCommitmentsApi.Given(
                    Request.Create().WithPath("/api/apprenticeships/20002")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(GetSampleJsonResponse("Apprenticeship20002Data.json")));

            _fakeCommitmentsApi.Given(
                    Request.Create().WithPath("/api/apprenticeships/5")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(GetSampleJsonResponse("Apprenticeship5Data.json")));
        }

        static void SetupGetLegalEntityResponse()
        {
            _fakeAccountsApi.Given(
                    Request.Create().WithPath($"/api/accounts/{_hashedAccountId}/legalentities/{_legalEntityId}")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(GetSampleJsonResponse("LegalEntityData.json")));
        }

        static string GetSampleJsonResponse(string file)
        {
            var json = File.ReadAllText(file);
            return json;
        }
    }
}
