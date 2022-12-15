using System;
using System.Linq;
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
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistForUser;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Shortlist
{
    public class WhenCallingGetAllForUser
    {
        // [Test, MoqAutoData]
        //  public async Task Then_Gets_Shortlist_For_User_From_Mediator(
        //      Guid shortlistUserId,
        //      GetShortlistForUserResult mediatorResult,
        //      [Frozen] Mock<IMediator> mockMediator,
        //      [Greedy] ShortlistController controller)
        //  {
        //      mockMediator
        //          .Setup(mediator => mediator.Send(
        //              It.Is<GetShortlistForUserQuery>(query => query.ShortlistUserId == shortlistUserId),
        //              It.IsAny<CancellationToken>()))
        //          .ReturnsAsync(mediatorResult);
        //
        //      var controllerResult = await controller.GetAllForUser(shortlistUserId) as ObjectResult;
        //
        //      controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        //      var model = controllerResult.Value as GetShortlistForUserResponse;
        //      model!.Shortlist.Should().BeEquivalentTo(mediatorResult.Shortlist.Select(item => (GetShortlistItem)item));
        //  }

        //MFCMFC

         [Test]
         public async Task Then_Gets_Shortlist_For_User_From_Mediator()
         {
             var mockMediator = new Mock<IMediator>();
             var shortlistUserId = Guid.NewGuid();
             var mediatorResult = new GetShortlistForUserResult();
             var controller = new ShortlistController(mockMediator.Object,null);

             mockMediator
                 .Setup(mediator => mediator.Send(
                     It.Is<GetShortlistForUserQuery>(query => query.ShortlistUserId == shortlistUserId),
                     It.IsAny<CancellationToken>()))
                 .ReturnsAsync(mediatorResult);

             var controllerResult = await controller.GetAllForUser(shortlistUserId) as ObjectResult;

             controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
             var model = controllerResult.Value as GetShortlistForUserResponse;
             model!.Shortlist.Should().BeEquivalentTo(mediatorResult.Shortlist.Select(item => (GetShortlistItem)item));
         }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            Guid shortlistUserId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetShortlistForUserQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetAllForUser(shortlistUserId) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}