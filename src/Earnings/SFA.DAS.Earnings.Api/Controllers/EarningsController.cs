using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Earnings.Application.Earnings;

namespace SFA.DAS.Earnings.Api.Controllers;

[ApiController]
[Route("earnings")]
public class EarningsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EarningsController> _logger;

    public EarningsController(IMediator mediator, ILogger<EarningsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets all earnings data.
    /// </summary>
    /// <returns>All earnings data in the format of an FM36Learner array.</returns>
    [HttpGet]
    [Route("/learners/{ukprn}/{collectionYear}/{collectionPeriod}")]
    public async Task<IActionResult> GetAll(long ukprn, int collectionYear, byte collectionPeriod)
    {
        try
        {
            var queryResult = await _mediator.Send(new GetAllEarningsQuery(ukprn, collectionYear, collectionPeriod));

            var model = queryResult.FM36Learners;

            return Ok(model);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error attempting to get all earnings");
            return BadRequest();
        }
    }
}