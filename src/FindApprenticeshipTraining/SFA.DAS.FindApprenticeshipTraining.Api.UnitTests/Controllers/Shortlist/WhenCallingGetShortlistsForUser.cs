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
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistsForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Shortlist;


public class WhenCallingGetShortlistsForUser
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Shortlist_For_User_From_Mediator(
        Guid shortlistUserId,
        GetShortlistsForUserResponse mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ShortlistsController sut)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetShortlistsForUserQuery>(query => query.UserId == shortlistUserId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await sut.GetShortlistsForUser(shortlistUserId) as OkObjectResult;

        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetShortlistsForUserResponse;
        model.Should().BeEquivalentTo(mediatorResult);
    }
}
