﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Queries.GetAccountLegalEntities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.EmployerAccountControllerTests;
public class GetEmployerLegalEntitiesTests
{
    [Test, MoqAutoData]
    public async Task Get_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployerAccountsController sut,
        string hashedAccountId,
        CancellationToken cancellationToken)
    {
        await sut.GetAccountLegalEntities(hashedAccountId, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(It.Is<GetAccountLegalEntitiesQuery>(x => x.HashedAccountId == hashedAccountId),
                It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task Get_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployerAccountsController sut,
        string hashedAccountId,
        GetAccountLegalEntitiesQueryResult queryResult,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAccountLegalEntitiesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await sut.GetAccountLegalEntities(hashedAccountId, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }
}
