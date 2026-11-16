using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Homepage;
using SFA.DAS.Campaign.Application.Queries.PreviewHomepage;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomepageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HomepageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetHomepageAsync(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetHomepageQuery(), cancellationToken);

            if (result?.PageModel == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = "Homepage content not found."
                });
            }

            return Ok(new GetHomepageResponse
            {
                Homepage = result.PageModel
            });
        }

        [HttpGet("preview")]
        public async Task<IActionResult> GetPreviewHomepageAsync(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetPreviewHomepageQuery(), cancellationToken);

            if (result?.PageModel == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = "Preview homepage content not found."
                });
            }

            return Ok(new GetHomepageResponse
            {
                Homepage = result.PageModel
            });
        }
    }
}
