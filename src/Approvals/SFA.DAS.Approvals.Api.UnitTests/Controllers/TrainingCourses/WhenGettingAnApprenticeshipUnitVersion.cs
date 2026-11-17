using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.TrainingCourses.Queries;
using SFA.DAS.Approvals.InnerApi.CoursesApi;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.TrainingCourses;

public class WhenGettingAnApprenticeshipUnitVersion
{
    private const string CourseCode = "ZSC00001";
    private const string UnitReference = "AU0001";
    private const string VersionUrl = "trainingcourses/apprenticeship-units/" + CourseCode + "/version";

    [TestCase("2026-11-14", "1.0")]
    [TestCase("2026-11-15", "1.1")]
    public async Task ThenReturnsTheVersionAndStandardUIdEffectiveOnTheRequestedDay(string startDate, string version)
    {
        using var server = CreateServer();
        using var client = server.CreateClient();

        var result = await client.GetFromJsonAsync<GetApprenticeshipUnitVersionResult>($"{VersionUrl}?startDate={startDate}");

        result.Should().Be(new GetApprenticeshipUnitVersionResult(version, $"{UnitReference}_{version}"));
    }

    [TestCase("?startDate=2026-10-31", HttpStatusCode.NotFound)]
    [TestCase("", HttpStatusCode.BadRequest)]
    public async Task ThenReturnsTheExpectedStatusWhenTheVersionCannotBeResolved(string query, HttpStatusCode status)
    {
        using var server = CreateServer();
        using var client = server.CreateClient();

        using var response = await client.GetAsync(VersionUrl + query);

        response.StatusCode.Should().Be(status);
    }

    private static TestServer CreateServer()
    {
        var courses = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        courses.Setup(client => client.Get<GetCourseLookupResponse>(It.Is<GetCourseLookupByIdRequest>(request => request.Id == CourseCode)))
            .ReturnsAsync(new GetCourseLookupResponse { LearningType = "ApprenticeshipUnit", IfateReferenceNumber = UnitReference });
        courses.Setup(client => client.Get<GetCourseVersionsResponse>(
                It.Is<GetCourseVersionsRequest>(request => request.GetUrl == $"api/courses/lookup/versions/{UnitReference}")))
            .ReturnsAsync(new GetCourseVersionsResponse
            {
                Courses =
                [
                    CreateVersion(0, new DateTime(2026, 11, 1), new DateTime(2026, 11, 14)),
                    CreateVersion(1, new DateTime(2026, 11, 15), null)
                ]
            });

        return new TestServer(new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton(courses.Object);
                services.AddTransient<IApprenticeshipUnitVersionService, ApprenticeshipUnitVersionService>();
                services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetApprenticeshipUnitVersionQuery).Assembly));
                services.AddControllers().AddApplicationPart(typeof(TrainingCoursesController).Assembly);
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseEndpoints(endpoints => endpoints.MapControllers());
            }));
    }

    private static GetCourseLookupResponse CreateVersion(int minor, DateTime earliest, DateTime? latest)
    {
        return new GetCourseLookupResponse
        {
            StandardUId = $"{UnitReference}_1.{minor}",
            Version = $"1.{minor}",
            VersionMajor = 1,
            VersionMinor = minor,
            VersionDetail = new StandardVersionDetail { EarliestStartDate = earliest, LatestStartDate = latest }
        };
    }
}
