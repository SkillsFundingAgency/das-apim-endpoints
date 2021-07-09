using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Menu;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMediator _mediator;


        public MenuController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetMenuAsync(CancellationToken cancellationToken = default)
        {
            var topLevelMenuResult = await _mediator.Send(new GetMenuQuery { MenuType = "TopLevel"}, cancellationToken);
            var apprenticesMenuResult = await _mediator.Send(new GetMenuQuery { MenuType = "Apprentices" }, cancellationToken);
            var employersMenuResult = await _mediator.Send(new GetMenuQuery { MenuType = "Employers" }, cancellationToken);
            var influencersMenuResult = await _mediator.Send(new GetMenuQuery { MenuType = "Influencers" }, cancellationToken);

            var menuModel = new MenuPageModel
            {
                MainContent = new MenuPageModel.MenuPageContent
                {
                    Apprentices = apprenticesMenuResult.PageModel.MainContent,
                    Employers = employersMenuResult.PageModel.MainContent,
                    Influencers = influencersMenuResult.PageModel.MainContent,
                    TopLevel = topLevelMenuResult.PageModel.MainContent
                }
            };
        
            return Ok(new GetMenuResponse
            {
                Menu = menuModel
            });
        }
    }
}
