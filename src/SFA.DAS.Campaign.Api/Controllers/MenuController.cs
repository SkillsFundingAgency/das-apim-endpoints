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

        [HttpGet("{menuType}")]
        public async Task<IActionResult> GetMenuAsync(string menuType, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetMenuQuery
            {
                MenuType = menuType
            }, cancellationToken);

            if (result.PageModel == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"menu not found for {menuType}"
                });
            }

            return Ok(new GetMenuResponse
            {
                Menu = result.PageModel
            });

        }
    }
}
