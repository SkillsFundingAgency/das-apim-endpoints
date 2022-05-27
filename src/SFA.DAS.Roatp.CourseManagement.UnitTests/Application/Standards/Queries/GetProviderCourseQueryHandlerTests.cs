﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetProviderCourse;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.UnitTests.Application.Standards.Queries
{
     public class GetProviderCourseQueryHandlerTests
    {
        [Test, RecursiveMoqAutoData]
        public async Task Handle_CallsInnerApiReturnsValidResponse(
            GetStandardResponse apiResponseStandard,
            GetProviderCourseResponse apiResponseProviderCourse,
            List<GetProviderCourseLocationsResponse> apiResponseProviderCourseLocation,
            GetProviderCourseQuery query,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClientMockCourse,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderCourseQueryHandler sut)
        {
            apiClientMockCourse.Setup(c => c.GetWithResponseCode<GetStandardResponse>(It.Is<GetStandardRequest>(c =>  
                        c.GetUrl.Equals(new GetStandardRequest(query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<GetStandardResponse>(apiResponseStandard, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<GetProviderCourseResponse>(It.Is<GetProviderCourseRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<GetProviderCourseResponse>(apiResponseProviderCourse, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>(apiResponseProviderCourseLocation, HttpStatusCode.OK, ""));

            var result = await sut.Handle(query, new CancellationToken());
            result.Should().NotBeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApiReturnsExceptionWhenBadRequest(
           string errorContent,
           GetProviderCourseQuery query,
           [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClientMockCourse,
           GetProviderCourseQueryHandler sut)
        {
            apiClientMockCourse.Setup(c => c.GetWithResponseCode<GetStandardResponse>(It.Is<GetStandardRequest>(c =>
                        c.GetUrl.Equals(new GetStandardRequest(query.LarsCode).GetUrl))))
                        .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest, errorContent));

            Assert.ThrowsAsync<HttpRequestContentException>(() => sut.Handle(query, new CancellationToken()));
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApiReturnsExceptionWhenInternalServerError(
           string errorContent,
           GetProviderCourseQuery query,
           [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClientMockCourse,
           GetProviderCourseQueryHandler sut)
        {
            apiClientMockCourse.Setup(c => c.GetWithResponseCode<GetStandardResponse>(It.Is<GetStandardRequest>(c =>
                        c.GetUrl.Equals(new GetStandardRequest(query.LarsCode).GetUrl))))
                        .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.InternalServerError, errorContent));

            Assert.ThrowsAsync<HttpRequestContentException>(() => sut.Handle(query, new CancellationToken()));
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApiReturnsBadRequest(
            GetStandardResponse apiResponseStandard,
            string errorContent,
            GetProviderCourseQuery query,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClientMockCourse,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderCourseQueryHandler sut)
        {
            apiClientMockCourse.Setup(c => c.GetWithResponseCode<GetStandardResponse>(It.Is<GetStandardRequest>(c =>
                        c.GetUrl.Equals(new GetStandardRequest(query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<GetStandardResponse>(apiResponseStandard, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<GetProviderCourseResponse>(It.Is<GetProviderCourseRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseRequest(query.Ukprn, query.LarsCode).GetUrl))))
                        .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest, errorContent));

            Assert.ThrowsAsync<HttpRequestContentException>(() => sut.Handle(query, new CancellationToken()));
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApiReturnsBadRequest(
            GetStandardResponse apiResponseStandard,
            GetProviderCourseResponse apiResponseProviderCourse,
            string errorContent,
            GetProviderCourseQuery query,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClientMockCourse,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderCourseQueryHandler sut)
        {
            apiClientMockCourse.Setup(c => c.GetWithResponseCode<GetStandardResponse>(It.Is<GetStandardRequest>(c =>
                        c.GetUrl.Equals(new GetStandardRequest(query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<GetStandardResponse>(apiResponseStandard, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<GetProviderCourseResponse>(It.Is<GetProviderCourseRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<GetProviderCourseResponse>(apiResponseProviderCourse, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl))))
                         .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest, errorContent));

            Assert.ThrowsAsync<HttpRequestContentException>(() => sut.Handle(query, new CancellationToken()));
        }
    }
}
