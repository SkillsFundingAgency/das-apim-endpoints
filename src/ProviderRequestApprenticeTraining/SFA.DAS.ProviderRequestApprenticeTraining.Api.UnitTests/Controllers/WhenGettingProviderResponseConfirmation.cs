using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.UnitTests.Controllers
{
    public class WhenGettingProviderResponseConfirmation
    {
        [Test, MoqAutoData]
        public async Task Then_The_providerResponseConfirmation_Is_Returned_From_Mediator(
            GetProviderResponseConfirmationResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProviderResponsesController controller,
            Guid providerResponseId)
        {
            queryResult.EmployerRequests.ForEach(x =>
            {
                x.StandardReference = "ST0001";
                x.StandardLevel = 1;
                x.StandardTitle = "Title";
            });
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetProviderResponseConfirmationQuery>(), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetProviderResponseConfirmation(providerResponseId) as ObjectResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var confirmation = actual.Value as ProviderResponseConfirmation;
            confirmation.Ukprn.Should().Be(queryResult.Ukprn);
            confirmation.Email.Should().Be(queryResult.Email);
            confirmation.Phone.Should().Be(queryResult.Phone);
            confirmation.Website.Should().Be(queryResult.Website);
            confirmation.StandardLevel.Should().Be(queryResult.EmployerRequests.First().StandardLevel);
            confirmation.StandardTitle.Should().Be(queryResult.EmployerRequests.First().StandardTitle);
            confirmation.EmployerRequests.Count().Should().Be(queryResult.EmployerRequests.Count());
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProviderResponsesController controller,
            Guid providerResponseId)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetProviderResponseConfirmationQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetProviderResponseConfirmation(providerResponseId) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
