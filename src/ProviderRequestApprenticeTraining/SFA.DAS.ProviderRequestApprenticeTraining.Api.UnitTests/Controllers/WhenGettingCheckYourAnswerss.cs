using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderWebsite;
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
    public class WhenGettingCheckYourAnswers
    {
        [Test, MoqAutoData]
        public async Task Then_CheckYourAnswers_Are_Returned_From_Mediator(
            GetEmployerRequestsByIdsResult requestsResult,
            GetProviderWebsiteResult websiteResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller,
            List<Guid> employerRequestids)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetEmployerRequestsByIdsQuery>(), CancellationToken.None))
                .ReturnsAsync(requestsResult);

            mockMediator
                .Setup(x => x.Send(It.IsAny<GetProviderWebsiteQuery>(), CancellationToken.None))
                .ReturnsAsync(websiteResult);

            var actual = await controller.GetCheckYourAnswers(123456, employerRequestids) as ObjectResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var checkAnswers = actual.Value as CheckYourAnswers;
            checkAnswers.StandardLevel.Should().Be(requestsResult.EmployerRequests.First().StandardLevel);
            checkAnswers.StandardReference.Should().Be(requestsResult.EmployerRequests.First().StandardReference);
            checkAnswers.StandardTitle.Should().Be(requestsResult.EmployerRequests.First().StandardTitle);
            checkAnswers.Requests.Count().Should().Be(requestsResult.EmployerRequests.Count());
            checkAnswers.Website.Should().Be(websiteResult.Website);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProvidersController controller,
            List<Guid> employerRequestids)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetEmployerRequestsByIdsQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetCheckYourAnswers(123456, employerRequestids) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown2(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProvidersController controller,
            List<Guid> employerRequestids)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetProviderWebsiteQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetCheckYourAnswers(123456, employerRequestids) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

    }
}
