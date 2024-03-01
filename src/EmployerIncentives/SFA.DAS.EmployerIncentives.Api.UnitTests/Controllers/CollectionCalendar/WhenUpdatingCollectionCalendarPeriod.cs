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
    public class WhenUpdatingCollectionCalendarPeriod
    {
        [Test, MoqAutoData]
        public async Task Then_UpdateCollectionPeriodCommand_Is_Sent(
            UpdateCollectionCalendarPeriodRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CollectionCalendarController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send<Unit>(
                    It.Is<UpdateCollectionCalendarPeriodCommand>(c =>
                        c.PeriodNumber == request.PeriodNumber &&
                        c.AcademicYear == request.AcademicYear
                    ), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var controllerResult = await controller.UpdateCollectionCalendarPeriod(request) as OkResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
