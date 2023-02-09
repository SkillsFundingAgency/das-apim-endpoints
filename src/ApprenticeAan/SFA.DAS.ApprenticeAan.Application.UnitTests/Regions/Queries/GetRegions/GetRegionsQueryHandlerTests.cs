using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Regions.Requests;
using SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Regions.Queries.GetRegions
{
    public class GetRegionsQueryHandlerTests
    {
        [Test]
        [MoqAutoData]
        public async Task Handle_ReturnAllRegions(
            GetRegionsQuery query,
            [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClient,
            [Frozen(Matching.ImplementedInterfaces)]
            GetRegionsQueryHandler handler,
            GetRegionsQueryResult response)
        {
            apiClient.Setup(x =>
                    x.GetWithResponseCode<GetRegionsQueryResult>(It.IsAny<GetRegionsQueryRequest>()))
                .ReturnsAsync(new ApiResponse<GetRegionsQueryResult>(response, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            result?.Regions?.Count.Should().NotBe(0);
            result?.Regions?.Count.Should().Be(response.Regions.Count);
        }

        [Test]
        [MoqAutoData]
        public async Task Handle_RegionsNotFoundOrError(
            GetRegionsQuery query,
            [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClient,
            [Frozen(Matching.ImplementedInterfaces)]
            GetRegionsQueryHandler handler)
        {
            apiClient.Setup(x =>
                    x.GetWithResponseCode<GetRegionsQueryResult>(It.IsAny<GetRegionsQueryRequest>()))
                .ReturnsAsync(new ApiResponse<GetRegionsQueryResult>(null!, HttpStatusCode.BadRequest, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().Be(null);
            result?.Regions.Should().BeNull();
        }
    }
}