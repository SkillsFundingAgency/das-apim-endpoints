using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.UnitTests.Controllers
{
    public class WhenGettingEmployerRequest
    {
        [Test, MoqAutoData]
        public async Task Then_The_EmployerRequest_Is_Returned_From_Mediator(
            Guid employerRequestId,
            GetEmployerRequestResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetEmployerRequestQuery>(p => p.EmployerRequestId == employerRequestId), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetEmployerRequest(employerRequestId) as ObjectResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(queryResult.EmployerRequest);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid employerRequestId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetEmployerRequestQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetEmployerRequest(employerRequestId) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
