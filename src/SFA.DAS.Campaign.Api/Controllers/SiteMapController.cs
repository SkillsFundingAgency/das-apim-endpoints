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
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
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
            var landingPages = await _mediator.Send(new GetSiteMapQuery { ContentType = "landingPage" }, cancellationToken);
            var articles = await _mediator.Send(new GetSiteMapQuery { ContentType = "article" }, cancellationToken);
            var hubs = await _mediator.Send(new GetSiteMapQuery { ContentType = "hub" }, cancellationToken);

            var siteMapModel = new SiteMapPageModel
            {
                MainContent = new SiteMapPageModel.SiteMapContent()
            };

            siteMapModel.MainContent.Pages.AddRange(landingPages.MapModel.MainContent.Pages); 
            siteMapModel.MainContent.Pages.AddRange(articles.MapModel.MainContent.Pages);
            siteMapModel.MainContent.Pages.AddRange(hubs.MapModel.MainContent.Pages);

            return Ok(new GetSiteMapResponse
            {
                Map = siteMapModel
            });
        }
    }
}
