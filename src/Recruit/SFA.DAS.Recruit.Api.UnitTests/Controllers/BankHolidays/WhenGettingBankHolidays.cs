using System;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetBankHolidays;
using SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes;
using SFA.DAS.Recruit.Application.Services;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.BankHolidays;

public class WhenGettingBankHolidays
{
    [Test, MoqAutoData]
    public async Task Then_Gets_BankHolidays_From_Mediator(
        GetBankHolidaysQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] BankHolidaysController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetBankHolidaysQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetBankHolidays() as ObjectResult;

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as BankHolidaysData;
        Assert.That(model, Is.Not.Null);
        model.Should().BeEquivalentTo(mediatorResult.Data);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_InternalServerError(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] BankHolidaysController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetBankHolidaysQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.GetBankHolidays() as StatusCodeResult;

        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}