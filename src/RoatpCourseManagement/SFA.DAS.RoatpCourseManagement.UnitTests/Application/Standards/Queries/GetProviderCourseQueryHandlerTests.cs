using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourse;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries
{
    [TestFixture]
     public class GetProviderCourseQueryHandlerTests
    {
        [Test, RecursiveMoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsValidResponse(
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
        public void Handle_CallsInnerApi_ReturnsExceptionWhenStandardGetsBadRequest(
           GetProviderCourseQuery query,
           [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClientMockCourse,
           GetProviderCourseQueryHandler sut)
        {
            apiClientMockCourse.Setup(c => c.GetWithResponseCode<GetStandardResponse>(It.Is<GetStandardRequest>(c =>
                        c.GetUrl.Equals(new GetStandardRequest(query.LarsCode).GetUrl))))
                        .ReturnsAsync(new ApiResponse<GetStandardResponse>(new GetStandardResponse(), HttpStatusCode.BadRequest, "Error"));
            
            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, new CancellationToken()));
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenStandardGetsInternalServerError(
           GetProviderCourseQuery query,
           [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClientMockCourse,
           GetProviderCourseQueryHandler sut)
        {
            apiClientMockCourse.Setup(c => c.GetWithResponseCode<GetStandardResponse>(It.Is<GetStandardRequest>(c =>
                        c.GetUrl.Equals(new GetStandardRequest(query.LarsCode).GetUrl))))
                        .ReturnsAsync(new ApiResponse<GetStandardResponse>(new GetStandardResponse(), HttpStatusCode.InternalServerError, "Error"));

            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, new CancellationToken()));
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenProviderCourseGetsBadRequest(
            GetStandardResponse apiResponseStandard,
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
                       .ReturnsAsync(new ApiResponse<GetProviderCourseResponse>(new GetProviderCourseResponse(), HttpStatusCode.BadRequest, "Error"));

            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, new CancellationToken()));
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenProviderCourseLocationsGetsBadRequest(
            GetStandardResponse apiResponseStandard,
            GetProviderCourseResponse apiResponseProviderCourse,
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
                        .ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>(new List<GetProviderCourseLocationsResponse> { }, HttpStatusCode.BadRequest, "Error"));

            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, new CancellationToken()));
        }
    }
}
