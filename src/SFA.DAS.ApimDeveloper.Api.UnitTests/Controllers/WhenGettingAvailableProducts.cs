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
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.Controllers
{
    public class WhenGettingAvailableProducts
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Data_Returned(
            string accountType,
            GetApiProductsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SubscriptionsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetApiProductsQuery>(c => c.AccountType.Equals(accountType)),
                CancellationToken.None)).ReturnsAsync(mediatorResult);

            var actual = await controller.GetAvailableProducts(accountType) as OkObjectResult;
            
            Assert.IsNotNull(actual);
            var actualModel = actual.Value as ProductsApiResponse;
            actualModel.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_Internal_Server_Error_Response(
            string accountType,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SubscriptionsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetApiProductsQuery>(),
                CancellationToken.None)).ThrowsAsync(new Exception());
            
            var actual = await controller.GetAvailableProducts(accountType) as StatusCodeResult;
            
            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}