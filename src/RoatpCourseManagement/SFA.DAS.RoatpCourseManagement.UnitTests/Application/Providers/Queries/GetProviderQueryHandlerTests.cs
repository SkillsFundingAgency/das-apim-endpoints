using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProvider;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Providers.Queries
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
            result.Should().BeEquivalentTo(apiResponseProvider);
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
            Assert.ThrowsAsync<HttpRequestContentException>(() => sut.Handle(query, new CancellationToken()));
        }
    }
}
