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
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllStandardRegions;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using SFA.DAS.Roatp.CourseManagement.Application.Regions.Queries;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries
{
    [TestFixture]
     public class GetAllStandardRegionsQueryHandlerTests
    {
        [Test, RecursiveMoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsValidResponse(
            List<RegionModel> apiResponseGetAllRegions,
            List<GetProviderCourseLocationsResponse> apiResponseProviderCourseLocations,
            GetAllStandardRegionsQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllStandardRegionsQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<RegionModel>>(It.Is<GetAllRegionsQuery>(c =>
                        c.GetUrl.Equals(new GetAllRegionsQuery().GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<RegionModel>>(apiResponseGetAllRegions, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>(apiResponseProviderCourseLocations, HttpStatusCode.OK, ""));

            var result = await sut.Handle(query, new CancellationToken());

            result.Should().NotBeNull();
        }


        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApiNoRegionsInResponseBody_ReturnsException(
            List<GetProviderCourseLocationsResponse> apiResponseProviderCourseLocations,
            GetAllStandardRegionsQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllStandardRegionsQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<RegionModel>>(It.Is<GetAllRegionsQuery>(c =>
                        c.GetUrl.Equals(new GetAllRegionsQuery().GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<RegionModel>>(null, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>(apiResponseProviderCourseLocations, HttpStatusCode.OK, ""));

            Assert.ThrowsAsync<ValidationException>(() => sut.Handle(query, new CancellationToken()));
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApiNoProviderCourseLocationsInResponseBody_ReturnsException(
            List<RegionModel> apiResponseGetAllRegions,
            GetAllStandardRegionsQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllStandardRegionsQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<RegionModel>>(It.Is<GetAllRegionsQuery>(c =>
                        c.GetUrl.Equals(new GetAllRegionsQuery().GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<RegionModel>>(apiResponseGetAllRegions, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>(null, HttpStatusCode.OK, ""));

            Assert.ThrowsAsync<ValidationException>(() => sut.Handle(query, new CancellationToken()));
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenRegionsGetsBadRequest(
            List<GetProviderCourseLocationsResponse> apiResponseProviderCourseLocations,
            GetAllStandardRegionsQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllStandardRegionsQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<RegionModel>>(It.Is<GetAllRegionsQuery>(c =>
                        c.GetUrl.Equals(new GetAllRegionsQuery().GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<RegionModel>>(null, HttpStatusCode.BadRequest, "Error"));

            apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>(apiResponseProviderCourseLocations, HttpStatusCode.OK, ""));

            Assert.ThrowsAsync<HttpRequestContentException>(() => sut.Handle(query, new CancellationToken()));
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenProviderCourseLocationsGetsBadRequest(
             List<RegionModel> apiResponseGetAllRegions,
             List<GetProviderCourseLocationsResponse> apiResponseProviderCourseLocations,
             GetAllStandardRegionsQuery query,
             [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
             GetAllStandardRegionsQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<RegionModel>>(It.Is<GetAllRegionsQuery>(c =>
                        c.GetUrl.Equals(new GetAllRegionsQuery().GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<RegionModel>>(apiResponseGetAllRegions, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>(apiResponseProviderCourseLocations, HttpStatusCode.BadRequest, "Error"));

            Assert.ThrowsAsync<HttpRequestContentException>(() => sut.Handle(query, new CancellationToken()));
        }
    }
}
