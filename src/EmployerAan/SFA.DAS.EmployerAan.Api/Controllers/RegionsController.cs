﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Regions.Queries.GetRegions;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RegionsController : Controller
{
    private readonly IMediator _mediator;

    public RegionsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetRegionsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRegions(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetRegionsQuery(), cancellationToken));
    }
}