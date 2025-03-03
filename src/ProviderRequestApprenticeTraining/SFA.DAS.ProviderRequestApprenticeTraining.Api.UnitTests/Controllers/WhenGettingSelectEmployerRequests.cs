using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSettings;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.UnitTests.Controllers
{
    public class WhenGettingSelectEmployerRequests
    {
        [Test, MoqAutoData]
        public async Task Then_The_SelectEmployerRequests_Are_Returned_From_Mediator(
            GetSelectEmployerRequestsResult queryResult,
            GetSettingsResult settingsResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller,
            long ukprn,
            string standardReference)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetSelectEmployerRequestsQuery>(), CancellationToken.None))
                .ReturnsAsync(queryResult);

            mockMediator
                .Setup(x => x.Send(It.IsAny<GetSettingsQuery>(), CancellationToken.None))
                .ReturnsAsync(settingsResult);

            var actual = await controller.GetSelectEmployerRequests(standardReference, ukprn) as ObjectResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var returnedRequests = actual.Value as SelectEmployerRequests;
            returnedRequests.StandardLevel.Should().Be(queryResult.SelectEmployerRequests.FirstOrDefault().StandardLevel);
            returnedRequests.StandardTitle.Should().Be(queryResult.SelectEmployerRequests.FirstOrDefault().StandardTitle);
            returnedRequests.StandardReference.Should().Be(queryResult.SelectEmployerRequests.FirstOrDefault().StandardReference);
            returnedRequests.EmployerRequests.Count.Should().Be(queryResult.SelectEmployerRequests.Count());
            returnedRequests.ExpiryAfterMonths.Should().Be(settingsResult.ExpiryAfterMonths);
            returnedRequests.RemovedAfterRequestedMonths.Should().Be(settingsResult.RemovedAfterRequestedMonths);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid employerRequestId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProvidersController controller,
            long ukprn,
            string standardReference)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetSelectEmployerRequestsQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetSelectEmployerRequests(standardReference, ukprn) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
