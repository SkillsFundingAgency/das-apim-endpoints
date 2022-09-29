#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Standard;
using SFA.DAS.Campaign.Application.Queries.Standards;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TrainingCoursesController : ControllerBase
    {
        private readonly ILogger<TrainingCoursesController> _logger;
        private readonly IMediator _mediator;

        public TrainingCoursesController(ILogger<TrainingCoursesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/StandardsBySector")]
        public async Task<IActionResult> GetStandards([FromQuery] string? sector)
        {
            try
            {
                var result = await _mediator.Send(new GetStandardsQuery { Sector = sector });

                return Ok(new GetStandardsResponse
                {
                    Standards = result.Standards != null ? result.Standards.Select(c => (GetStandardsResponseItem)c).ToList()
                        : new List<GetStandardsResponseItem>()
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting standards by sector {sector}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("/StandardByLarsCode")]
        public async Task<IActionResult> GetStandard([FromQuery] int larsCode)
        {
            try
            {
                var result = await _mediator.Send(new GetStandardQuery { LarsCode = larsCode });

                return Ok(new GetStandardByLarsCodeResponse
                {
                    Standard = (GetStandardResponse)result.Standard
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting standards by LarsCode {larsCode}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}