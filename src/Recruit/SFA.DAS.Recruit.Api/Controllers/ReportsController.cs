using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models.Reports;
using SFA.DAS.Recruit.Application.Report.Command.PostCreateReport;
using SFA.DAS.Recruit.Application.Report.Query.GenerateReportsById;
using SFA.DAS.Recruit.Application.Report.Query.GetReportsByUkprn;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Recruit.Application.Report.Query.GetReportById;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ReportsController(IMediator mediator,
    ILogger<ReportsController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{reportId:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute, Required] Guid reportId,
        CancellationToken token = default)
    {
        try
        {
            var result = await mediator.Send(new GetReportByIdQuery(reportId), token);
            return result.Report is null 
                ? NotFound() 
                : Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting report by report Id");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("{ukprn:int}/provider")]
    public async Task<IActionResult> GetByUkprn(
        [FromRoute, Required] int ukprn,
        CancellationToken token = default)
    {
        try
        {
            var result = await mediator.Send(new GetReportsByUkprnQuery(ukprn), token);
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting reports by ukprn");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("generate/{reportId:guid}")]
    public async Task<IActionResult> Generate(
        [FromRoute, Required] Guid reportId,
        CancellationToken token = default)
    {
        try
        {
            var result = await mediator.Send(new GenerateReportsByReportIdQuery(reportId), token);
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error generating report by report id");
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
                OwnerType = request.OwnerType,
                Ukprn = request.Ukprn,
                UserId = request.UserId
            }, token);
            return Created();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error posting report");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}