using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetAccount;
using SFA.DAS.Recruit.Application.Queries.GetAccountLegalEntities;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class EmployerAccountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployerAccountsController> _logger;

        public EmployerAccountsController(IMediator mediator, ILogger<EmployerAccountsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{accountId}")]
        public async Task<IActionResult> GetById(long accountId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetAccountQuery {AccountId = accountId});

                var returnModel = new GetAccountResponse
                {
                    HashedAccountId = queryResult.HashedAccountId
                };

                return Ok(returnModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting account by id");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{hashedAccountId}/legalentities")]
        public async Task<IActionResult> GetAccountLegalEntities(string hashedAccountId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetAccountLegalEntitiesQuery {HashedAccountId = hashedAccountId});

                return Ok((GetAccountLegalEntitiesResponse)queryResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting account legal entities for account");
                return BadRequest();
            }
        }
        
    }
}