using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Redirects;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class RedirectsController : Controller
    {
        private readonly IMediator _mediator;

        public RedirectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetRedirectsAsync(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetRedirectsQuery(), cancellationToken);

            return Ok(new GetRedirectsResponse
            {
                Redirects = result.Redirects
            });
        }
    }
}
