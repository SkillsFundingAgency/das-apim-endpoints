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
using SFA.DAS.ApimDeveloper.Application.ApiProducts.Queries.GetApiProduct;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.Controllers
{
    public class WhenGettingAvailableProducts
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Data_Returned(
            string productId,
            GetApiProductQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProductsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetApiProductQuery>(c => 
                    c.ProductName.Equals(productId)
                ),
                CancellationToken.None)).ReturnsAsync(mediatorResult);

            var actual = await controller.GetApiProduct(productId) as OkObjectResult;
            
            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Value as ProductApiResponseItem;
            actualModel.Should().BeEquivalentTo(mediatorResult.Product);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Null_Returned_Then_Not_Found_Result_Returned(
            string productId,
            GetApiProductQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProductsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetApiProductQuery>(),
                CancellationToken.None)).ReturnsAsync(new GetApiProductQueryResult
            {
                Product = null
            });
            
            var actual = await controller.GetApiProduct(productId) as StatusCodeResult;
            
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_Internal_Server_Error_Response(
            string productId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProductsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetApiProductQuery>(),
                CancellationToken.None)).ThrowsAsync(new Exception());
            
            var actual = await controller.GetApiProduct(productId) as StatusCodeResult;
            
            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}