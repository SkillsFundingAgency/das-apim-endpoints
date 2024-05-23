using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.SelectProvider.Queries;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class SelectProviderController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SelectProviderController> _logger;

        public SelectProviderController(IMediator mediator, ILogger<SelectProviderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetProviderStatus([FromQuery]long accountLegalEntityId)
        {
            try
            {
                _logger.LogInformation("Getting Select Provider information");
                var result = await _mediator.Send(new GetSelectProviderQuery {AccountLegalEntityId = accountLegalEntityId});
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when getting Select Provider information");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}