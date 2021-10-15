using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetJobRoles;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLevels;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetSectors;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    [Route("reference")]
    public class ReferenceController : ControllerBase
    {
        private readonly ILogger<ReferenceController> _logger;
        private readonly IMediator _mediator;

        public ReferenceController(ILogger<ReferenceController> logger, IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Route("levels")]
        [HttpGet]
        public async Task<IActionResult> Levels()
        {
            _logger.LogInformation("Getting Level reference data");
            var result = await _mediator.Send(new GetLevelsQuery());
            return Ok(result.ReferenceDataItems);
        }

        [Route("sectors")]
        [HttpGet]
        public async Task<IActionResult> Sectors()
        {
            _logger.LogInformation("Getting Sector reference data");
            var result = await _mediator.Send(new GetSectorsQuery());
            return Ok(result.ReferenceDataItems);
        }

        [Route("jobRoles")]
        [HttpGet]
        public async Task<IActionResult> JobRoles()
        {
            _logger.LogInformation("Getting Job Role reference data");
            var result = await _mediator.Send(new GetJobRolesQuery());
            return Ok(result.ReferenceDataItems);
        }
    }
}
