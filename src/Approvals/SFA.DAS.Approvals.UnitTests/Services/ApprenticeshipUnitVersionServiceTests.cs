using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.Validation;
using SFA.DAS.Approvals.InnerApi.CoursesApi;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.Approvals.UnitTests.Services;

[TestFixture]
public class ApprenticeshipUnitVersionServiceTests
{
    [Test]
    public async Task LooksUpLarsCodeAndResolvesVersionsByUnitReference()
    {
        var courses = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        var course = new GetCourseLookupResponse
        {
            LarsCode = "ZSC00001", IfateReferenceNumber = "AU0001", LearningType = "ApprenticeshipUnit"
        };
        courses.Setup(x => x.Get<GetCourseLookupResponse>(It.Is<GetCourseLookupByIdRequest>(r => r.Id == "ZSC00001")))
            .ReturnsAsync(course);
        courses.Setup(x => x.Get<GetCourseVersionsResponse>(It.Is<GetCourseVersionsRequest>(
                r => r.GetUrl == "api/courses/lookup/versions/AU0001")))
            .ReturnsAsync(new GetCourseVersionsResponse
            {
                Courses = new List<GetCourseLookupResponse> { Version("1.0", 0, new DateTime(2026, 1, 1), null) }
            });
        var service = new ApprenticeshipUnitVersionService(courses.Object);

        var result = await service.Resolve("ZSC00001", new DateTime(2026, 1, 15));

        result.StandardUId.Should().Be("AU0001_1.0");
        courses.Verify(x => x.Get<GetCourseLookupResponse>(It.Is<GetCourseLookupByIdRequest>(
            r => r.GetUrl == "api/courses/lookup/ZSC00001")), Times.Once);
    }

    [TestCase(14, "1.0")]
    [TestCase(15, "1.1")]
    [TestCase(31, "1.1")]
    public async Task ResolvesVersionUsingFullStartDay(int day, string expectedVersion)
    {
        var courses = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        courses.Setup(x => x.Get<GetCourseVersionsResponse>(It.IsAny<GetCourseVersionsRequest>()))
            .ReturnsAsync(new GetCourseVersionsResponse
            {
                Courses = new List<GetCourseLookupResponse>
                {
                    Version("1.0", 0, new DateTime(2026, 1, 1), new DateTime(2026, 1, 14)),
                    Version("1.1", 1, new DateTime(2026, 1, 15), null)
                }
            });
        var service = new ApprenticeshipUnitVersionService(courses.Object);

        var result = await service.Resolve(new GetCourseLookupResponse
        {
            LearningType = "ApprenticeshipUnit", IfateReferenceNumber = "AU0001"
        }, new DateTime(2026, 1, day, 23, 59, 59));

        result.Version.Should().Be(expectedVersion);
        result.StandardUId.Should().Be($"AU0001_{expectedVersion}");
        courses.Verify(x => x.Get<GetCourseVersionsResponse>(It.Is<GetCourseVersionsRequest>(
            request => request.GetUrl == "api/courses/lookup/versions/AU0001")), Times.Once);
    }

    [TestCase(null)]
    [TestCase("Apprenticeship")]
    [TestCase("FoundationApprenticeship")]
    public async Task LeavesOtherCourseTypesOnExistingPath(string learningType)
    {
        var courses = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        var service = new ApprenticeshipUnitVersionService(courses.Object);

        var result = await service.Resolve(new GetCourseLookupResponse { LearningType = learningType }, DateTime.Today);

        result.Should().BeNull();
        courses.VerifyNoOtherCalls();
    }

    [Test]
    public async Task DoesNotStampLatestVersionWhenStartDateIsOutsideAllWindows()
    {
        var courses = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        courses.Setup(x => x.Get<GetCourseVersionsResponse>(It.IsAny<GetCourseVersionsRequest>()))
            .ReturnsAsync(new GetCourseVersionsResponse
            {
                Courses = new List<GetCourseLookupResponse> { Version("1.0", 0, new DateTime(2026, 1, 1), null) }
            });
        var service = new ApprenticeshipUnitVersionService(courses.Object);

        var act = () => service.Resolve(new GetCourseLookupResponse
        {
            LearningType = "ApprenticeshipUnit", IfateReferenceNumber = "AU0001"
        }, new DateTime(2025, 12, 31));

        var error = (await act.Should().ThrowAsync<ApprenticeshipUnitVersionException>()).Which;
        var response = JsonConvert.DeserializeObject<ErrorResponse>(error.Content);
        response.Errors.Should().ContainSingle().Which.Field.Should().Be("ActualStartDate");
        response.Errors[0].Message.Should().Be(error.Message);
    }

    [Test]
    public async Task RequiresActualStartDateForApprenticeshipUnits()
    {
        var courses = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        var service = new ApprenticeshipUnitVersionService(courses.Object);
        var act = () => service.Resolve(new GetCourseLookupResponse { LearningType = "ApprenticeshipUnit" }, null);

        var error = (await act.Should().ThrowAsync<ApprenticeshipUnitVersionException>()).Which;
        var response = JsonConvert.DeserializeObject<ErrorResponse>(error.Content);
        response.Errors.Should().ContainSingle().Which.Field.Should().Be("ActualStartDate");
        response.Errors[0].Message.Should().Be(error.Message);
        courses.VerifyNoOtherCalls();
    }

    [Test]
    public async Task FetchesVersionsSeparatelyForDifferentUnitReferences()
    {
        var courses = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        courses.Setup(x => x.Get<GetCourseVersionsResponse>(It.IsAny<GetCourseVersionsRequest>()))
            .ReturnsAsync(new GetCourseVersionsResponse
            {
                Courses = [Version("1.0", 0, new DateTime(2026, 1, 1), null)]
            });
        var service = new ApprenticeshipUnitVersionService(courses.Object);

        foreach (var reference in new[] { "AU0001", "AU0002", "AU0001" })
        {
            await service.Resolve(new GetCourseLookupResponse
            {
                LearningType = "ApprenticeshipUnit", IfateReferenceNumber = reference
            }, new DateTime(2026, 1, 15));
        }

        courses.Verify(x => x.Get<GetCourseVersionsResponse>(It.Is<GetCourseVersionsRequest>(
            r => r.GetUrl == "api/courses/lookup/versions/AU0001")), Times.Once);
        courses.Verify(x => x.Get<GetCourseVersionsResponse>(It.Is<GetCourseVersionsRequest>(
            r => r.GetUrl == "api/courses/lookup/versions/AU0002")), Times.Once);
    }

    private static GetCourseLookupResponse Version(string version, int minor, DateTime from, DateTime? to) => new()
    {
        Version = version,
        VersionMajor = 1,
        VersionMinor = minor,
        StandardUId = $"AU0001_{version}",
        VersionDetail = new StandardVersionDetail { EarliestStartDate = from, LatestStartDate = to }
    };
}
