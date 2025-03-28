using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipTraining.Application.AcademicYears.Queries.GetLatest;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public sealed class AcademicYearsController(IMediator _mediator) : ControllerBase
{

    [HttpGet]
    [Route("latest")]
    public async Task<IActionResult> GetAcademicYearsLatest()
    {
        var response = await _mediator.Send(new GetAcademicYearsLatestQuery());
        return Ok(response);
    }
}
