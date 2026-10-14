using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.RecruitJobs.Api.Core.BackgroundWork;
using SFA.DAS.RecruitJobs.Handlers;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ReportsController(ILogger<ReportsController> logger) : ControllerBase
{
    [HttpPost]
    [Route("generate/{id:guid}")]
    public IResult PostGenerateReport(
        [FromServices] IBackgroundWorkQueue backgroundWorkQueue,
        [FromRoute] Guid id)
    {
        backgroundWorkQueue.Enqueue($"GenerateReport:{id}", (serviceProvider, cancellationToken) =>
            serviceProvider.GetRequiredService<IGenerateReportHandler>().HandleAsync(id, cancellationToken));

        logger.LogInformation("RecruitJobs: Queued report generation for report Id: {ReportId}", id);
        return TypedResults.Accepted((string)null);
    }
}
