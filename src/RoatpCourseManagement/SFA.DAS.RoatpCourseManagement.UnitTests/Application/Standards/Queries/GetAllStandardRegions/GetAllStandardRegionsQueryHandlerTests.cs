using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllStandardRegions;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries.GetAllStandardRegions
{
    [TestFixture]
    public class GetAllStandardRegionsQueryHandlerTests
    {
        [Test, RecursiveMoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsValidResponse(
            List<StandardRegionModel> apiResponseGetAllRegions,
            List<GetProviderCourseLocationsResponse> apiResponseProviderCourseLocations,
            GetAllStandardRegionsQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllStandardRegionsQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<StandardRegionModel>>(It.Is<GetAllRegionsRequest>(c =>
                        c.GetUrl.Equals(new GetAllRegionsRequest().GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<StandardRegionModel>>(apiResponseGetAllRegions, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>(apiResponseProviderCourseLocations, HttpStatusCode.OK, ""));

            var result = await sut.Handle(query, new CancellationToken());

            result.Should().NotBeNull();
            result.Regions.Should().NotBeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApiNoProviderCourseLocationsInResponseBody_ReturnsException(
            List<StandardRegionModel> apiResponseGetAllRegions,
            GetAllStandardRegionsQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllStandardRegionsQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<StandardRegionModel>>(It.Is<GetAllRegionsRequest>(c =>
                        c.GetUrl.Equals(new GetAllRegionsRequest().GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<StandardRegionModel>>(apiResponseGetAllRegions, HttpStatusCode.OK, ""));

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
            apiClientMock.Setup(c => c.GetWithResponseCode<List<StandardRegionModel>>(It.Is<GetAllRegionsRequest>(c =>
                        c.GetUrl.Equals(new GetAllRegionsRequest().GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<StandardRegionModel>>(null, HttpStatusCode.BadRequest, "Error"));

            apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>(apiResponseProviderCourseLocations, HttpStatusCode.OK, ""));

            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, new CancellationToken()));
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenProviderCourseLocationsGetsBadRequest(
             List<StandardRegionModel> apiResponseGetAllRegions,
             List<GetProviderCourseLocationsResponse> apiResponseProviderCourseLocations,
             GetAllStandardRegionsQuery query,
             [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
             GetAllStandardRegionsQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<StandardRegionModel>>(It.Is<GetAllRegionsRequest>(c =>
                        c.GetUrl.Equals(new GetAllRegionsRequest().GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<StandardRegionModel>>(apiResponseGetAllRegions, HttpStatusCode.OK, ""));

            apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>(apiResponseProviderCourseLocations, HttpStatusCode.BadRequest, "Error"));

            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, new CancellationToken()));
        }
    }
}
