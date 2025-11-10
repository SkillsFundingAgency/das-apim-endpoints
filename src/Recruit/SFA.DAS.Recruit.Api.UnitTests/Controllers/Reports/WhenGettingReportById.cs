using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Report.Query.GetReportById;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Reports;
[TestFixture]
internal class WhenGettingReportById
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Report_From_Mediator(
        Guid reportId,
        GetReportByIdQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ReportsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetReportByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetById(reportId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult?.Value as GetReportByIdQueryResult;

        model.Should().NotBeNull();
        model!.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        Guid reportId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ReportsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetReportByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.GetById(reportId) as BadRequestResult;

        controllerResult?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Test, MoqAutoData]
    public async Task And_NotFound_Then_Returns_Not_Request(
        Guid reportId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ReportsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetReportByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetReportByIdQueryResult(null));

        var controllerResult = await controller.GetById(reportId) as NotFoundResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}
