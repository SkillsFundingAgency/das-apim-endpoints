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
            GetCourseLookupResponse courseResponse,
            [Frozen] Mock<ICachedLocationLookupService> cachedLocationLookupService,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICachedStandardDetailsService> cachedStandardDetailsService,
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

            cachedStandardDetailsService
                .Setup(client => client.GetStandardDetails(query.LarsCode))
                .ReturnsAsync(courseResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(apiResponse, options => options.Excluding(x => x.LarsCode).Excluding(x => x.CourseDates));
                result.LarsCode.Should().BeEquivalentTo(apiResponse.LarsCode.ToString());
                result.CourseDates.Should().Be(courseResponse.CourseDates);
                result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            }
        }

        [Test, MoqAutoData]
        public async Task WhenHandle_AndNoLocationMatched_ThenReturnsExpectedResponse(
            GetCourseProvidersQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICachedLocationLookupService> cachedLocationLookupService,
            [Frozen] Mock<ICachedStandardDetailsService> cachedStandardDetailsService,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler,
            GetCourseLookupResponse courseResponse,
            CancellationToken cancellationToken
        )
        {
            cachedLocationLookupService.Setup(
                client => client.GetCachedLocationInformation(query.LocationName, false))
            .ReturnsAsync((LocationItem)null);

            cachedStandardDetailsService.Setup(
                client => client.GetStandardDetails(query.LarsCode))
            .ReturnsAsync(courseResponse);

            var expectedResponse = new GetCourseProvidersResponse
            {
                PageSize = 10,
                Page = 1,
                LarsCode = query.LarsCode,
                Providers = [],
                QarPeriod = string.Empty,
                ReviewPeriod = string.Empty,
                StandardName = $"{courseResponse.Title} (level {courseResponse.Level})",
                CourseDates = courseResponse.CourseDates,
                TotalCount = 0,
                TotalPages = 0
            };

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(expectedResponse);
                cachedStandardDetailsService.Verify(x => x.GetStandardDetails(query.LarsCode), Times.Once);
                courseManagementApiMock.Verify(x => x.GetWithResponseCode<GetCourseProvidersResponse>(It.IsAny<GetProvidersByCourseIdRequest>()), Times.Never);
            }
        }


        [Test, MoqAutoData]
        public async Task WhenHandle_AndNoLocationEntered_ThenCallsCourseManagementApi(
            GetCourseProvidersResponse apiResponse,
            GetCourseProvidersQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ILocationLookupService> locationLookupServiceMock,
            [Frozen] Mock<ICachedStandardDetailsService> cachedStandardDetailsService,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler,
            GetCourseLookupResponse courseResponse,
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

            cachedStandardDetailsService
                .Setup(client => client.GetStandardDetails(query.LarsCode))
                .ReturnsAsync(courseResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(apiResponse, options => options.Excluding(x => x.LarsCode).Excluding(x => x.CourseDates));
                result.LarsCode.Should().BeEquivalentTo(apiResponse.LarsCode.ToString());
                result.CourseDates.Should().Be(courseResponse.CourseDates);
                cachedStandardDetailsService.Verify(x => x.GetStandardDetails(query.LarsCode), Times.Once);
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
            [Frozen] Mock<ICachedStandardDetailsService> cachedStandardDetailsService,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler,
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

            using (new AssertionScope())
            {
                result.Should().BeNull();
                cachedStandardDetailsService.Verify(x => x.GetStandardDetails(It.IsAny<string>()), Times.Never);
            }
        }

        [Test, MoqAutoData]
        public async Task WhenHandle_AndLocationMatched_ThenForwardsCoordinatesAndSetsCourseDates(
            GetCourseProvidersResponse apiResponse,
            GetCourseProvidersQuery query,
            GetCourseLookupResponse courseResponse,
            [Frozen] Mock<ICachedLocationLookupService> cachedLocationLookupService,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICachedStandardDetailsService> cachedStandardDetailsService,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler)
        {
            query.LocationName = "Leeds";
            var location = new LocationItem(query.LocationName, [51.5, -1.5], "England");

            cachedLocationLookupService
                .Setup(client => client.GetCachedLocationInformation(query.LocationName, false))
                .ReturnsAsync(location);

            cachedStandardDetailsService
                .Setup(client => client.GetStandardDetails(query.LarsCode))
                .ReturnsAsync(courseResponse);

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
                result.CourseDates.Should().Be(courseResponse.CourseDates);
                courseManagementApiMock.Verify(x => x.GetWithResponseCode<GetCourseProvidersResponse>(It.Is<GetProvidersByCourseIdRequest>(
                    c => c.Latitude == location.Latitude && c.Longitude == location.Longitude)), Times.Once);
            }
        }

        [Test, MoqAutoData]
        public async Task WhenHandle_AndApiReturnsNonSuccess_ThenReturnsEmptyResponse(
            GetCourseProvidersResponse apiResponse,
            GetCourseProvidersQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICachedStandardDetailsService> cachedStandardDetailsService,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler)
        {
            query.LocationName = null;

            courseManagementApiMock
                .Setup(client => client.GetWithResponseCode<GetCourseProvidersResponse>(It.IsAny<GetProvidersByCourseIdRequest>()))
                .ReturnsAsync(new ApiResponse<GetCourseProvidersResponse>(apiResponse, HttpStatusCode.InternalServerError, "Error"));

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(new GetCourseProvidersResponse());
                result.Should().NotBeSameAs(apiResponse);
                cachedStandardDetailsService.Verify(x => x.GetStandardDetails(It.IsAny<string>()), Times.Never);
            }
        }

        [Test, MoqAutoData]
        public async Task WhenHandle_AndSuccessWithNullBody_ThenReturnsNull(
            GetCourseProvidersQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICachedStandardDetailsService> cachedStandardDetailsService,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler)
        {
            query.LocationName = null;

            courseManagementApiMock
                .Setup(client => client.GetWithResponseCode<GetCourseProvidersResponse>(It.IsAny<GetProvidersByCourseIdRequest>()))
                .ReturnsAsync(new ApiResponse<GetCourseProvidersResponse>(null, HttpStatusCode.OK, ""));

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeNull();
                cachedStandardDetailsService.Verify(x => x.GetStandardDetails(It.IsAny<string>()), Times.Never);
            }
        }

        [Test, MoqAutoData]
        public async Task WhenHandle_AndCourseBodyIsNull_ThenClearsCourseDates(
            GetCourseProvidersResponse apiResponse,
            GetCourseProvidersQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICachedStandardDetailsService> cachedStandardDetailsService,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler)
        {
            query.LocationName = null;

            cachedStandardDetailsService
                .Setup(client => client.GetStandardDetails(query.LarsCode))
                .ReturnsAsync((GetCourseLookupResponse)null);

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
