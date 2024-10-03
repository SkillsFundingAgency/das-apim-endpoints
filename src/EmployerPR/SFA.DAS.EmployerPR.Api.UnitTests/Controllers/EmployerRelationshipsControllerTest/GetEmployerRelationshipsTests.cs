using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.EmployerRelationships.Queries.GetEmployerRelationships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.EmployerRelationshipsControllerTest;

public class GetEmployerRelationshipsTests
{
    [Test]
    [MoqAutoData]
    public async Task GetEmployerRelationships_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployerRelationshipsController sut,
        long accountId,
        CancellationToken cancellationToken
    )
    {
        await sut.GetEmployerRelationships(accountId, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(
                It.Is<GetEmployerRelationshipsQuery>(x =>
                    x.AccountId == accountId
                ),
                It.IsAny<CancellationToken>()
            )
        );
    }

    [Test, MoqAutoData]
    public async Task GetEmployerRelationships_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployerRelationshipsController sut,
        long accountId,
        GetEmployerRelationshipsQueryResult queryResult,
        CancellationToken cancellationToken)
    {
        mediatorMock
            .Setup(m => m.Send(It.Is<GetEmployerRelationshipsQuery>(x => x.AccountId == accountId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await sut.GetEmployerRelationships(accountId, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }
}
