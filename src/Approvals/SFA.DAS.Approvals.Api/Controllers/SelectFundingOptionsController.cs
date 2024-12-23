using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.SelectFunding.Queries;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    public class SelectFundingOptionsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SelectFundingOptionsController> _logger;

        public SelectFundingOptionsController(IMediator mediator, ILogger<SelectFundingOptionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{accountId}/unapproved/add/select-funding")]
        public async Task<IActionResult> Get(long accountId)
        {
            try
            {
                _logger.LogInformation("Getting Select Funding information for Account {accountId}", accountId);
                var result = await _mediator.Send(new GetSelectFundingOptionsQuery {AccountId = accountId});
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when getting Select Funding information for Account {accountId}", accountId);
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}