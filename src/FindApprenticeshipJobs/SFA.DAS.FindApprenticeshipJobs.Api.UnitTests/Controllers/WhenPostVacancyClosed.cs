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

public class WhenPostVacancyClosed
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Processed(
        long vacancyRef,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LiveVacanciesController controller)
    {
        var actual = await controller.PostVacancyClosed(vacancyRef) as OkResult;

        actual.Should().NotBeNull();
        mediator.Verify(
            x => x.Send(
                It.Is<ProcessVacancyClosedEarlyCommand>(c =>
                    c.VacancyReference == vacancyRef ),
                CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_An_Exception_Then_InternalServerError_Returned(
        long vacancyRef,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LiveVacanciesController controller)
    {
        mediator.Setup(
            x => x.Send(
                It.Is<ProcessVacancyClosedEarlyCommand>(c =>
                    c.VacancyReference == vacancyRef),
                CancellationToken.None)).ThrowsAsync(new Exception());
            
        var actual = await controller.PostVacancyClosed(vacancyRef) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}