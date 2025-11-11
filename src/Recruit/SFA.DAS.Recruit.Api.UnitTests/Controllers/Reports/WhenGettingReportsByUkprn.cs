using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Report.Query.GetReportsByUkprn;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Reports;
[TestFixture]
internal class WhenGettingReportsByUkprn
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Report_From_Mediator(
        int ukprn,
        GetReportsByUkprnQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ReportsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetReportsByUkprnQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetByUkprn(ukprn) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetReportsByUkprnQueryResult;

        model.Should().NotBeNull();
        model!.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        int ukprn,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ReportsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetReportsByUkprnQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.GetByUkprn(ukprn) as BadRequestResult;

        controllerResult?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}
