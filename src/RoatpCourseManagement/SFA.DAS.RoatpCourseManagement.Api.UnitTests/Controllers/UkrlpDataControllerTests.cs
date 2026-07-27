using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData.Queries.GetUkrlpProviders;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers;

[TestFixture]
public class UkrlpDataControllerTests
{
    [Test, MoqAutoData]
    public async Task WhenGettingProvidersData_ThenInvokesMediator(
        [Frozen] Mock<IMediator> _mediatorMock,
        [Greedy] UkrlpDataController sut,
        GetUkrlpProvidersQueryResult result,
        GetUkrlpProvidersQuery query,
        CancellationToken cancellationToken)
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUkrlpProvidersQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        await sut.GetProvidersData(query, cancellationToken);

        _mediatorMock.Verify(m => m.Send(query, cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenGettingProvidersData_EmptyUkrpnList_ReturnsBadRequest(
        [Frozen] Mock<IMediator> _mediatorMock,
        [Greedy] UkrlpDataController sut,
        GetUkrlpProvidersQueryResult result,
        CancellationToken cancellationToken)
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUkrlpProvidersQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var response = await sut.GetProvidersData(new GetUkrlpProvidersQuery(), cancellationToken);

        response.As<BadRequestObjectResult>().Should().NotBeNull();
        _mediatorMock.Verify(m => m.Send(It.Is<GetUkrlpProvidersQuery>(q => q.UpdatedSinceDate == null), cancellationToken), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task WhenGettingProvidersData_ThenReturnsData(
        [Frozen] Mock<IMediator> _mediatorMock,
        [Greedy] UkrlpDataController sut,
        GetUkrlpProvidersQueryResult result,
        GetUkrlpProvidersQuery query,
        CancellationToken cancellationToken)
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUkrlpProvidersQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var response = await sut.GetProvidersData(query, cancellationToken);

        response.As<OkObjectResult>().Should().NotBeNull();
        response.As<OkObjectResult>().Value.Should().BeEquivalentTo(result);
    }
}
