using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Report.Query.GetReportDataById;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Reports;

[TestFixture]
internal class WhenGettingReportData
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Report_Data_From_Mediator(
        Guid reportId,
        GetReportDataByIdQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ReportsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetReportDataByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetData(reportId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetReportDataByIdQueryResult;
        model.Should().NotBeNull();
        model!.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_InternalServerError(
        Guid reportId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ReportsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetReportDataByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.GetData(reportId) as StatusCodeResult;

        controllerResult.Should().NotBeNull();
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
