using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Api.Models;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAccountTaskList;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionCurrent;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionHistory;
using SFA.DAS.EmployerAccounts.Application.Queries.GetTasks;

namespace SFA.DAS.EmployerAccounts.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IMediator mediator, ILogger<AccountsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{hashedAccountId}/levy/english-fraction-history")]
        public async Task<IActionResult> GetEnglishFractionHistory(string hashedAccountId, [FromQuery] string empRef)
        {
            try
            {
                var result = await _mediator.Send(new GetEnglishFractionHistoryQuery()
                {
                    HashedAccountId = hashedAccountId,
                    EmpRef = empRef
                });

                if (result == null)
                {
                    return NotFound();
                }

                var model = (GetEnglishFractionResponse)result;

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting english fraction history");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{hashedAccountId}/levy/english-fraction-current")]
        public async Task<IActionResult> GetEnglishFractionCurrent(string hashedAccountId, [FromQuery] string[] empRefs)
        {
            try
            {
                var result = await _mediator.Send(new GetEnglishFractionCurrentQuery()
                {
                    HashedAccountId = hashedAccountId,
                    EmpRefs = empRefs
                });

                if (result == null)
                {
                    return NotFound();
                }

                var model = (GetEnglishFractionResponse)result;

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting english fraction current");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{accountId}/account-task-list")]
        public async Task<IActionResult> GetEmployerAccountTaskList([FromRoute] long accountId, [FromQuery] string hashedAccountId)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerAccountTaskListQuery
                {
                    AccountId = accountId,
                    HashedAccountId = hashedAccountId
                });

                if (result == null)
                {
                    return NotFound();
                }

                var model = (GetEmployerAccountTaskListResponse)result;

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting account task list");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{accountId}/teams")]
        public async Task<IActionResult> GetTasks([FromRoute] long accountId)
        {
            try
            {
                var result = await _mediator.Send(new GetTasksQuery()
                {
                    AccountId = accountId
                });

                if (result == null)
                {
                    return NotFound();
                }

                var model = (GetTasksResponse)result;

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting tasks for account {accountId}");
                return BadRequest();
            }
        }
    }
}