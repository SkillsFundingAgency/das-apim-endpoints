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
using SFA.DAS.Campaign.Extensions;
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
            var menuModel = await _mediator.RetrieveMenu(cancellationToken: cancellationToken);
        
            return Ok(new GetMenuResponse
            {
                Menu = menuModel
            });
        }
    }
}
