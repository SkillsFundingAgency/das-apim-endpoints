using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.ProviderCourseTypes.Queries;
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

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.ProviderCourseTypes.Queries
{
    [TestFixture]
    public class GetProviderCourseTypesQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsResults(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            List<ProviderCourseTypeResult> providerCourseTypes,
            GetProviderCourseTypesQuery query,
            GetProviderCourseTypesQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<ProviderCourseTypeResult>>(It.IsAny<GetProviderCourseTypesRequest>())).ReturnsAsync(new ApiResponse<List<ProviderCourseTypeResult>>(providerCourseTypes, HttpStatusCode.OK, ""));
            var result = await sut.Handle(query, new CancellationToken());

            result.Should().BeEquivalentTo(providerCourseTypes);
        }

        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsEmptySet(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderCourseTypesQuery query,
            GetProviderCourseTypesQueryHandler sut)
        {
            var providerCourseTypes = new List<ProviderCourseTypeResult>();
            apiClientMock.Setup(c => c.GetWithResponseCode<List<ProviderCourseTypeResult>>(It.IsAny<GetProviderCourseTypesRequest>())).ReturnsAsync(new ApiResponse<List<ProviderCourseTypeResult>>(providerCourseTypes, HttpStatusCode.OK, ""));
            var result = await sut.Handle(query, new CancellationToken());

            result.Should().BeEmpty();
        }

        [Test, MoqAutoData]
        public void Handle_CallsInnerApi_ThrowsException(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderCourseTypesQuery query,
            GetProviderCourseTypesQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<ProviderCourseTypeResult>>(It.IsAny<GetProviderCourseTypesRequest>())).ReturnsAsync(new ApiResponse<List<ProviderCourseTypeResult>>(null, HttpStatusCode.InternalServerError, ""));
            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, new CancellationToken()));
        }
    }
}
