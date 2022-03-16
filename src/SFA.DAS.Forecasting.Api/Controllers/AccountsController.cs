using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Approvals.Queries;

namespace SFA.DAS.Forecasting.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IMediator mediator, ILogger<AccountsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("with-cohorts")]
        public async Task<IActionResult> GetAccountsWithCohorts()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetAccountsWithCohortsQuery());
                var result = new GetAccountsWithCohortsResponse { AccountIds = queryResult.AccountIds };
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred getting accounts with cohorts");
                throw;
            }

        }
    }
}
