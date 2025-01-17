using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.LevyTransferMatching.Queries.GetApprovedAccountApplication;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    public class SelectAcceptedLevyApplicationsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SelectAcceptedLevyApplicationsController> _logger;

        public SelectAcceptedLevyApplicationsController(IMediator mediator, ILogger<SelectAcceptedLevyApplicationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{accountId}/unapproved/add/select-funding/select-accepted-levy-connection")]
        public async Task<IActionResult> Get(long accountId)
        {
            try
            {
                _logger.LogInformation("Getting Levy Transfer Connections for Account {accountId}", accountId);
                var result = await _mediator.Send(new GetAcceptedEmployerAccountApplicationsQuery { EmployerAccountId = accountId});
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when getting Levy Transfer Connections for Account {accountId}", accountId);
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}