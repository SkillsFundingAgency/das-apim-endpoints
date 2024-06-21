using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Api.Controllers;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.Controllers;

public class WhenPostSendClosingSoonNotifications
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Processed(
        long vacancyRef,
        int daysUntilClosing,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LiveVacanciesController controller)
    {
        var actual = await controller.PostSendClosingSoonNotifications(vacancyRef, daysUntilClosing) as OkResult;

        actual.Should().NotBeNull();
        mediator.Verify(
            x => x.Send(
                It.Is<ProcessApplicationReminderCommand>(c =>
                    c.VacancyReference == vacancyRef && c.DaysUntilClosing == daysUntilClosing),
                CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_An_Exception_Then_InternalServerError_Returned(
        long vacancyRef,
        int daysUntilClosing,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LiveVacanciesController controller)
    {
        mediator.Setup(
            x => x.Send(
                It.Is<ProcessApplicationReminderCommand>(c =>
                    c.VacancyReference == vacancyRef && c.DaysUntilClosing == daysUntilClosing),
                CancellationToken.None)).ThrowsAsync(new Exception());
            
        var actual = await controller.PostSendClosingSoonNotifications(vacancyRef, daysUntilClosing) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}