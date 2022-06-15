using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.RegisteredProviders.Queries;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.RegisteredProvider;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.RegisteredProviders.Queries
{
    [TestFixture]
    public class GetRegisteredProvidersQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi_OkResponse(
            [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
            List<RegisteredProviderModel> providers,
            GetRegisteredProvidersQuery query,
            GetRegisteredProvidersQueryHandler sut)
        {
            var response = new ApiResponse<List<RegisteredProviderModel>>(providers, HttpStatusCode.OK, "");

            apiClientMock.Setup(c => c.GetWithResponseCode<List<RegisteredProviderModel>>(It.IsAny<GetRegisteredProvidersQuery>())).ReturnsAsync(response);
            var result = await sut.Handle(query, new CancellationToken());
            result.Body.Should().BeEquivalentTo(providers);
        }

        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi_ErrorResponse(
            [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
            List<RegisteredProviderModel> providers,
            GetRegisteredProvidersQuery query,
            GetRegisteredProvidersQueryHandler sut)
        {
            var errorMessage = "error message";
            var response = new ApiResponse<List<RegisteredProviderModel>>(null, HttpStatusCode.BadRequest, errorMessage);

            apiClientMock.Setup(c => c.GetWithResponseCode<List<RegisteredProviderModel>>(It.IsAny<GetRegisteredProvidersQuery>())).ReturnsAsync(response);
            var result = await sut.Handle(query, new CancellationToken());
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.ErrorContent.Should().Be(errorMessage);
        }
    }
}