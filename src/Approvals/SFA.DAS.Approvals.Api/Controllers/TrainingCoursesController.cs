﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.Courses.Queries;
using SFA.DAS.Approvals.Application.TrainingCourses.Queries;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TrainingCoursesController : ControllerBase
    {
        private readonly ILogger<TrainingCoursesController> _logger;
        private readonly IMediator _mediator;

        public TrainingCoursesController(
            ILogger<TrainingCoursesController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("standards")]
        public async Task<IActionResult> GetStandards()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetStandardsQuery());
                
                var model = new GetStandardsListResponse()
                {
                    Standards = queryResult.Standards.Select(c=>(GetStandardResponse)c).ToList()
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of standards");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("frameworks")]
        public async Task<IActionResult> GetFrameworks()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetFrameworksQuery());
                
                var model = new GetFrameworksListResponse
                {
                    Frameworks = queryResult.Frameworks.Select(c=>(GetFrameworkResponse)c).ToList()
                };
                
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error attempting to get list of frameworks");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("standards/{courseCode}")]
        public async Task<IActionResult> GetStandard(string courseCode)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetStandardQuery(courseCode));

                if (queryResult == null)
                {
                    return NotFound();
                }

                var model = (GetStandardResponse)queryResult;
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of standards");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{courseCode}/funding-band")]
        public async Task<IActionResult> GetFundingBand(string courseCode, [FromQuery] DateTime? startDate)
        {
            try
            {
                _logger.LogInformation("Getting Funding Band details for course {courseId} with start date of {startDate}", courseCode, startDate);
                var result = await _mediator.Send(new GetFundingBandQuery { CourseCode = courseCode, StartDate = startDate });
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error gettingFunding Band details {courseId}", courseCode);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}