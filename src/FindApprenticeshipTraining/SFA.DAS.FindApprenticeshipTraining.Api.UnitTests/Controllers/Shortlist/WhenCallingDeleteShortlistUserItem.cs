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
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Shortlist;

public class WhenCallingDeleteShortlistUserItem
{
    [Test, MoqAutoData]
    public async Task Then_Deletes_Shortlist_Item_For_User_From_Mediator(
        Guid id,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ShortlistsController controller)
    {
        var controllerResult = await controller.DeleteShortlistItemForUser(id) as ObjectResult;

        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
        mockMediator.Verify(mediator => mediator.Send(It.Is<DeleteShortlistItemCommand>(command => command.ShortlistId == id), It.IsAny<CancellationToken>()), Times.Once);
    }
}
