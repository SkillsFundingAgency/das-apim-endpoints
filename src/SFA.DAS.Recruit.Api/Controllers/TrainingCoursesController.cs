﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetFrameworks;
using SFA.DAS.Recruit.Application.Queries.GetStandards;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TrainingCoursesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TrainingCoursesController> _logger;

        public TrainingCoursesController (IMediator mediator, ILogger<TrainingCoursesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpGet]
        [Route("standards")]
        public async Task<IActionResult> GetStandards()
        {
            try
            {
                var result = await _mediator.Send(new GetStandardsQuery());
                
                var response = new GetStandardsListResponse
                {
                    Standards = result.Standards.Select(c=>(GetStandardResponse)c)
                };
                
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting list of standards");
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("frameworks")]
        public async Task<IActionResult> GetFrameworks()
        {
            try
            {
                var result = await _mediator.Send(new GetFrameworksQuery());
                
                var response = new GetFrameworksListResponse
                {
                    Frameworks = result.Frameworks.Select(c=>(GetFrameworkResponse)c)
                };
                
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting list of frameworks");
                return BadRequest();
            }
        }
    }
}