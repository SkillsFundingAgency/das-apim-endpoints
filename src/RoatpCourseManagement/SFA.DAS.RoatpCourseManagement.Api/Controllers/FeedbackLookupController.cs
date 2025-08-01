using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RoatpCourseManagement.Application.FeedbackLookup.Queries.GetAnnualSummariesFeedback;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[Route("feedback")]
[ApiController]
public class FeedbackLookupController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [Route("lookup/annual-summarised")]
    public async Task<IActionResult> GetAnnualSummarisedFeedback([FromQuery] string academicYear, CancellationToken cancellationToken)
    {
        GetAnnualSummariesFeedbackQueryResult result = await _mediator.Send(new GetAnnualSummariesFeedbackQuery(academicYear), cancellationToken);
        return Ok(result);
    }
}
