using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Api.Controllers;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.DeleteSubscriptionKey;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.Controllers.Subscriptions
{
    public class WhenDeletingSubscriptionKey
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Sent(
            string id,
            string productId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SubscriptionsController controller)
        {
            mockMediator.Setup(x => x.Send(It.Is<DeleteSubscriptionKeyCommand>(c =>
                    c.AccountIdentifier.Equals(id)
                    && c.ProductId.Equals(productId)
                ), CancellationToken.None))
                .ReturnsAsync(Unit.Value);

            var actual = await controller.DeleteSubscription(id, productId) as IStatusCodeActionResult;

            actual!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            mockMediator.Verify(x => x.Send(It.Is<DeleteSubscriptionKeyCommand>(c =>
                    c.AccountIdentifier.Equals(id)
                    && c.ProductId.Equals(productId)
                ), CancellationToken.None),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_HttpException_It_Is_Returned(
            string errorContent,
            string id,
            string productId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SubscriptionsController controller)
        {
            mockMediator.Setup(x => x.Send(
                    It.IsAny<DeleteSubscriptionKeyCommand>(),
                    CancellationToken.None))
                .ThrowsAsync(new ValidationException(errorContent));

            var controllerResult = await controller.DeleteSubscription(id, productId) as IStatusCodeActionResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Error_A_ServerError_Is_Returned(
            string id,
            string productId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SubscriptionsController controller)
        {
            mockMediator.Setup(x => x.Send(
                    It.IsAny<DeleteSubscriptionKeyCommand>(),
                    CancellationToken.None))
                .ThrowsAsync(new ApplicationException());

            var controllerResult = await controller.DeleteSubscription(id, productId) as IStatusCodeActionResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}