using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetProviderLocationDetails;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Locations.Queries.GetAllProviderLocations
{
    [TestFixture]
    public class GetProviderLocationDetailsQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            ProviderLocationModel location,
            GetProviderLocationDetailsQuery query,
            GetProviderLocationDetailsQueryHandler sut)
        {

            apiClientMock.Setup(c => c.GetWithResponseCode<ProviderLocationModel>(It.Is<GetProviderLocationDetailsQuery>(q => q == query)))
                .ReturnsAsync(new ApiResponse<ProviderLocationModel>(location, HttpStatusCode.OK, ""));

            var result = await sut.Handle(query, new CancellationToken());
            result.ProviderLocation.Should().BeEquivalentTo(location);
        }

        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi_404_Returns_Null(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            ProviderLocationModel location,
            GetProviderLocationDetailsQuery query,
            GetProviderLocationDetailsQueryHandler sut)
        {

            apiClientMock.Setup(c => c.GetWithResponseCode<ProviderLocationModel>(It.Is<GetProviderLocationDetailsQuery>(q => q == query)))
                .ReturnsAsync(new ApiResponse<ProviderLocationModel>(location, HttpStatusCode.NotFound, ""));

            var result = await sut.Handle(query, new CancellationToken());
            result.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi_Throws_Exception(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderLocationDetailsQuery query,
            GetProviderLocationDetailsQueryHandler sut)
        {

            apiClientMock.Setup(c => c.GetWithResponseCode<ProviderLocationModel>(It.Is<GetProviderLocationDetailsQuery>(q => q == query)))
                .ReturnsAsync(new ApiResponse<ProviderLocationModel>(null, HttpStatusCode.BadRequest, ""));

            Func<Task> action = () => sut.Handle(query, new CancellationToken());

            await action.Should().ThrowAsync<ApiResponseException>();
        }
    }
}
