using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Api.ApiRequests;
using SFA.DAS.ApimDeveloper.Api.Controllers;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.RenewSubscriptionKey;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.Controllers.Subscriptions
{
    public class WhenRenewingSubscriptionKey
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Sent(
            RenewSubscriptionKeyApiRequest apiRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SubscriptionsController controller)
        {
            mockMediator.Setup(x => x.Send(It.Is<RenewSubscriptionKeyCommand>(c =>
                    c.AccountIdentifier.Equals(apiRequest.AccountIdentifier)
                    && c.ProductId.Equals(apiRequest.ProductId)
                ), CancellationToken.None))
                .ReturnsAsync(Unit.Value);

            var actual = await controller.RenewSubscriptionKey(apiRequest) as IStatusCodeActionResult;

            actual!.StatusCode.Should().Be((int) HttpStatusCode.NoContent);
            /*mockMediator.Verify(x => x.Send(It.Is<RenewSubscriptionKeyCommand>(c =>
                    c.AccountIdentifier.Equals(accountIdentifier)
                    && c.ProductId.Equals(productId)
                ), CancellationToken.None),
                Times.Once);*/
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_HttpException_It_Is_Returned(
            string errorContent,
            RenewSubscriptionKeyApiRequest apiRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SubscriptionsController controller)
        {
            mockMediator.Setup(x => x.Send(
                    It.IsAny<RenewSubscriptionKeyCommand>(), 
                    CancellationToken.None))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest, errorContent));
            
            var controllerResult = await controller.RenewSubscriptionKey(apiRequest) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            controllerResult.Value.Should().Be(errorContent);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Error_A_ServerError_Is_Returned(
            RenewSubscriptionKeyApiRequest apiRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SubscriptionsController controller)
        {
            mockMediator.Setup(x => x.Send(
                    It.IsAny<RenewSubscriptionKeyCommand>(), 
                    CancellationToken.None))
                .ThrowsAsync(new ApplicationException());
            
            var controllerResult = await controller.RenewSubscriptionKey(apiRequest) as IStatusCodeActionResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}