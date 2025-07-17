using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipTraining.Application.Standards.Queries.GetStandards;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class StandardsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(GetStandardsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStandards()
    {
        var result = await _mediator.Send(new GetStandardsQuery());
        return Ok(result);
    }
}
