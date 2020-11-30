using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.CollectionsCalendar;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.CollectionCalendar
{
    public class WhenActivatingCollectionCalendarPeriod
    {
        [Test, MoqAutoData]
        public async Task Then_ActivateCollectionPeriodCommand_Is_Sent(
            ActivateCollectionCalendarPeriodRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CollectionCalendarController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send<Unit>(
                    It.Is<ActivateCollectionCalendarPeriodCommand>(c =>
                        c.CollectionPeriodNumber == request.CollectionPeriodNumber &&
                        c.CollectionPeriodYear == request.CollectionPeriodYear
                    ), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var controllerResult = await controller.ActivateCollectionCalendarPeriod(request) as OkResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
