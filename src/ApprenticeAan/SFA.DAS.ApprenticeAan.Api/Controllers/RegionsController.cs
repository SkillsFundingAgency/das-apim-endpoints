﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions;

namespace SFA.DAS.ApprenticeAan.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IMediator _mediator;

        public RegionsController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        ///     Get list of regions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(GetRegionsQueryResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRegions()
        {
            try
            {
                var result = await _mediator.Send(new GetRegionsQuery());

                if (result == null) return NotFound();

                return Ok(result);
            }
            catch { return BadRequest(); }
        }
    }
}