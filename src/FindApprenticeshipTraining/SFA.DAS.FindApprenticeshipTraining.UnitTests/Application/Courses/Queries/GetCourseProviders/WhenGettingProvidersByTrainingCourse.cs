using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using GetStandardRequest = SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests.GetStandardRequest;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseProviders
{
    public class WhenGettingProvidersByTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Handle_ReturnsExpectedReponse(
            GetCourseProvidersResponseFromCourseApi apiResponse,
            GetCourseProvidersQuery query,
            [Frozen] Mock<ICachedLocationLookupService> cachedLocationLookupService,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Greedy] GetCourseProvidersQueryHandler handler,
            CancellationToken cancellationToken
            )
        {
            courseManagementApiMock
                .Setup(client => client.GetWithResponseCode<GetCourseProvidersResponseFromCourseApi>(It.Is<GetProvidersByCourseIdRequest>(
                    c => c.GetUrl.Contains(query.Id)
                      && c.GetUrl.Contains($"api/courses/{query.Id}/providers")
                    )
                ))
                .ReturnsAsync(new ApiResponse<GetCourseProvidersResponseFromCourseApi>(apiResponse,
                    HttpStatusCode.OK, ""));

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(apiResponse, options => options.Excluding(x => x.LarsCode));
            result.LarsCode.Should().BeEquivalentTo(apiResponse.LarsCode.ToString());
        }

        [Test, MoqAutoData]
        public async Task Handler_NoLocationMatched_ReturnsExpectedResponse(
            GetCourseProvidersQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ICachedLocationLookupService> cachedLocationLookupService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiMock,
            [Greedy] GetCourseProvidersQueryHandler handler,
            GetStandardsListItem standardResponse,
            CancellationToken cancellationToken
        )
        {
            cachedLocationLookupService.Setup(
                client => client.GetCachedLocationInformation(query.Location, false))
            .ReturnsAsync((LocationItem)null);

            coursesApiMock.Setup(
                client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(x => x.StandardId == query.Id)))
            .ReturnsAsync(standardResponse);

            var expectedResponse = new GetCourseProvidersResponse
            {
                PageSize = 10,
                Page = 1,
                LarsCode = query.Id,
                Providers = [],
                QarPeriod = string.Empty,
                ReviewPeriod = string.Empty,
                StandardName = $"{standardResponse.Title} (level {standardResponse.Level})",
                TotalCount = 0,
                TotalPages = 0
            };

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(expectedResponse);

            coursesApiMock.Verify(x => x.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(x => x.StandardId == query.Id)), Times.Once);
            courseManagementApiMock.Verify(x => x.GetWithResponseCode<GetCourseProvidersResponse>(It.IsAny<GetProvidersByCourseIdRequest>()), Times.Never);
        }


        [Test, MoqAutoData]
        public async Task Handler_NoLocationEntered_GetExpectedResponse(
            GetCourseProvidersResponseFromCourseApi apiResponse,
            GetCourseProvidersQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Frozen] Mock<ILocationLookupService> locationLookupServiceMock,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiMock,
            [Greedy] GetCourseProvidersQueryHandler handler,
            GetStandardsListItem standardResponse,
            CancellationToken cancellationToken
        )
        {
            query.Location = null;

            courseManagementApiMock
                .Setup(client => client.GetWithResponseCode<GetCourseProvidersResponseFromCourseApi>(It.Is<GetProvidersByCourseIdRequest>(
                        c => c.GetUrl.Contains(query.Id.ToString())
                             && c.GetUrl.Contains($"api/courses/{query.Id}/providers")
                    )
                ))
                .ReturnsAsync(new ApiResponse<GetCourseProvidersResponseFromCourseApi>(apiResponse,
                    HttpStatusCode.OK, ""));

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(apiResponse, options => options.Excluding(x => x.LarsCode));
            result.LarsCode.Should().BeEquivalentTo(apiResponse.LarsCode.ToString());
            coursesApiMock.Verify(x => x.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(x => x.StandardId == query.Id)), Times.Never);
            courseManagementApiMock.Verify(x => x.GetWithResponseCode<GetCourseProvidersResponseFromCourseApi>(It.IsAny<GetProvidersByCourseIdRequest>()), Times.Once);
            courseManagementApiMock.Verify(x => x.Get<GetAcademicYearsLatestQueryResponse>(It.IsAny<GetAcademicYearsLatestRequest>()), Times.Never);
        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        public async Task Handler_BadRequestOrNotFoundResponseFromApi_ReturnsNull(
            HttpStatusCode statusCode,
            GetCourseProvidersQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Greedy] GetCourseProvidersQueryHandler handler,
            GetStandardsListItem standardResponse,
            CancellationToken cancellationToken
        )
        {
            query.Location = null;

            courseManagementApiMock
                .Setup(client => client.GetWithResponseCode<GetCourseProvidersResponseFromCourseApi>(It.IsAny<GetProvidersByCourseIdRequest>()
                    )
                )
                 .ReturnsAsync(new ApiResponse<GetCourseProvidersResponseFromCourseApi>(null, statusCode, "Error"));

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeNull();
        }
    }
}