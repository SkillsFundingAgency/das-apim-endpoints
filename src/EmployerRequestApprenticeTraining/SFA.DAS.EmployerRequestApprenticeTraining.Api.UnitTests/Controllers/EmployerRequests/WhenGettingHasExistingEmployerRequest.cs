using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetActiveEmployerRequest;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequests
{
    public class WhenGettingHasExistingEmployerRequest
    {
        [Test, MoqAutoData]
        public async Task And_EmployerRequest_ByAccountId_And_StandardReference_Exists_Then_True_Is_Returned(
           long accountId,
           string standardReference,
           GetActiveEmployerRequestResult queryResult,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] EmployerRequestsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetActiveEmployerRequestQuery>(p => p.AccountId == accountId && p.StandardReference == standardReference), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.HasExistingEmployerRequest(accountId, standardReference) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(true);
        }

        [Test, MoqAutoData]
        public async Task And_EmployerRequest_ByAccountId_And_StandardReference_NotExists_Then_False_Is_Returned(
           long accountId,
           string standardReference,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] EmployerRequestsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetActiveEmployerRequestQuery>(), CancellationToken.None))
                .ReturnsAsync(new GetActiveEmployerRequestResult { EmployerRequest = null});

            var actual = await controller.HasExistingEmployerRequest(accountId, standardReference) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(false);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            long accountId,
           string standardReference,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetActiveEmployerRequestQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.HasExistingEmployerRequest(accountId, standardReference) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
