using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.PermissionsControllerTests;
public class GetPermissionsTests
{
    [Test, MoqAutoData]
    public async Task GetPermissions_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        GetPermissionsQuery query,
        CancellationToken cancellationToken)
    {
        await sut.GetPermissions(query, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(It.IsAny<GetPermissionsQuery>(),
                It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task GetPermissions_HandlerReturnsData_ReturnsExpectedResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        GetPermissionsResponse queryResponse,
        GetPermissionsQuery query,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetPermissionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResponse);

        var result = await sut.GetPermissions(query, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResponse);
    }

    [Test, MoqAutoData]
    public async Task GetPermissions_HandlerReturnsNull_Returns404(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        GetPermissionsQuery query,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetPermissionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetPermissionsResponse)null!);

        var result = await sut.GetPermissions(query, cancellationToken);

        result.As<NotFoundResult>().Should().NotBeNull();
    }
}
