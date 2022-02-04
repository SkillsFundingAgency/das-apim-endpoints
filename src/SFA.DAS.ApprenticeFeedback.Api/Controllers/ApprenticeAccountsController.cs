using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ApprenticeAccountsController : Controller
    {
        private readonly IMediator _mediator;

        public ApprenticeAccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetApprenticeQuery { ApprenticeId = id });
               
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
