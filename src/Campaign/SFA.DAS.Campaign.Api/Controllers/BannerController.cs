using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Banner;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BannerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBannerAsync(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetBannerQuery(), cancellationToken);

            return Ok(new GetBannerResponse
            {
               Banner = result.PageModel
            });
        }
    }
}
