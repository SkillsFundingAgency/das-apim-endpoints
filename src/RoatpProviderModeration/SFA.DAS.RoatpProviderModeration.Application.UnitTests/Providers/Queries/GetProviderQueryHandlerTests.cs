using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Requests;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Responses;
using SFA.DAS.RoatpProviderModeration.Application.Queries.GetProvider;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpProviderModeration.Application.UnitTests.Providers.Queries
{
    [TestFixture]
    public class GetProviderQueryHandlerTests
    {
        [Test, RecursiveMoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsValidResponse(
            GetProviderResponse apiResponseProvider,
            GetProviderQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<GetProviderResponse>(It.Is<GetProviderRequest>(c =>
                        c.GetUrl.Equals(new GetProviderRequest(query.Ukprn).GetUrl)))).
                        ReturnsAsync(new ApiResponse<GetProviderResponse>(apiResponseProvider, HttpStatusCode.OK, ""));

            var result = await sut.Handle(query, new CancellationToken());
            result.Should().NotBeNull();
            result.MarketingInfo.Should().BeEquivalentTo(apiResponseProvider.MarketingInfo);
            result.ProviderType.Should().Be(apiResponseProvider.ProviderType);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Handle_CallsInnerApi_404ReturnsNull(
            GetProviderResponse apiResponseProvider,
            GetProviderQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<GetProviderResponse>(It.Is<GetProviderRequest>(c =>
                    c.GetUrl.Equals(new GetProviderRequest(query.Ukprn).GetUrl)))).
                ReturnsAsync(new ApiResponse<GetProviderResponse>(apiResponseProvider, HttpStatusCode.NotFound, ""));

            var result = await sut.Handle(query, new CancellationToken());
            result.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenProviderGetsBadRequest(
            GetProviderResponse apiResponseProvider,
            GetProviderQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(new ApiResponse<GetProviderResponse>(apiResponseProvider, HttpStatusCode.BadRequest, ""));
            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, new CancellationToken()));
        }
    }
}
