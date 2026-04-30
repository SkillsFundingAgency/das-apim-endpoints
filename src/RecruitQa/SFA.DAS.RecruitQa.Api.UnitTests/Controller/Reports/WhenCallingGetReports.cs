using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Application.Report.Queries.GetReports;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.Reports;

public class WhenCallingGetReports
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Handled_And_Reports_Returned(
        GetReportsQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ReportsController controller)
    {
        mediator
            .Setup(x => x.Send(It.IsAny<GetReportsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetReports() as OkObjectResult;

        actual.Should().NotBeNull();
        actual!.Value.Should().BeEquivalentTo(queryResult);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Internal_Server_Error_Returned(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ReportsController controller)
    {
        mediator
            .Setup(x => x.Send(It.IsAny<GetReportsQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.GetReports() as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
