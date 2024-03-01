using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Api.Controllers;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.Application.Transfers.Queries.GetTransferValidity;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.Api.UnitTests.Controllers.Transfers
{
    [TestFixture]
    public class WhenCallingGetTransferValidity
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Validity_From_Mediator(
            long senderId,
            long receiverId,
            int? pledgeApplicationId,
            GetTransferValidityQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TransfersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTransferValidityQuery>(query =>
                        query.SenderId == senderId && query.ReceiverId == receiverId && query.PledgeApplicationId == pledgeApplicationId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetTransferValidity(senderId, receiverId, pledgeApplicationId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTransferValidityResponse;
            Assert.That(model, Is.Not.Null);
            model.IsValid.Should().Be(mediatorResult.IsValid);
        }
    }
}
