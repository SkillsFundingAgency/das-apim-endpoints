using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AparRegister.Api.Controllers;
using SFA.DAS.AparRegister.Application.Queries.ProviderStatusEvents;
using SFA.DAS.AparRegister.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AparRegister.Api.UnitTests.Controllers.ProvidersControllerTests;

public class ProviderControllerGetProviderStatusEventsTests
{
    [Test, MoqAutoData]
    public async Task GetProviderStatusEvents_InvokesMediator(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProvidersController sut,
        IEnumerable<ProviderStatusEvent> expected,
        int sinceEventId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken)
    {
        mediatorMock
            .Setup(m => m.Send(It.IsAny<GetProviderStatusEventsQuery>(), cancellationToken))
            .ReturnsAsync(expected);

        await sut.GetProviderStatusEvents(sinceEventId, pageSize, pageNumber, cancellationToken);

        mediatorMock.Verify(m => m.Send(
            It.Is<GetProviderStatusEventsQuery>(q =>
                q.SinceEventId == sinceEventId &&
                q.PageSize == pageSize &&
                q.PageNumber == pageNumber),
            cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task GetProviderStatusEvents_InvokesMediator_ReturnsList(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProvidersController sut,
        IEnumerable<ProviderStatusEvent> expected,
        int sinceEventId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken)
    {
        mediatorMock
            .Setup(m => m.Send(It.IsAny<GetProviderStatusEventsQuery>(), cancellationToken))
            .ReturnsAsync(expected);

        IActionResult result = await sut.GetProviderStatusEvents(sinceEventId, pageSize, pageNumber, cancellationToken);

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expected);
    }

    [MoqInlineAutoData(-1)]
    [MoqInlineAutoData(1001)]
    public async Task GetProviderStatusEvents_SetDefaultFilters_AndInvokesMediator(
        int pageSize,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProvidersController sut,
        IEnumerable<ProviderStatusEvent> expected,
        CancellationToken cancellationToken)
    {
        int sinceEventId = -1;
        int pageNumber = -1;

        mediatorMock
            .Setup(m => m.Send(It.IsAny<GetProviderStatusEventsQuery>(), cancellationToken))
            .ReturnsAsync(expected);

        await sut.GetProviderStatusEvents(sinceEventId, pageSize, pageNumber, cancellationToken);

        mediatorMock.Verify(m => m.Send(
            It.Is<GetProviderStatusEventsQuery>(q =>
                q.SinceEventId == 0 &&
                q.PageSize == 1000 &&
                q.PageNumber == 1),
            cancellationToken), Times.Once);
    }
}
