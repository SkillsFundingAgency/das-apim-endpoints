using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Shortlist;

public class WhenCallingPostToCreateShortlistUserItem
{
    [Test, MoqAutoData]
    public async Task Then_Creates_Shortlist_From_Mediator_Command(
        CreateShortListRequest shortlistRequest,
        PostShortListResponse expectedResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ShortlistsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<CreateShortlistForUserCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var controllerResult = await controller.CreateShortlistForUser(shortlistRequest);

        mockMediator
            .Verify(mediator => mediator.Send(
                It.Is<CreateShortlistForUserCommand>(command =>
                       command.ShortlistUserId == shortlistRequest.ShortlistUserId
                    && command.Ukprn.Equals(shortlistRequest.Ukprn)
                    && command.LocationName.Equals(shortlistRequest.LocationName)
                    && command.LarsCode.Equals(shortlistRequest.LarsCode)
                ), It.IsAny<CancellationToken>()));

        controllerResult.As<OkObjectResult>().Should().NotBeNull();
        controllerResult.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedResult);
    }
}
