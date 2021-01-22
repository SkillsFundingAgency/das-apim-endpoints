﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Standards;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TrainingCoursesController : ControllerBase
    {
        private readonly ILogger<TrainingCoursesController> _logger;
        private readonly IMediator _mediator;

        public TrainingCoursesController (ILogger<TrainingCoursesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetStandardsBySector([FromQuery]string sector)
        {
            try
            {
                var result = await _mediator.Send(new GetStandardsQuery {Sector = sector});

                return Ok(new GetStandardsResponse
                {
                    Standards = result.Standards.Select(c => (GetStandardsResponseItem)c).ToList()
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting standards by sector {sector}");
                return BadRequest();
            }
        }
    }
}