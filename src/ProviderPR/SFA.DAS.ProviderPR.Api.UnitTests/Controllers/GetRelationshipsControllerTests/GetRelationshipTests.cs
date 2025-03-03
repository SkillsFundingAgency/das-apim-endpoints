using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ProviderPR.Api.Controllers;
using SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationship;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Api.UnitTests.Controllers.GetRelationshipsControllerTests;

public class GetRelationshipTests
{
    [Test, AutoData]
    public async Task GetRelationship_InvokesInnerApi(int ukprn, int accountLEgalEntityId, CancellationToken cancellationToken)
    {
        Mock<IMediator> mediatorMock = new();
        RelationshipsController sut = new(mediatorMock.Object);

        await sut.GetRelationship(ukprn, accountLEgalEntityId, cancellationToken);

        mediatorMock.Verify(c => c.Send(It.Is<GetRelationshipQuery>(q => q.Ukprn == ukprn && q.AccountLegalEntityId == accountLEgalEntityId), cancellationToken), Times.Once);
    }

    [Test, AutoData]
    public async Task GetRelationship_ReturnsOkResponse(int ukprn, int accountLEgalEntityId, GetRelationshipResponse response, CancellationToken cancellationToken)
    {
        Mock<IMediator> mediatorMock = new();
        mediatorMock.Setup(m => m.Send(It.Is<GetRelationshipQuery>(q => q.Ukprn == ukprn && q.AccountLegalEntityId == accountLEgalEntityId), cancellationToken)).ReturnsAsync(response);

        RelationshipsController sut = new(mediatorMock.Object);

        var result = await sut.GetRelationship(ukprn, accountLEgalEntityId, cancellationToken);

        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(response);
    }
}
