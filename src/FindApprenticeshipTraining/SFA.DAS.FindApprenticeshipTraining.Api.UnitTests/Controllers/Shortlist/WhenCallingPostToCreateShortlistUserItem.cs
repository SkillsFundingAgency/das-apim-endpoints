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
using SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Shortlist
{
    public class WhenCallingPostToCreateShortlistUserItem
    {
        [Test, MoqAutoData]
        public async Task Then_Creates_Shortlist_From_Mediator_Command(
            CreateShortListRequest shortlistRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            var controllerResult = await controller.CreateShortlistForUser(shortlistRequest) as CreatedResult;

            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<CreateShortlistForUserCommand>(command =>
                        command.ShortlistUserId == shortlistRequest.ShortlistUserId
                        && command.Lat.Equals(shortlistRequest.Lat)
                        && command.Lon.Equals(shortlistRequest.Lon)
                        && command.Ukprn.Equals(shortlistRequest.Ukprn)
                        && command.LocationDescription.Equals(shortlistRequest.LocationDescription)
                        && command.StandardId.Equals(shortlistRequest.StandardId)
                    ), It.IsAny<CancellationToken>()));

            controllerResult.As<CreatedResult>().Should().NotBeNull();
            controllerResult.Value.Should().BeEquivalentTo(new { userid = shortlistRequest.ShortlistUserId});

        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_HttpException_It_Is_Returned(
            string errorContent,
            CreateShortListRequest shortlistRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateShortlistForUserCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest,errorContent));
            
            var controllerResult = await controller.CreateShortlistForUser(shortlistRequest) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            controllerResult.Value.Should().Be(errorContent);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Error_A_Bad_Request_Is_Returned(
            CreateShortListRequest shortlistRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateShortlistForUserCommand>(),
                    It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            
            var controllerResult = await controller.CreateShortlistForUser(shortlistRequest) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
        }
    }
}