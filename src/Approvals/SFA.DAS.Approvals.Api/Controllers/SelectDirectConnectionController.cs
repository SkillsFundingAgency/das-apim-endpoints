using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.SelectDirectTransferConnection.Queries;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    public class SelectDirectConnectionController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SelectDirectConnectionController> _logger;

        public SelectDirectConnectionController(IMediator mediator, ILogger<SelectDirectConnectionController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{accountId}/unapproved/add/select-funding/select-direct-connection")]
        public async Task<IActionResult> Get(long accountId)
        {
            try
            {
                _logger.LogInformation("Getting Direct Transfer Connections for Account {accountId}", accountId);
                var result = await _mediator.Send(new GetSelectDirectTransferConnectionQuery {AccountId = accountId});
                return Ok((GetSelectDirectConnectionResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when getting Direct Transfer Connections for Account {accountId}", accountId);
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}