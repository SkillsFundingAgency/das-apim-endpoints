using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.Profiles.Queries.GetProfilesByUserType;
using SFA.DAS.EmployerAan.Configuration;
using SFA.DAS.EmployerAan.InnerApi.Profiles;
using SFA.DAS.EmployerAan.Services;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Profiles.Queries.GetProfilesByUserType;
public class GetProfilesByUserTypeQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnAllProfiles(
        GetProfilesByUserTypeQuery query,
        [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClient,
        GetProfilesByUserTypeQueryHandler handler,
        GetProfilesByUserTypeQueryResult response)
    {
        apiClient.Setup(x =>
                        x.GetWithResponseCode<GetProfilesByUserTypeQueryResult>(It.IsAny<GetProfilesByUserTypeRequest>()))
                        .ReturnsAsync(new ApiResponse<GetProfilesByUserTypeQueryResult>(response, HttpStatusCode.OK, string.Empty));

        var result = await handler.Handle(query, CancellationToken.None);

        result?.Profiles?.Count.Should().NotBe(0);
        result?.Profiles?.Count.Should().Be(response.Profiles.Count);
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_ProfilesNotFoundOrError(
        GetProfilesByUserTypeQuery query,
        [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClient,
        GetProfilesByUserTypeQueryHandler handler)
    {
        apiClient.Setup(x =>
                        x.GetWithResponseCode<GetProfilesByUserTypeQueryResult>(It.IsAny<GetProfilesByUserTypeRequest>()))
                        .ReturnsAsync(new ApiResponse<GetProfilesByUserTypeQueryResult>(null!, HttpStatusCode.BadRequest, string.Empty));

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().Be(null);
        result?.Profiles.Should().BeNull();
    }
}
