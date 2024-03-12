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
using SFA.DAS.ApimDeveloper.Api.Controllers;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.CreateSubscriptionKey;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.Controllers
{
    public class WhenCreatingProductSubscription
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Created_Result_Returned(
            string accountType,
            string accountIdentifier,
            string productId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SubscriptionsController controller)
        {
            await controller.CreateProductSubscription(accountIdentifier, productId, accountType);
            
            mediator.Verify(x=>x.Send(It.Is<CreateSubscriptionKeyCommand>(c=>
                c.AccountIdentifier.Equals(accountIdentifier)
                && c.AccountType.Equals(accountType)
                && c.ProductId.Equals(productId)), CancellationToken.None), Times.Once);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_Internal_Server_Error_Response(
            string accountType,
            string accountIdentifier,
            string productId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SubscriptionsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<CreateSubscriptionKeyCommand>(),
                CancellationToken.None)).ThrowsAsync(new Exception());
            
            var actual = await controller.CreateProductSubscription(accountIdentifier, productId, accountType) as StatusCodeResult;
            
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
        
        
        [Test, MoqAutoData]
        public async Task Then_If_Validation_Error_Then_Request_Error_Response_Is_Returned(
            string accountType,
            string accountIdentifier,
            string productId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SubscriptionsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<CreateSubscriptionKeyCommand>(),
                CancellationToken.None)).ThrowsAsync(new HttpRequestContentException("error message", HttpStatusCode.BadRequest,"error"));
            
            var actual = await controller.CreateProductSubscription(accountIdentifier, productId, accountType) as ObjectResult;
            
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}