using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models.Reports;
using SFA.DAS.Recruit.Application.Report.Command.PostCreateReport;
using SFA.DAS.Recruit.Application.Report.Query.GenerateReportsById;
using SFA.DAS.Recruit.Application.Report.Query.GetReportsByUkprn;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ReportsController(IMediator mediator,
    ILogger<ReportsController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{ukprn:int}/provider")]
    public async Task<IActionResult> GetByUkprn(int ukprn)
    {
        try
        {
            var result = await mediator.Send(new GetReportsByUkprnQuery(ukprn));
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting reports by ukprn");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("{reportId:guid}")]
    public async Task<IActionResult> Generate(Guid reportId)
    {
        try
        {
            var result = await mediator.Send(new GenerateReportsByReportIdQuery(reportId));
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
    public async Task<IActionResult> CreateReport([FromBody] PostCreateReportApiRequest request)
    {
        try
        {
            await mediator.Send(new PostCreateReportCommand
            {
                CreatedBy = request.CreatedBy,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Name = request.Name,
                OwnerType = request.OwnerType,
                Ukprn = request.Ukprn,
                UserId = request.UserId
            });
            return NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error posting report");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
