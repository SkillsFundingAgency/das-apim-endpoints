using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourse;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries
{
    [TestFixture]
    public class GetProviderCourseQueryHandlerTests
    {
        [Test, RecursiveMoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsValidResponse(
            GetStandardResponseFromCourseManagementApi apiResponseStandard,
            GetProviderCourseResponse apiResponseProviderCourse,
            List<GetProviderCourseLocationsResponse> apiResponseProviderCourseLocation,
            GetProviderCourseQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderCourseQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<GetStandardResponseFromCourseManagementApi>(It.Is<GetStandardLookRequest>(c =>
                        c.GetUrl.Equals(new GetStandardLookRequest(query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<GetStandardResponseFromCourseManagementApi>(apiResponseStandard, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<GetProviderCourseResponse>(It.Is<GetProviderCourseRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<GetProviderCourseResponse>(apiResponseProviderCourse, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>(apiResponseProviderCourseLocation, HttpStatusCode.OK, ""));

            var result = await sut.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();

            var course = apiResponseProviderCourse;
            var standard = apiResponseStandard;
            using (new AssertionScope())
            {
                result.LarsCode.Should().Be(apiResponseProviderCourse.LarsCode);
                result.IfateReferenceNumber.Should().Be(standard.IfateReferenceNumber);
                result.CourseName.Should().Be(standard.Title);
                result.Level.Should().Be(standard.Level);
                result.ApprenticeshipType.Should().Be(standard.ApprenticeshipType);
                result.ApprovalBody.Should().Be(standard.ApprovalBody);
                result.Route.Should().Be(standard.Route);
                result.StandardInfoUrl.Should().Be(course.StandardInfoUrl);
                result.ContactUsPhoneNumber.Should().Be(course.ContactUsPhoneNumber);
                result.ContactUsEmail.Should().Be(course.ContactUsEmail);
                result.ProviderCourseLocations.Count.Should().Be(apiResponseProviderCourseLocation.Count);
                result.IsApprovedByRegulator.Should().Be(course.IsApprovedByRegulator);
                result.IsRegulatedForProvider.Should().Be(course.IsRegulatedForProvider);
                result.HasLocations.Should().Be(course.HasLocations);
            }
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenStandardGetsBadRequest(
           GetProviderCourseQuery query,
           [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
           GetProviderCourseQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<GetStandardResponseFromCourseManagementApi>(It.Is<GetStandardLookRequest>(c =>
                        c.GetUrl.Equals(new GetStandardLookRequest(query.LarsCode).GetUrl))))
                        .ReturnsAsync(new ApiResponse<GetStandardResponseFromCourseManagementApi>(new GetStandardResponseFromCourseManagementApi(), HttpStatusCode.BadRequest, "Error"));

            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, CancellationToken.None));
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenStandardGetsInternalServerError(
           GetProviderCourseQuery query,
           [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
           GetProviderCourseQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<GetStandardResponseFromCourseManagementApi>(It.Is<GetStandardLookRequest>(c =>
                        c.GetUrl.Equals(new GetStandardLookRequest(query.LarsCode).GetUrl))))
                        .ReturnsAsync(new ApiResponse<GetStandardResponseFromCourseManagementApi>(new GetStandardResponseFromCourseManagementApi(), HttpStatusCode.InternalServerError, "Error"));

            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, CancellationToken.None));
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenProviderCourseGetsBadRequest(
            GetStandardResponseFromCourseManagementApi apiResponseStandard,
            GetProviderCourseQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderCourseQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<GetStandardResponseFromCourseManagementApi>(It.Is<GetStandardLookRequest>(c =>
                        c.GetUrl.Equals(new GetStandardLookRequest(query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<GetStandardResponseFromCourseManagementApi>(apiResponseStandard, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<GetProviderCourseResponse>(It.Is<GetProviderCourseRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseRequest(query.Ukprn, query.LarsCode).GetUrl))))
                       .ReturnsAsync(new ApiResponse<GetProviderCourseResponse>(new GetProviderCourseResponse(), HttpStatusCode.BadRequest, "Error"));

            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, CancellationToken.None));
        }


        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenProviderCourseLocationsGetsBadRequest(
            GetStandardResponseFromCourseManagementApi apiResponseStandard,
            GetProviderCourseResponse apiResponseProviderCourse,
            GetProviderCourseQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderCourseQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<GetStandardResponseFromCourseManagementApi>(It.Is<GetStandardLookRequest>(c =>
                        c.GetUrl.Equals(new GetStandardLookRequest(query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<GetStandardResponseFromCourseManagementApi>(apiResponseStandard, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<GetProviderCourseResponse>(It.Is<GetProviderCourseRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<GetProviderCourseResponse>(apiResponseProviderCourse, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl))))
                        .ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>([], HttpStatusCode.BadRequest, "Error"));

            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, CancellationToken.None));
        }
    }
}
