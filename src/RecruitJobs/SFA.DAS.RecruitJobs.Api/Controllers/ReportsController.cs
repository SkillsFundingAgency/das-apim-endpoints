using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.Client;
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
    
    [HttpPost]
    [Route("cleanup")]
    public async Task<IResult> PostCleanupReports([FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    {
        await recruitApiClient.PostWithResponseCode<NullResponse>(new PostReportsCleanupApiRequest(), false);
        return TypedResults.Ok();
    }
}
