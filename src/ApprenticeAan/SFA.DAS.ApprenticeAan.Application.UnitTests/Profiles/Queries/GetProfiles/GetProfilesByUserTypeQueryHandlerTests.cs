using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Profiles;
using SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfilesByUserType;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Profiles.Queries.GetProfiles
{
    public class GetProfilesByUserTypeQueryHandlerTests
    {
        [Test]
        [MoqAutoData]
        public async Task Handle_ReturnAllProfiles(
            GetProfilesByUserTypeQuery query,
            [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClient,
            [Frozen(Matching.ImplementedInterfaces)]
            GetProfilesByUserTypeQueryHandler handler,
            GetProfilesByUserTypeQueryResult response)
        {
            apiClient.Setup(x =>
                    x.GetWithResponseCode<GetProfilesByUserTypeQueryResult>(It.IsAny<GetProfilesByUserTypeQueryRequest>()))
                .ReturnsAsync(new ApiResponse<GetProfilesByUserTypeQueryResult>(response, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            result?.Profiles?.Count.Should().NotBe(0);
            result?.Profiles?.Count.Should().Be(response.Profiles.Count);
        }

        [Test]
        [MoqAutoData]
        public async Task Handle_RegionsNotFoundOrError(
            GetProfilesByUserTypeQuery query,
            [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClient,
            [Frozen(Matching.ImplementedInterfaces)]
            GetProfilesByUserTypeQueryHandler handler)
        {
            apiClient.Setup(x =>
                    x.GetWithResponseCode<GetProfilesByUserTypeQueryResult>(It.IsAny<GetProfilesByUserTypeQueryRequest>()))
                .ReturnsAsync(new ApiResponse<GetProfilesByUserTypeQueryResult>(null!, HttpStatusCode.BadRequest, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().Be(null);
            result?.Profiles.Should().BeNull();
        }
    }
}