using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Models.Reports;
using SFA.DAS.RecruitQa.Application.Report.Commands.PostCreateReport;
using SFA.DAS.RecruitQa.Application.Report.Queries.GenerateQaReport;
using SFA.DAS.RecruitQa.Application.Report.Queries.GetReports;
using System.Net;

namespace SFA.DAS.RecruitQa.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ReportsController(IMediator mediator, ILogger<ReportsController> logger) : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetReports(CancellationToken token = default)
    {
        try
        {
            var result = await mediator.Send(new GetReportsQuery(), token);
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting QA reports");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("generate/{reportId:guid}")]
    public async Task<IActionResult> Generate(
        [FromRoute] Guid reportId,
        CancellationToken token = default)
    {
        try
        {
            var result = await mediator.Send(new GenerateQaReportQuery(reportId), token);
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error generating QA report for report Id {ReportId}", reportId);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateReport(
        [FromBody] PostCreateReportApiRequest request,
        CancellationToken token = default)
    {
        try
        {
            await mediator.Send(new PostCreateReportCommand
            {
                Id = request.Id,
                CreatedBy = request.CreatedBy,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Name = request.Name,
                UserId = request.UserId
            }, token);
            return Created();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating QA report");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
