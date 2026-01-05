using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.ProviderCourseTypes.Queries;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

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
            apiClientMock.Setup(c => c.Get<List<ProviderCourseTypeResult>>(It.IsAny<GetProviderCourseTypesRequest>())).ReturnsAsync(providerCourseTypes);
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
            apiClientMock.Setup(c => c.Get<List<ProviderCourseTypeResult>>(It.IsAny<GetProviderCourseTypesRequest>())).ReturnsAsync(providerCourseTypes);
            var result = await sut.Handle(query, new CancellationToken());

            result.Should().BeEmpty();
        }

        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsNullSet(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderCourseTypesQuery query,
            GetProviderCourseTypesQueryHandler sut)
        {
            apiClientMock.Setup(c => c.Get<List<ProviderCourseTypeResult>>(It.IsAny<GetProviderCourseTypesRequest>())).ReturnsAsync((List<ProviderCourseTypeResult>)null);
            var result = await sut.Handle(query, new CancellationToken());

            result.Should().BeNull();
        }

        [Test, MoqAutoData]
        public void Handle_CallsInnerApi_ThrowsException(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderCourseTypesQuery query,
            GetProviderCourseTypesQueryHandler sut)
        {
            apiClientMock.Setup(c => c.Get<List<ProviderCourseTypeResult>>(It.IsAny<GetProviderCourseTypesRequest>())).ThrowsAsync(new Exception());
            Assert.ThrowsAsync<Exception>(() => sut.Handle(query, new CancellationToken()));
        }
    }
}
