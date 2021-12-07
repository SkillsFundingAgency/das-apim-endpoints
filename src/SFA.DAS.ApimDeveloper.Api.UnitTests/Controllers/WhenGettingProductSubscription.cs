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
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Api.Controllers;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProductSubscription;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.Controllers
{
    public class WhenGettingProductSubscription
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Data_Returned(
            string accountType,
            string accountIdentifier,
            string productId,
            GetApiProductSubscriptionQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SubscriptionsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetApiProductSubscriptionQuery>(c => 
                    c.AccountType.Equals(accountType)
                    && c.AccountIdentifier.Equals(accountIdentifier)
                    && c.ProductId.Equals(productId)
                ),
                CancellationToken.None)).ReturnsAsync(mediatorResult);

            var actual = await controller.GetProductSubscription(accountIdentifier, productId, accountType) as OkObjectResult;
            
            Assert.IsNotNull(actual);
            var actualModel = actual.Value as ProductSubscriptionApiResponseItem;
            actualModel.Id.Should().Be(mediatorResult.Product.Id);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Null_Returned_NotFound_Result_Returned(string accountType,
            string accountIdentifier,
            string productId,
            GetApiProductSubscriptionQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SubscriptionsController controller)
        {
            mediatorResult.Product = null;
            mediatorResult.Subscription = null;
            mediator.Setup(x => x.Send(It.Is<GetApiProductSubscriptionQuery>(c => 
                    c.AccountType.Equals(accountType)
                    && c.AccountIdentifier.Equals(accountIdentifier)
                    && c.ProductId.Equals(productId)
                ),
                CancellationToken.None)).ReturnsAsync(mediatorResult);

            var actual = await controller.GetProductSubscription(accountIdentifier, productId, accountType) as NotFoundResult;
            
            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_Internal_Server_Error_Response(
            string accountType,
            string accountIdentifier,
            string productId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SubscriptionsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetApiProductSubscriptionQuery>(),
                CancellationToken.None)).ThrowsAsync(new Exception());
            
            var actual = await controller.GetProductSubscription(accountIdentifier, productId, accountType) as StatusCodeResult;
            
            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}