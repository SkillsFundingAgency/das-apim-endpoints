using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.AccountLegalEntity;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountLegalEntityController : Controller
    {
        private readonly ILogger<ApprenticesController> _logger;
        private readonly IMediator _mediator;

        public AccountLegalEntityController(ILogger<ApprenticesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var result = await _mediator.Send(new GetAccountLegalEntityQuery { AccountLegalEntityId = id });
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Account legal entity data {id}", id);
                return BadRequest();
            }
        }
    }
}
