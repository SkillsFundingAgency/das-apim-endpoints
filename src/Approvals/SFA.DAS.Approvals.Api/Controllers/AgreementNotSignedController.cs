using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.AgreementNotSigned.Queries;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    public class AgreementNotSignedController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AgreementNotSignedController> _logger;

        public AgreementNotSignedController(IMediator mediator, ILogger<AgreementNotSignedController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{accountId}/unapproved/AgreementNotSigned")]
        public async Task<IActionResult> Get(long accountId)
        {
            try
            {
                _logger.LogInformation("Getting Account details {accountId}", accountId);
                var result = await _mediator.Send(new GetAgreementNotSignedQuery { AccountId = accountId});
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when getting Account {accountId}", accountId);
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}