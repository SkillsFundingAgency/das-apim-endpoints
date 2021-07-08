using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.SiteMap;

namespace SFA.DAS.Campaign.Api.Controllers
{
    public class SiteMapController : Controller
    {
        private readonly IMediator _mediator;

        public SiteMapController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetFullSiteMapAsync(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetSiteMapQuery(), cancellationToken);

            return Ok(new GetSiteMapResponse
            {
                Map = result.MapModel
            });
        }
    }
}
