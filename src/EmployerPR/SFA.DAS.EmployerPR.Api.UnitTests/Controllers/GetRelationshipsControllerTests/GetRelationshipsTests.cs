using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Relationships.Queries.GetRelationships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.GetRelationshipsControllerTests;

public class GetRelationshipsTests
{
    [Test, MoqAutoData]
    public async Task GetRelationships_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RelationshipsController sut,
        GetRelationshipsQuery query,
        CancellationToken cancellationToken)
    {
        await sut.GetRelationships(query, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(It.IsAny<GetRelationshipsQuery>(),
                It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task GetRelationships_HandlerReturnsData_ReturnsExpectedResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RelationshipsController sut,
        GetRelationshipsResponse queryResponse,
        GetRelationshipsQuery query,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetRelationshipsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResponse);

        var result = await sut.GetRelationships(query, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResponse);
    }
}
