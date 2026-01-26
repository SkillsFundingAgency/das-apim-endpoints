using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.Courses.Queries;
using SFA.DAS.Approvals.Application.TrainingCourses.Queries;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TrainingCoursesController(
        ILogger<TrainingCoursesController> logger,
        IMediator mediator)
        : ControllerBase
    {
        [HttpGet]
        [Route("standards")]
        public async Task<IActionResult> GetStandards()
        {
            try
            {
                var queryResult = await mediator.Send(new GetStandardsQuery());
                
                var model = new GetStandardsListResponse
                {
                    Standards = queryResult.Standards.Select(c=>(GetStandardResponse)c).ToList()
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error attempting to get list of standards");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("frameworks")]
        public async Task<IActionResult> GetFrameworks()
        {
            try
            {
                var queryResult = await mediator.Send(new GetFrameworksQuery());
                
                var model = new GetFrameworksListResponse
                {
                    Frameworks = queryResult.Frameworks.Select(c=>(GetFrameworkResponse)c).ToList()
                };
                
                return Ok(model);
            }
            catch (Exception e)
            {
                logger.LogError(e,"Error attempting to get list of frameworks");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("standards/{courseCode}")]
        public async Task<IActionResult> GetStandard(string courseCode)
        {
            try
            {
                var queryResult = await mediator.Send(new GetStandardQuery(courseCode));

                if (queryResult == null)
                {
                    return NotFound();
                }

                var model = (GetStandardResponse)queryResult;
                return Ok(model);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error attempting to get list of standards");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{courseCode}/funding-band")]
        public async Task<IActionResult> GetFundingBand(string courseCode, [FromQuery] DateTime? startDate)
        {
            try
            {
                logger.LogInformation("Getting Funding Band details for course {courseId} with start date of {startDate}", courseCode, startDate);
                var result = await mediator.Send(new GetFundingBandQuery { CourseCode = courseCode, StartDate = startDate });
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error gettingFunding Band details {courseId}", courseCode);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("courses")]
        public async Task<IActionResult> GetCourses()
        {
            try
            {
                var queryResult = await mediator.Send(new GetCoursesQuery());

                var model = new GetCoursesResponse
                {
                    TrainingProgrammes = queryResult.TrainingProgrammes
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error attempting to get all courses");
                return BadRequest();
            }
        }
    }
}