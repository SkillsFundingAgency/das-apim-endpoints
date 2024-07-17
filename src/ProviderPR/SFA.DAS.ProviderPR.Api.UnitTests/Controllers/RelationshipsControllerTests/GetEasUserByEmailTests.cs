﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ProviderPR.Api.Controllers;
using SFA.DAS.ProviderPR.Application.Queries.GetEasUserByEmail;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderPR.Api.UnitTests.Controllers.RelationshipsControllerTests;
public class GetEasUserByEmailTests
{
    [Test, MoqAutoData]
    public async Task Get_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RelationshipsController sut,
        string email,
        long ukprn,
        CancellationToken cancellationToken)
    {
        await sut.GetEasUserByEmail(email, ukprn, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(It.Is<GetEasUserByEmailQuery>(x => x.Ukprn == ukprn && x.Email == email),
                It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task Get_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RelationshipsController sut,
        string email,
        long ukprn,
        GetEasUserByEmailQueryResult queryResult,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetEasUserByEmailQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await sut.GetEasUserByEmail(email, ukprn, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }
}
