using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.UnitTests.Controllers
{
    public class WhenGettingEmployerRequestsByIds
    {
        [Test, MoqAutoData]
        public async Task Then_The_EmployerRequests_Are_Returned_From_Mediator(
            GetEmployerRequestsByIdsResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestsController controller,
            List<Guid> employerRequestids)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetEmployerRequestsByIdsQuery>(), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetEmployerRequestsByIds(employerRequestids) as ObjectResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var returnedRequests = actual.Value as EmployerRequests;
            returnedRequests.StandardLevel.Should().Be(queryResult.EmployerRequests.First().StandardLevel);
            returnedRequests.StandardReference.Should().Be(queryResult.EmployerRequests.First().StandardReference);
            returnedRequests.StandardTitle.Should().Be(queryResult.EmployerRequests.First().StandardTitle);
            returnedRequests.Requests.Count().Should().Be(queryResult.EmployerRequests.Count());
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller,
            List<Guid> employerRequestids)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetSelectEmployerRequestsQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetEmployerRequestsByIds(employerRequestids) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
