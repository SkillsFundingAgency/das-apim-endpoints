using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Application.Regions.Queries;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using SFA.DAS.RoatpCourseManagement.Application.Regions.Queries;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Regions.Queries
{
    [TestFixture]
    public class GetAllRegionsQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_UnsuccessfulStatus_ThrowsException(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllRegionsQueryHandler sut)
        {
            var response = new ApiResponse<List<RegionModel>>(null, HttpStatusCode.InternalServerError, "");
            apiClientMock.Setup(a => a.GetWithResponseCode<List<RegionModel>>(It.IsAny<GetAllRegionsRequest>())).ReturnsAsync(response);

            Func<Task> action = () => sut.Handle(It.IsAny<GetAllRegionsQuery>(), It.IsAny<CancellationToken>());

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test, MoqAutoData]
        public async Task Handle_SuccessfulStatus_ReturnsData(
           [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
           GetAllRegionsQueryHandler sut,
           List<RegionModel> data)
        {
            var response = new ApiResponse<List<RegionModel>>(data, HttpStatusCode.OK, "");
            apiClientMock.Setup(a => a.GetWithResponseCode<List<RegionModel>>(It.IsAny<GetAllRegionsRequest>())).ReturnsAsync(response);

            var result = await sut.Handle(It.IsAny<GetAllRegionsQuery>(), It.IsAny<CancellationToken>());

            result.Regions.Should().BeEquivalentTo(data);
        }
    }
}
