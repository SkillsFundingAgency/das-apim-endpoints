using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Roatp.Queries.GetRoatpProviders;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.ProvidersControllerTests;
public class GetProvidersTests
{
    [Test, MoqAutoData]
    public async Task Get_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProvidersController sut,
        CancellationToken cancellationToken)
    {
        await sut.GetProviders(cancellationToken);

        mediatorMock.Verify(
            m => m.Send(It.IsAny<GetRoatpProvidersQuery>(),
                It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task Get_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProvidersController sut,
        GetRoatpProvidersQueryResult queryResult,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetRoatpProvidersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await sut.GetProviders(cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }
}