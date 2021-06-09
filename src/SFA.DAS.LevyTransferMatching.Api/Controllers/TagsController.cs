using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetJobRoles;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLevels;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetSectors;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("levels")]
        [HttpGet]
        public async Task<IActionResult> Levels()
        {
            var result = await _mediator.Send(new GetLevelsQuery());
            return Ok(result.Tags);
        }

        [Route("sectors")]
        [HttpGet]
        public async Task<IActionResult> Sectors()
        {
            var result = await _mediator.Send(new GetSectorsQuery());
            return Ok(result.Tags);
        }

        [Route("jobRoles")]
        [HttpGet]
        public async Task<IActionResult> JobRoles()
        {
            var result = await _mediator.Send(new GetJobRolesQuery());
            return Ok(result.Tags);
        }
    }
}
