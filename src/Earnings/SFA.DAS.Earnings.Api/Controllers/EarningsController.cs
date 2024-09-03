using AutoFixture;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Earnings.Application.Earnings;

namespace SFA.DAS.Earnings.Api.Controllers;

[ApiController]
[Route("")]
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
    [Route("{collectionPeriod}/{ukprn}")]
    public async Task<IActionResult> GetAll(byte collectionPeriod, long ukprn)
    {
        try
        {
            var queryResult = await _mediator.Send(new GetAllEarningsQuery{ Ukprn = ukprn });

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