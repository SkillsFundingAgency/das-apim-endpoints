using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using SFA.DAS.Roatp.CourseManagement.Application.Regions.Queries;
using System.Collections.Generic;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries
{
    [TestFixture]
    public class GetAllRegionsQueryHandlerTests
    {
        [Test, RecursiveMoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsValidResponse(
            GetAllRegionsQuery query,
            List<RegionModel> apiResponse,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllRegionsQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<RegionModel>>(It.Is<GetAllRegionsQuery>(c =>
                        c.GetUrl.Equals(query.GetUrl)))).ReturnsAsync(new ApiResponse<List<RegionModel>>(apiResponse, HttpStatusCode.OK, ""));

            var result = await sut.Handle(query, new CancellationToken());

            result.Should().NotBeNull();

        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenStandardGetsBadRequest(
           GetAllRegionsQuery query,
           List<RegionModel> apiResponse,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllRegionsQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<RegionModel>>(It.Is<GetAllRegionsQuery>(c =>
                       c.GetUrl.Equals(query.GetUrl)))).ReturnsAsync(new ApiResponse<List<RegionModel>>(apiResponse, HttpStatusCode.BadRequest, "Error"));

            Assert.ThrowsAsync<HttpRequestContentException>(() => sut.Handle(query, new CancellationToken()));
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenStandardGetsInternalServerError(
          GetAllRegionsQuery query,
           List<RegionModel> apiResponse,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllRegionsQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<RegionModel>>(It.Is<GetAllRegionsQuery>(c =>
                      c.GetUrl.Equals(query.GetUrl)))).ReturnsAsync(new ApiResponse<List<RegionModel>>(apiResponse, HttpStatusCode.InternalServerError, "Error"));

            Assert.ThrowsAsync<HttpRequestContentException>(() => sut.Handle(query, new CancellationToken()));
        }
    }
}


