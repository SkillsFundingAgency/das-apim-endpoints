using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Api.Controllers;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.Application.Rules.Queries.GetAvailableDates;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.Api.UnitTests.Controllers.Rules
{
    public class WhenCallingGetAvailableDates
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Available_Dates_From_Mediator(
          int accountLegalEntityId,
          GetAvailableDatesResult mediatorResult,
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] RulesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAvailableDatesQuery>(query =>
                        query.AccountLegalEntityId == accountLegalEntityId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAvailableDates(accountLegalEntityId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAvailableDatesApiResponse;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult);
        }
    }
}
