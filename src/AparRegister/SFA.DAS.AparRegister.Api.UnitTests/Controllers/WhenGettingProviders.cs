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
using SFA.DAS.AparRegister.Api.ApiResponses;
using SFA.DAS.AparRegister.Api.Controllers;
using SFA.DAS.AparRegister.Application.ProviderRegister.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AparRegister.Api.UnitTests.Controllers
{
    public class WhenGettingProviders
    {
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Query_Is_Called_And_Response_Returned(
            GetProvidersQueryResult mediatorResponse,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProvidersController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetProvidersQuery>(), CancellationToken.None))
                .ReturnsAsync(mediatorResponse);

            var actual = await controller.GetProviders() as OkObjectResult;
            
            Assert.IsNotNull(actual);
            var actualModel = actual.Value as ProvidersApiResponse;
            actualModel.Should().BeEquivalentTo((ProvidersApiResponse)mediatorResponse);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Query_Is_Called_And_Error_Returned_If_Exception(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProvidersController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetProvidersQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetProviders() as StatusCodeResult;
            
            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}