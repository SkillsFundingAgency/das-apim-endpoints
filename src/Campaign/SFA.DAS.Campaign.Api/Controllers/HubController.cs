using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Hub;
using SFA.DAS.Campaign.Application.Queries.PreviewHub;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class HubController : Controller
    {
        private readonly IMediator _mediator;

        public HubController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{hub}")]
        public async Task<IActionResult> GetHubAsync(string hub, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetHubQuery
            {
                Hub = hub

            }, cancellationToken);

            if (result.PageModel == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"Hub not found for {hub}"
                });
            }

            return Ok(new GetHubResponse
            {
                Hub = result.PageModel
            });

        }

        [HttpGet("preview/{hub}")]
        public async Task<IActionResult> GetPreviewHub(string hub)
        {
            var result = await _mediator.Send(new GetPreviewHubQuery
            {
                Hub = hub
            });

            if (result.PageModel == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"Preview hub not found for {hub}"
                });
            }

            return Ok(new GetHubResponse
            {
                Hub = result.PageModel
            });
        }
    }
}
