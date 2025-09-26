using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItem;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistCountForUser;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Shortlist;

public class WhenCallingDeleteShortlistUserItem
{
    [Test, MoqAutoData]
    public async Task Then_Deletes_Shortlist_Item_For_User_From_Mediator(
        Guid id,
        DeleteShortlistItemCommandResult expected,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ShortlistsController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(It.IsAny<DeleteShortlistItemCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        var controllerResult = await controller.DeleteShortlistItemForUser(id) as ObjectResult;

        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
        controllerResult.As<AcceptedResult>().Value.Should().Be(expected);
        mockMediator.Verify(mediator => mediator.Send(It.Is<DeleteShortlistItemCommand>(command => command.ShortlistId == id), It.IsAny<CancellationToken>()), Times.Once);
    }
}

public class WhenCallingGetShortlistCountForUser
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Shortlist_Count_For_User_From_Mediator(
        Guid id,
        GetShortlistCountForUserQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ShortlistsController sut)
    {
        mockMediator.Setup(mediator => mediator.Send(It.Is<GetShortlistCountForUserQuery>(command => command.UserId == id), It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResult);

        var controllerResult = await sut.GetShortlistCountForUser(id) as ObjectResult;

        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        mockMediator.Verify(mediator => mediator.Send(It.Is<GetShortlistCountForUserQuery>(command => command.UserId == id), It.IsAny<CancellationToken>()), Times.Once);
        controllerResult.Value.As<GetShortlistCountForUserQueryResult>().Count.Should().Be(mediatorResult.Count);
    }
}
