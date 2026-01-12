using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourses;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourses;

public sealed class WhenGettingCourses
{
    [Test, MoqAutoData]
    public async Task Then_Calls_CoursesApi_With_Correct_Request(
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<ILocationLookupService> _locationLookupService,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpApiClient,
        [Greedy] GetCoursesQueryHandler sut,
        LocationItem locationItem,
        GetStandardsListResponse coursesResponse,
        GetCourseTrainingProvidersCountResponse roatpResponse,
        ApprenticeshipType apprenticeshipType,
        CancellationToken cancellationToken
    )
    {
        GetCoursesQuery query = new GetCoursesQuery()
        {
            Keyword = "Construction",
            RouteIds = [1],
            Levels = [2],
            Distance = 40,
            Location = "SW1",
            ApprenticeshipType = apprenticeshipType.ToString()
        };

        coursesApiClient
            .Setup(x => x.GetWithResponseCode<GetStandardsListResponse>(
                It.Is<GetActiveStandardsListRequest>(a =>
                    a.Keyword.Equals(query.Keyword) &&
                    a.OrderBy.Equals(query.OrderBy) &&
                    a.Levels.SequenceEqual(query.Levels) &&
                    a.RouteIds.SequenceEqual(query.RouteIds) &&
                    a.ApprenticeshipType.Equals(query.ApprenticeshipType)
                )
             ))
            .ReturnsAsync(
                new ApiResponse<GetStandardsListResponse>(
                    coursesResponse,
                    HttpStatusCode.OK,
                    string.Empty
            ));

        roatpApiClient
            .Setup(x =>
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.IsAny<GetCourseTrainingProvidersCountRequest>()
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    roatpResponse,
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        await sut.Handle(query, cancellationToken);

        coursesApiClient.Verify(x =>
            x.GetWithResponseCode<GetStandardsListResponse>(
                It.Is<GetActiveStandardsListRequest>(r =>
                    r.Keyword == (query.Keyword ?? string.Empty) &&
                    r.OrderBy == query.OrderBy &&
                    r.RouteIds == query.RouteIds &&
                    r.Levels == query.Levels &&
                    r.ApprenticeshipType == query.ApprenticeshipType)
                ),
                Times.Once
        );
    }

    [Test, MoqAutoData]
    public async Task When_Location_Is_Set_And_Location_Item_Is_Null_Then_Empty_Response_Is_Returned(
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<ILocationLookupService> _locationLookupService,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpApiClient,
        [Greedy] GetCoursesQueryHandler sut,
        GetStandardsListResponse coursesResponse,
        GetCourseTrainingProvidersCountResponse roatpResponse,
        CancellationToken cancellationToken
    )
    {
        GetCoursesQuery query = new GetCoursesQuery()
        {
            Keyword = "Construction",
            RouteIds = [1],
            Levels = [2],
            Distance = 40,
            Location = "SW1"
        };

        _locationLookupService
            .Setup(a =>
                a.GetLocationInformation(query.Location, 0, 0, false)
            )
            .ReturnsAsync((LocationItem)null);

        coursesApiClient
            .Setup(x => x.GetWithResponseCode<GetStandardsListResponse>(
                It.Is<GetActiveStandardsListRequest>(a =>
                    a.Keyword.Equals(query.Keyword) &&
                    a.OrderBy.Equals(query.OrderBy) &&
                    a.Levels.SequenceEqual(query.Levels) &&
                    a.RouteIds.SequenceEqual(query.RouteIds)
                )
             ))
            .ReturnsAsync(
                new ApiResponse<GetStandardsListResponse>(
                    coursesResponse,
                    HttpStatusCode.OK,
                    string.Empty
            ));

        roatpApiClient
            .Setup(x =>
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.IsAny<GetCourseTrainingProvidersCountRequest>()
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    roatpResponse,
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        var result = await sut.Handle(query, cancellationToken);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Page, Is.EqualTo(1));
            Assert.That(result.PageSize, Is.EqualTo(query.PageSize));
            Assert.That(result.TotalPages, Is.EqualTo(0));
            Assert.That(result.TotalCount, Is.EqualTo(0));
            Assert.That(result.Standards, Has.Count.EqualTo(0));
        });

        coursesApiClient.Verify(x =>
            x.GetWithResponseCode<GetStandardsListResponse>(
                    It.IsAny<GetActiveStandardsListRequest>()
                ),
            Times.Never
        );
    }

    [Test, MoqAutoData]
    public async Task When_Courses_Api_Returns_No_Standards_Then_Empty_Response_Is_Returned(
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<ILocationLookupService> _locationLookupService,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpApiClient,
        [Greedy] GetCoursesQueryHandler sut,
        LocationItem locationItem,
        GetStandardsListResponse coursesResponse,
        GetCourseTrainingProvidersCountResponse roatpResponse,
        CancellationToken cancellationToken
    )
    {
        coursesResponse.Standards = [];

        GetCoursesQuery query = new GetCoursesQuery()
        {
            Keyword = "Construction",
            RouteIds = [1],
            Levels = [2],
            Distance = 40,
            Location = "SW1"
        };

        _locationLookupService
            .Setup(a =>
                a.GetLocationInformation(query.Location, 0, 0, false)
            )
            .ReturnsAsync(locationItem);

        coursesApiClient
            .Setup(x => x.GetWithResponseCode<GetStandardsListResponse>(
                It.Is<GetActiveStandardsListRequest>(a =>
                    a.Keyword.Equals(query.Keyword) &&
                    a.OrderBy.Equals(query.OrderBy) &&
                    a.Levels.SequenceEqual(query.Levels) &&
                    a.RouteIds.SequenceEqual(query.RouteIds)
                )
             ))
            .ReturnsAsync(
                new ApiResponse<GetStandardsListResponse>(
                    coursesResponse,
                    HttpStatusCode.OK,
                    string.Empty
            ));

        roatpApiClient
            .Setup(x =>
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.IsAny<GetCourseTrainingProvidersCountRequest>()
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    roatpResponse,
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        var result = await sut.Handle(query, cancellationToken);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Page, Is.EqualTo(1));
            Assert.That(result.PageSize, Is.EqualTo(query.PageSize));
            Assert.That(result.TotalPages, Is.EqualTo(0));
            Assert.That(result.TotalCount, Is.EqualTo(0));
            Assert.That(result.Standards, Has.Count.EqualTo(0));
        });

        roatpApiClient.Verify(x =>
            x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.IsAny<GetCourseTrainingProvidersCountRequest>()
                ),
            Times.Never
        );
    }

    [Test, MoqAutoData]
    public async Task Then_Calls_RoatpApi_With_Correct_Request(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpCourseManagementApiClient,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<ILocationLookupService> _locationLookupService,
        [Greedy] GetCoursesQueryHandler sut,
        LocationItem locationItem,
        GetStandardsListResponse coursesResponse,
        GetCourseTrainingProvidersCountResponse providerCountResponse,
        CancellationToken cancellationToken
    )
    {
        GetCoursesQuery query = new GetCoursesQuery()
        {
            Keyword = "Construction",
            RouteIds = [1],
            Levels = [2],
            Distance = 40,
            Location = "SW1"
        };

        var pagedStandards = coursesResponse.Standards
            .Skip(query.Page == 1 ? 0 : query.Page * query.PageSize)
            .Take(query.PageSize)
            .ToArray();

        coursesApiClient
            .Setup(x =>
                x.GetWithResponseCode<GetStandardsListResponse>(
                    It.Is<GetActiveStandardsListRequest>(a =>
                        a.Keyword.Equals(query.Keyword) &&
                        a.OrderBy.Equals(query.OrderBy) &&
                        a.Levels.SequenceEqual(query.Levels) &&
                        a.RouteIds.SequenceEqual(query.RouteIds)
                    )
                )
            )
            .ReturnsAsync(new ApiResponse<GetStandardsListResponse>(
                coursesResponse,
                HttpStatusCode.OK,
                string.Empty
            )
        );

        _locationLookupService
            .Setup(a =>
                a.GetLocationInformation(query.Location, 0, 0, false)
            )
            .ReturnsAsync(locationItem);

        roatpCourseManagementApiClient
            .Setup(x =>
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.IsAny<GetCourseTrainingProvidersCountRequest>()
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    providerCountResponse,
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        await sut.Handle(query, cancellationToken);

        roatpCourseManagementApiClient.Verify(x =>
            x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                It.Is<GetCourseTrainingProvidersCountRequest>(r =>
                        r.LarsCodes.SequenceEqual(pagedStandards.Select(s => s.LarsCode.ToString()).ToArray()) &&
                        r.Distance == query.Distance &&
                        r.Latitude == (decimal?)locationItem.GeoPoint[0] &&
                        r.Longitude == (decimal?)locationItem.GeoPoint[1]
                    )
                ),
                Times.Once
            );
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Correct_Mapped_Result(
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<ILocationLookupService> _locationLookupService,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpCourseManagementApiClient,
        [Greedy] GetCoursesQueryHandler sut,
        LocationItem locationItem,
        GetStandardsListResponse coursesResponse,
        GetCourseTrainingProvidersCountResponse providerCountResponse,
        CancellationToken cancellationToken
    )
    {
        GetCoursesQuery query = new GetCoursesQuery()
        {
            Keyword = "Construction",
            RouteIds = [1],
            Levels = [2],
            Distance = 40,
            Location = "SW1"
        };

        var pagedStandards = coursesResponse.Standards
            .Skip(query.Page == 1 ? 0 : query.Page * query.PageSize)
            .Take(query.PageSize).ToArray();

        _locationLookupService
            .Setup(a =>
                a.GetLocationInformation(query.Location, 0, 0, false)
            )
        .ReturnsAsync(locationItem);

        coursesApiClient
            .Setup(x =>
                x.GetWithResponseCode<GetStandardsListResponse>(
                    It.Is<GetActiveStandardsListRequest>(a =>
                        a.Keyword.Equals(query.Keyword) &&
                        a.OrderBy.Equals(query.OrderBy) &&
                        a.Levels.SequenceEqual(query.Levels) &&
                        a.RouteIds.SequenceEqual(query.RouteIds)
                    )
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetStandardsListResponse>(
                    coursesResponse,
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        roatpCourseManagementApiClient
            .Setup(x =>
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.Is<GetCourseTrainingProvidersCountRequest>(a =>
                        a.LarsCodes.SequenceEqual(pagedStandards.Select(a => a.LarsCode.ToString()).ToArray()) &&
                        a.Distance.Equals(query.Distance) &&
                        a.Latitude.Equals((decimal)locationItem.GeoPoint[0]) &&
                        a.Longitude.Equals((decimal)locationItem.GeoPoint[1])
                    )
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    providerCountResponse,
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        var result = await sut.Handle(query, cancellationToken);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Page, Is.EqualTo(query.Page));
            Assert.That(result.PageSize, Is.EqualTo(query.PageSize));
            Assert.That(result.TotalPages, Is.EqualTo((int)Math.Ceiling((double)coursesResponse.TotalFiltered / query.PageSize)));
            Assert.That(result.TotalCount, Is.EqualTo(coursesResponse.TotalFiltered));
            Assert.That(result.Standards.Count, Is.EqualTo(pagedStandards.Length));
        });
    }

    [Test, MoqAutoData]
    public async Task When_Request_Page_Exceeds_Max_Page_Then_Empty_Response_Is_Returned(
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<ILocationLookupService> _locationLookupService,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpCourseManagementApiClient,
        [Greedy] GetCoursesQueryHandler sut,
        LocationItem locationItem,
        GetStandardsListResponse coursesResponse,
        GetCourseTrainingProvidersCountResponse providerCountResponse,
        CancellationToken cancellationToken
    )
    {
        GetCoursesQuery query = new GetCoursesQuery()
        {
            Keyword = "Construction",
            RouteIds = [1],
            Levels = [2],
            Distance = 40,
            Location = "SW1",
            Page = 43431
        };

        var pagedStandards = coursesResponse.Standards
            .Skip(query.Page == 1 ? 0 : query.Page * query.PageSize)
            .Take(query.PageSize).ToArray();

        _locationLookupService
            .Setup(a =>
                a.GetLocationInformation(query.Location, 0, 0, false)
            )
        .ReturnsAsync(locationItem);

        coursesApiClient
            .Setup(x =>
                x.GetWithResponseCode<GetStandardsListResponse>(
                    It.Is<GetActiveStandardsListRequest>(a =>
                        a.Keyword.Equals(query.Keyword) &&
                        a.OrderBy.Equals(query.OrderBy) &&
                        a.Levels.SequenceEqual(query.Levels) &&
                        a.RouteIds.SequenceEqual(query.RouteIds)
                    )
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetStandardsListResponse>(
                    coursesResponse,
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        roatpCourseManagementApiClient
            .Setup(x =>
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.Is<GetCourseTrainingProvidersCountRequest>(a =>
                        a.LarsCodes.SequenceEqual(pagedStandards.Select(a => a.LarsCode.ToString()).ToArray()) &&
                        a.Distance.Equals(query.Distance) &&
                        a.Latitude.Equals((decimal)locationItem.GeoPoint[0]) &&
                        a.Longitude.Equals((decimal)locationItem.GeoPoint[1])
                    )
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    providerCountResponse,
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        var _sut = await sut.Handle(query, cancellationToken);

        Assert.Multiple(() =>
        {
            Assert.That(_sut, Is.Not.Null);
            Assert.That(_sut.Page, Is.EqualTo(1));
            Assert.That(_sut.PageSize, Is.EqualTo(query.PageSize));
            Assert.That(_sut.TotalPages, Is.EqualTo(0));
            Assert.That(_sut.TotalCount, Is.EqualTo(0));
            Assert.That(_sut.Standards, Has.Count.EqualTo(0));
        });
    }
}