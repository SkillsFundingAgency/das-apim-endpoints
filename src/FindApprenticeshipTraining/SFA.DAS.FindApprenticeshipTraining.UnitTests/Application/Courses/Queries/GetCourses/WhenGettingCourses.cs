using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.GetCourses;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourses;

public sealed class WhenGettingCourses
{
    [Test, MoqAutoData]
    public async Task Then_Calls_CoursesApi_With_Correct_Request(
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpApiClient,
        [Greedy] GetCoursesQueryHandler sut,
        GetStandardsListResponse coursesResponse,
        GetCourseTrainingProvidersCountResponse roatpResponse,
        CancellationToken cancellationToken
    )
    {
        GetCoursesQuery query = new GetCoursesQuery();

        coursesApiClient
            .Setup(x => x.GetWithResponseCode<GetStandardsListResponse>(
                It.IsAny<GetAvailableToStartStandardsListRequest>()
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
                It.Is<GetAvailableToStartStandardsListRequest>(r =>
                    r.Keyword == (query.Keyword ?? string.Empty) &&
                    r.OrderBy == query.OrderBy &&
                    r.RouteIds == query.RouteIds &&
                    r.Levels == query.Levels)
                ), 
                Times.Once
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
        GetCoursesQuery query = new GetCoursesQuery();

        var pagedStandards = coursesResponse.Standards
            .Skip(query.Page == 1 ? 0 : query.Page * query.PageSize)
            .Take(query.PageSize)
            .ToArray();

        coursesApiClient
            .Setup(x => 
                x.GetWithResponseCode<GetStandardsListResponse>(
                    It.IsAny<GetAvailableToStartStandardsListRequest>()
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
                        r.LarsCodes.SequenceEqual(pagedStandards.Select(s => s.LarsCode).ToArray()) &&
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
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpCourseManagementApiClient,
        [Greedy] GetCoursesQueryHandler sut,
        GetStandardsListResponse coursesResponse,
        GetCourseTrainingProvidersCountResponse providerCountResponse,
        CancellationToken cancellationToken
    )
    {
        GetCoursesQuery query = new GetCoursesQuery();

        var pagedStandards = coursesResponse.Standards
            .Skip(query.Page == 1 ? 0 : query.Page * query.PageSize)
            .Take(query.PageSize).ToArray();

        coursesApiClient
            .Setup(x =>
                x.GetWithResponseCode<GetStandardsListResponse>(
                    It.IsAny<GetAvailableToStartStandardsListRequest>()
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetStandardsListResponse>(
                    coursesResponse,
                    HttpStatusCode.OK, string.Empty
                )
            );

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
}
