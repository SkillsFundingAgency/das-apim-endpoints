using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.Profiles.Queries.GetProfilesByUserType;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Profiles.Queries.GetProfilesByUserType;
public class GetProfilesByUserTypeQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnAllProfiles(
        GetProfilesByUserTypeQuery query,
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        GetProfilesByUserTypeQueryHandler handler,
        GetProfilesByUserTypeQueryResult response,
        CancellationToken cancellationToken)
    {
        apiClientMock
            .Setup(c => c.GetProfiles(query.UserType, cancellationToken))
            .ReturnsAsync(response);

        var result = await handler.Handle(query, CancellationToken.None);

        result?.Profiles?.Count.Should().NotBe(0);
        result?.Profiles?.Count.Should().Be(response.Profiles.Count);
    }
}
