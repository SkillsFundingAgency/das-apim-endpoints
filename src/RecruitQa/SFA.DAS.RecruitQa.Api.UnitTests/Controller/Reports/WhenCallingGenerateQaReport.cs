using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Application.Report.Queries.GenerateQaReport;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.Reports;

public class WhenCallingGenerateQaReport
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Report_Returned(
        Guid reportId,
        GenerateQaReportQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ReportsController controller)
    {
        mediator
            .Setup(x => x.Send(
                It.Is<GenerateQaReportQuery>(q => q.ReportId == reportId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.Generate(reportId) as OkObjectResult;

        actual.Should().NotBeNull();
        actual!.Value.Should().BeEquivalentTo(queryResult);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Internal_Server_Error_Returned(
        Guid reportId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ReportsController controller)
    {
        mediator
            .Setup(x => x.Send(
                It.Is<GenerateQaReportQuery>(q => q.ReportId == reportId),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.Generate(reportId) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
