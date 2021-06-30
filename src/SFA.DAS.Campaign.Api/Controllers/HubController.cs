using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Articles;
using SFA.DAS.Campaign.Application.Queries.Hub;

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
    }
}
