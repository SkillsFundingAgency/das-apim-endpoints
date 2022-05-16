using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllCoursesQuery;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllStandards;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class StandardsController : ControllerBase
    {
        private readonly ILogger<StandardsController> _logger;
        private readonly IMediator _mediator;

        public StandardsController(ILogger<StandardsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{ukprn}")]
        public async Task<IActionResult> GetAllStandards(int ukprn)
        {
            if (ukprn <= 0)
            {
                _logger.LogInformation("Invalid ukprn number {ukprn} ukprn number has to be a positive number", ukprn);
                return BadRequest();
            }

            _logger.LogInformation("Get Standards for ukprn number {ukprn}", ukprn);
            try
            {
                var result = await _mediator.Send(new GetAllCoursesQuery(ukprn));

                if (result == null)
                {
                    _logger.LogInformation("Standards data not found for ukprn number {ukprn}", ukprn);
                    return NotFound();
                }

                _logger.LogInformation("Standards data found for ukprn number {ukprn}", ukprn);
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred trying to retrieve Standards data for ukprn number {ukprn}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("lookup/standards")]
        public async Task<IActionResult> GetAllStandards()
        {
            _logger.LogInformation("Get all active standards");
            try
            {
                var result = await _mediator.Send(new GetAllStandardsQuery());

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError($"Active standards not gathered, status code {result.StatusCode}, Error content:[{result.ErrorContent}]");
                    return StatusCode((int)result.StatusCode, result.ErrorContent);
                }

                _logger.LogInformation("Active standards gathered");
                return Ok(result.Body);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred trying to retrieve active standards");
                return BadRequest();
            }
        }
    }
}
