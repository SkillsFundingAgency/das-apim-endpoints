using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseProviders
{
    public class GetTrainingCourseProvidersQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task WhenHandle_AndNoLocationEntered_ThenReturnsExpectedResponse(
            GetCourseProvidersResponse apiResponse,
            GetCourseProvidersQuery query,
            GetStandardsListItem standardResponse,
            [Frozen] Mock<ICachedLocationLookupService> cachedLocationLookupService,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiMock,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler,
            CancellationToken cancellationToken
            )
        {
            query.LocationName = null;

            courseManagementApiMock
                .Setup(client => client.GetWithResponseCode<GetCourseProvidersResponse>(It.Is<GetProvidersByCourseIdRequest>(
                    c => c.GetUrl.Contains(query.LarsCode)
                      && c.GetUrl.Contains($"api/courses/{query.LarsCode}/providers")
                    )
                ))
                .ReturnsAsync(new ApiResponse<GetCourseProvidersResponse>(apiResponse,
                    HttpStatusCode.OK, ""));

            coursesApiMock
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetCourseLookupDetailsByIdRequest>(x => x.Id == query.LarsCode)))
                .ReturnsAsync(standardResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(apiResponse, options => options.Excluding(x => x.LarsCode).Excluding(x => x.CourseDates));
                result.LarsCode.Should().BeEquivalentTo(apiResponse.LarsCode.ToString());
                result.CourseDates.Should().Be(standardResponse.CourseDates);
                result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            }
        }

        [Test, MoqAutoData]
        public async Task WhenHandle_AndNoLocationMatched_ThenReturnsExpectedResponse(
            GetCourseProvidersQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICachedLocationLookupService> cachedLocationLookupService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiMock,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler,
            GetStandardsListItem standardResponse,
            CancellationToken cancellationToken
        )
        {
            cachedLocationLookupService.Setup(
                client => client.GetCachedLocationInformation(query.LocationName, false))
            .ReturnsAsync((LocationItem)null);

            coursesApiMock.Setup(
                client => client.Get<GetStandardsListItem>(It.Is<GetCourseLookupDetailsByIdRequest>(x => x.Id == query.LarsCode)))
            .ReturnsAsync(standardResponse);

            var expectedResponse = new GetCourseProvidersResponse
            {
                PageSize = 10,
                Page = 1,
                LarsCode = query.LarsCode,
                Providers = [],
                QarPeriod = string.Empty,
                ReviewPeriod = string.Empty,
                StandardName = $"{standardResponse.Title} (level {standardResponse.Level})",
                CourseDates = standardResponse.CourseDates,
                TotalCount = 0,
                TotalPages = 0
            };

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(expectedResponse);
                coursesApiMock.Verify(x => x.Get<GetStandardsListItem>(It.Is<GetCourseLookupDetailsByIdRequest>(x => x.Id == query.LarsCode)), Times.Once);
                courseManagementApiMock.Verify(x => x.GetWithResponseCode<GetCourseProvidersResponse>(It.IsAny<GetProvidersByCourseIdRequest>()), Times.Never);
            }
        }


        [Test, MoqAutoData]
        public async Task WhenHandle_AndNoLocationEntered_ThenCallsCourseManagementApi(
            GetCourseProvidersResponse apiResponse,
            GetCourseProvidersQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ILocationLookupService> locationLookupServiceMock,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiMock,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler,
            GetStandardsListItem standardResponse,
            CancellationToken cancellationToken
        )
        {
            query.LocationName = null;

            courseManagementApiMock
                .Setup(client => client.GetWithResponseCode<GetCourseProvidersResponse>(It.Is<GetProvidersByCourseIdRequest>(
                        c => c.GetUrl.Contains(query.LarsCode.ToString())
                             && c.GetUrl.Contains($"api/courses/{query.LarsCode}/providers")
                    )
                ))
                .ReturnsAsync(new ApiResponse<GetCourseProvidersResponse>(apiResponse,
                    HttpStatusCode.OK, ""));

            coursesApiMock
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetCourseLookupDetailsByIdRequest>(x => x.Id == query.LarsCode)))
                .ReturnsAsync(standardResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(apiResponse, options => options.Excluding(x => x.LarsCode).Excluding(x => x.CourseDates));
                result.LarsCode.Should().BeEquivalentTo(apiResponse.LarsCode.ToString());
                result.CourseDates.Should().Be(standardResponse.CourseDates);
                coursesApiMock.Verify(x => x.Get<GetStandardsListItem>(It.Is<GetCourseLookupDetailsByIdRequest>(x => x.Id == query.LarsCode)), Times.Once);
                courseManagementApiMock.Verify(x => x.GetWithResponseCode<GetCourseProvidersResponse>(It.IsAny<GetProvidersByCourseIdRequest>()), Times.Once);
                courseManagementApiMock.Verify(x => x.Get<GetAcademicYearsLatestQueryResponse>(It.IsAny<GetAcademicYearsLatestRequest>()), Times.Never);
            }
        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        public async Task WhenHandle_AndBadRequestOrNotFound_ThenReturnsNull(
            HttpStatusCode statusCode,
            GetCourseProvidersQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler,
            GetStandardsListItem standardResponse,
            CancellationToken cancellationToken
        )
        {
            query.LocationName = null;

            courseManagementApiMock
                .Setup(client => client.GetWithResponseCode<GetCourseProvidersResponse>(It.IsAny<GetProvidersByCourseIdRequest>()
                    )
                )
                 .ReturnsAsync(new ApiResponse<GetCourseProvidersResponse>(null, statusCode, "Error"));

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task WhenHandle_AndLocationMatched_ThenForwardsCoordinatesAndSetsCourseDates(
            GetCourseProvidersResponse apiResponse,
            GetCourseProvidersQuery query,
            GetStandardsListItem standardResponse,
            [Frozen] Mock<ICachedLocationLookupService> cachedLocationLookupService,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiMock,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler)
        {
            query.LocationName = "Leeds";
            var location = new LocationItem(query.LocationName, [51.5, -1.5], "England");

            cachedLocationLookupService
                .Setup(client => client.GetCachedLocationInformation(query.LocationName, false))
                .ReturnsAsync(location);

            coursesApiMock
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetCourseLookupDetailsByIdRequest>(x => x.Id == query.LarsCode)))
                .ReturnsAsync(standardResponse);

            courseManagementApiMock
                .Setup(client => client.GetWithResponseCode<GetCourseProvidersResponse>(It.Is<GetProvidersByCourseIdRequest>(
                    c => c.CourseId == query.LarsCode
                         && c.Latitude == location.Latitude
                         && c.Longitude == location.Longitude
                         && c.LocationName == query.LocationName)))
                .ReturnsAsync(new ApiResponse<GetCourseProvidersResponse>(apiResponse, HttpStatusCode.OK, ""));

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeSameAs(apiResponse);
                result.CourseDates.Should().Be(standardResponse.CourseDates);
                courseManagementApiMock.Verify(x => x.GetWithResponseCode<GetCourseProvidersResponse>(It.Is<GetProvidersByCourseIdRequest>(
                    c => c.Latitude == location.Latitude && c.Longitude == location.Longitude)), Times.Once);
            }
        }

        [Test, MoqAutoData]
        public async Task WhenHandle_AndNoLocationMatchedAndCourseMissing_ThenReturnsEmptyStandardDetails(
            GetCourseProvidersQuery query,
            [Frozen] Mock<ICachedLocationLookupService> cachedLocationLookupService,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiMock,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler)
        {
            query.LocationName = "Unknown place";

            cachedLocationLookupService
                .Setup(client => client.GetCachedLocationInformation(query.LocationName, false))
                .ReturnsAsync((LocationItem)null);

            coursesApiMock
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetCourseLookupDetailsByIdRequest>(x => x.Id == query.LarsCode)))
                .ReturnsAsync((GetStandardsListItem)null);

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(new GetCourseProvidersResponse
                {
                    PageSize = 10,
                    Page = 1,
                    LarsCode = query.LarsCode,
                    Providers = [],
                    QarPeriod = string.Empty,
                    ReviewPeriod = string.Empty,
                    StandardName = string.Empty,
                    CourseDates = null,
                    TotalCount = 0,
                    TotalPages = 0
                });
                courseManagementApiMock.Verify(x => x.GetWithResponseCode<GetCourseProvidersResponse>(It.IsAny<GetProvidersByCourseIdRequest>()), Times.Never);
            }
        }

        [Test, MoqAutoData]
        public async Task WhenHandle_AndApiReturnsNonSuccess_ThenReturnsEmptyResponse(
            GetCourseProvidersResponse apiResponse,
            GetCourseProvidersQuery query,
            GetStandardsListItem standardResponse,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiMock,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler)
        {
            query.LocationName = null;

            coursesApiMock
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetCourseLookupDetailsByIdRequest>(x => x.Id == query.LarsCode)))
                .ReturnsAsync(standardResponse);

            courseManagementApiMock
                .Setup(client => client.GetWithResponseCode<GetCourseProvidersResponse>(It.IsAny<GetProvidersByCourseIdRequest>()))
                .ReturnsAsync(new ApiResponse<GetCourseProvidersResponse>(apiResponse, HttpStatusCode.InternalServerError, "Error"));

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(new GetCourseProvidersResponse());
                result.Should().NotBeSameAs(apiResponse);
            }
        }

        [Test, MoqAutoData]
        public async Task WhenHandle_AndSuccessWithNullBody_ThenReturnsNull(
            GetCourseProvidersQuery query,
            GetStandardsListItem standardResponse,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiMock,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler)
        {
            query.LocationName = null;

            coursesApiMock
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetCourseLookupDetailsByIdRequest>(x => x.Id == query.LarsCode)))
                .ReturnsAsync(standardResponse);

            courseManagementApiMock
                .Setup(client => client.GetWithResponseCode<GetCourseProvidersResponse>(It.IsAny<GetProvidersByCourseIdRequest>()))
                .ReturnsAsync(new ApiResponse<GetCourseProvidersResponse>(null, HttpStatusCode.OK, ""));

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task WhenHandle_AndCourseMissing_ThenClearsCourseDates(
            GetCourseProvidersResponse apiResponse,
            GetCourseProvidersQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiMock,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler)
        {
            query.LocationName = null;

            coursesApiMock
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetCourseLookupDetailsByIdRequest>(x => x.Id == query.LarsCode)))
                .ReturnsAsync((GetStandardsListItem)null);

            courseManagementApiMock
                .Setup(client => client.GetWithResponseCode<GetCourseProvidersResponse>(It.Is<GetProvidersByCourseIdRequest>(
                    c => c.GetUrl.Contains(query.LarsCode))))
                .ReturnsAsync(new ApiResponse<GetCourseProvidersResponse>(apiResponse, HttpStatusCode.OK, ""));

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeSameAs(apiResponse);
                result.CourseDates.Should().BeNull();
            }
        }
    }
}