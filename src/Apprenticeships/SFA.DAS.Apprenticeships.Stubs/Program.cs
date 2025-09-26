using System.Net;
using System.Text.Json;
using AutoFixture;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.Apprenticeships.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace SFA.DAS.Apprenticeships.Stubs
{   
    public class Program
    {
        private static WireMockServer _fakeCoursesApi;
        private static WireMockServer _fakeApprenticeshipsApi;
        private static WireMockServer _fakeCommitmentsApi;
        private static WireMockServer _fakeAccountsApi;
        private static WireMockServer _fakeEmployerProfilesApi;
        private static WireMockServer _fakeCollectionCalendarApi;

        static void Main(string[] args)
        {
            try
            {
                _fakeCoursesApi = WireMockServer.Start(new WireMockServerSettings
                {
                    Urls = new[] { "http://*:6011" },
                    StartAdminInterface = true,
                });
                SetUpCoursesResponses();

                _fakeApprenticeshipsApi = WireMockServer.Start(new WireMockServerSettings
                {
                    Urls = new[] { "http://*:6012" },
                    StartAdminInterface = true,
                });
                SetUpApprenticeshipResponses();

                _fakeCommitmentsApi = WireMockServer.Start(new WireMockServerSettings
                {
                    Urls = new[] { "http://*:6013" },
                    StartAdminInterface = true,
                });
                SetUpCommitmentsResponses();

                _fakeAccountsApi = WireMockServer.Start(new WireMockServerSettings
                {
                    Urls = new[] { "http://*:6014" },
                    StartAdminInterface = true,
                });
                SetUpAccountsResponses();

                _fakeEmployerProfilesApi = WireMockServer.Start(new WireMockServerSettings
                {
                    Urls = new[] { "http://*:6015" },
                    StartAdminInterface = true,
                });
                SetUpEmployerProfilesResponses();

                _fakeCollectionCalendarApi = WireMockServer.Start(new WireMockServerSettings
                {
                    Urls = new[] { "http://*:6016" },
                    StartAdminInterface = true,
                });
                SetupCollectionCalendarResponses();

                Console.WriteLine(("Please RETURN to stop server"));
                Console.ReadLine();
            }
            finally
            {
                _fakeCoursesApi.Stop();
                _fakeCoursesApi.Dispose();
                _fakeApprenticeshipsApi.Stop();
                _fakeApprenticeshipsApi.Dispose();
                _fakeCommitmentsApi.Stop();
                _fakeCommitmentsApi.Dispose();
                _fakeAccountsApi.Stop();
                _fakeAccountsApi.Dispose();
                _fakeEmployerProfilesApi.Stop();
                _fakeEmployerProfilesApi.Dispose();
                _fakeCollectionCalendarApi.Stop();
                _fakeCollectionCalendarApi.Dispose();
            }
        }

        private static void SetUpCoursesResponses()
        {
            _fakeCoursesApi.Given(
                    Request.Create().WithPath($"/api/courses/standards/*")
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(new Fixture().Create<GetStandardsListItem>())));
        }

        private static void SetupCollectionCalendarResponses()
        {
            _fakeCollectionCalendarApi.Given(
                    Request.Create().WithPath($"/academicyears")
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(new Fixture().Create<GetAcademicYearsResponse>())));
        }

        private static void SetUpCommitmentsResponses()
        {
            _fakeCommitmentsApi.Given(
                    Request.Create().WithPath($"/api/AccountLegalEntity/*")
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(new Fixture().Create<GetAccountLegalEntityResponse>())));

            _fakeCommitmentsApi.Given(
                    Request.Create().WithPath($"/api/providers/*")
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(new Fixture().Create<GetProviderResponse>())));

            _fakeCommitmentsApi.Given(
                    Request.Create().WithPath($"/api/trainingprogramme/*/versions")
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(new Fixture().Create<GetTrainingProgrammeVersionsResponse>())));
        }

        private static void SetUpApprenticeshipResponses()
        {
            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/key")
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(new Fixture().Create<Guid>())));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/price")
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(new Fixture().Create<GetLearningPriceResponse>())));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/startDate")
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(new Fixture().Create<GetLearningStartDateResponse>())));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/priceHistory").UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(new Fixture().Create<PostCreateApprenticeshipPriceChangeApiResponse>())));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/startDateChange").UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody("{}"));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/priceHistory/pending").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(new Fixture().Create<GetPendingPriceChangeApiResponse>())));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/startDateChange/pending").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(new Fixture().Create<GetPendingStartDateChangeApiResponse>())));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/priceHistory/pending").UsingDelete()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody("{}"));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/priceHistory/pending/reject").UsingPatch()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody("{}"));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/priceHistory/pending").UsingPatch()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody("{}"));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/startDateChange/pending").UsingDelete()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody("{}"));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/startDateChange/reject").UsingPatch()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody("{}"));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/startDateChange/pending").UsingPatch()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody("{}"));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/freeze").UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody("{}"));

            _fakeApprenticeshipsApi.Given(
                    Request.Create().WithPath($"/*/unfreeze").UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody("{}"));
        }

        private static void SetUpAccountsResponses()
        {
            _fakeAccountsApi.Given(
                    Request.Create().WithPath($"/api/user/*/accounts")
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(new Fixture().CreateMany<GetUserAccountsResponse>())));
        }

        private static void SetUpEmployerProfilesResponses()
        {
            _fakeEmployerProfilesApi.Given(
                    Request.Create().WithPath($"/api/users/*")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(new Fixture().Build<EmployerProfileUsersApiResponse>().With(x => x.Id, Guid.NewGuid().ToString).Create())));
        }
    }
}