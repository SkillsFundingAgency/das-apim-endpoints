using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models.Reports;
using SFA.DAS.Recruit.Application.Report.Command.PostCreateReport;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Reports;
[TestFixture]
internal class WhenPostingCreateReport
{
    [Test, MoqAutoData]
    public async Task Then_Request_Is_Handled_And_Mediator_Command_Sent(
        PostCreateReportApiRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ReportsController controller)
    {
        var actual = await controller.CreateReport(request) as CreatedResult;

        actual.Should().NotBeNull();
        mediator.Verify(x => x.Send(
                It.Is<PostCreateReportCommand>(c =>
                    c.Id == request.Id &&
                    c.Name == request.Name &&
                    c.CreatedBy == request.CreatedBy &&
                                        c.Ukprn == request.Ukprn && 
                    c.FromDate == request.FromDate &&
                    c.ToDate == request.ToDate &&
                    c.OwnerType == request.OwnerType &&
                    c.Ukprn == request.Ukprn && 
                    c.UserId == request.UserId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Thrown_InternalServerError_Returned(
        PostCreateReportApiRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ReportsController controller)
    {
        mediator.Setup(x => x.Send(
            It.Is<PostCreateReportCommand>(c =>
                c.Id == request.Id &&
                c.Name == request.Name &&
                c.CreatedBy == request.CreatedBy &&
                c.Ukprn == request.Ukprn &&
                c.FromDate == request.FromDate &&
                c.ToDate == request.ToDate &&
                c.OwnerType == request.OwnerType &&
                c.Ukprn == request.Ukprn &&
                c.UserId == request.UserId),
            It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        var actual = await controller.CreateReport(request) as StatusCodeResult;
        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
