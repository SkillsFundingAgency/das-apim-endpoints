using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models.Reports;
using SFA.DAS.RecruitQa.Application.Report.Commands.PostCreateReport;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.Reports;

public class WhenCallingCreateReport
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Command_Is_Sent_And_Created_Returned(
        PostCreateReportApiRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ReportsController controller)
    {
        mediator
            .Setup(x => x.Send(It.IsAny<PostCreateReportCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var actual = await controller.CreateReport(request);

        actual.Should().BeOfType<CreatedResult>();
        mediator.Verify(x => x.Send(
            It.Is<PostCreateReportCommand>(c =>
                c.Id == request.Id &&
                c.Name == request.Name &&
                c.UserId == request.UserId &&
                c.CreatedBy == request.CreatedBy &&
                c.FromDate == request.FromDate &&
                c.ToDate == request.ToDate),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Internal_Server_Error_Returned(
        PostCreateReportApiRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ReportsController controller)
    {
        mediator
            .Setup(x => x.Send(It.IsAny<PostCreateReportCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.CreateReport(request) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
