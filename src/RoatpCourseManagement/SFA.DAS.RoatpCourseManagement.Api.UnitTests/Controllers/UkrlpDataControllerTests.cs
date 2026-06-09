using System;
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
        DateTime updatedSinceDate,
        CancellationToken cancellationToken)
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUkrlpProvidersQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        await sut.GetProvidersData(updatedSinceDate, cancellationToken);

        _mediatorMock.Verify(m => m.Send(It.Is<GetUkrlpProvidersQuery>(q => q.UpdatedSinceDate == updatedSinceDate), cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenGettingProvidersData_NullUpdatedSinceDateIsAcceptable(
        [Frozen] Mock<IMediator> _mediatorMock,
        [Greedy] UkrlpDataController sut,
        GetUkrlpProvidersQueryResult result,
        CancellationToken cancellationToken)
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUkrlpProvidersQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        await sut.GetProvidersData(null, cancellationToken);

        _mediatorMock.Verify(m => m.Send(It.Is<GetUkrlpProvidersQuery>(q => q.UpdatedSinceDate == null), cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenGettingProvidersData_ThenReturnsData(
        [Frozen] Mock<IMediator> _mediatorMock,
        [Greedy] UkrlpDataController sut,
        GetUkrlpProvidersQueryResult result,
        DateTime updatedSinceDate,
        CancellationToken cancellationToken)
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUkrlpProvidersQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var response = await sut.GetProvidersData(updatedSinceDate, cancellationToken);

        response.As<OkObjectResult>().Should().NotBeNull();
        response.As<OkObjectResult>().Value.Should().BeEquivalentTo(result);
    }
}
