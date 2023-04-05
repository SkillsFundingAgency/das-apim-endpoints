﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAddresses([FromQuery] string query)
    {
        var queryResponse = await _mediator.Send(new GetAddressesQuery(query));

        if (!queryResponse.Addresses.Any()) return NotFound();

        return Ok(queryResponse);
    }
}