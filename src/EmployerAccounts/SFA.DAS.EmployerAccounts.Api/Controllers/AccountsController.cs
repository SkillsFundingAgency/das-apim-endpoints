using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Api.Models;
using SFA.DAS.EmployerAccounts.Application.Queries.GetCreateAccountTaskList;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAccountTaskList;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionCurrent;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionHistory;
using SFA.DAS.EmployerAccounts.Application.Queries.GetTasks;

namespace SFA.DAS.EmployerAccounts.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class AccountsController(IMediator mediator, ILogger<AccountsController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{hashedAccountId}/levy/english-fraction-history")]
    public async Task<IActionResult> GetEnglishFractionHistory(string hashedAccountId, [FromQuery] string empRef)
    {
        try
        {
            var result = await mediator.Send(new GetEnglishFractionHistoryQuery()
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
            logger.LogError(e, "Error getting english fraction history");
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("{hashedAccountId}/levy/english-fraction-current")]
    public async Task<IActionResult> GetEnglishFractionCurrent(string hashedAccountId, [FromQuery] string[] empRefs)
    {
        try
        {
            var result = await mediator.Send(new GetEnglishFractionCurrentQuery()
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
            logger.LogError(e, "Error getting english fraction current");
            return BadRequest();
        }
    }
    
    [HttpGet]
    [Route("{accountId}/create-task-list")]
    public async Task<IActionResult> GetCreateAccountTaskList([FromRoute] long accountId, [FromQuery] string hashedAccountId, [FromQuery] string userRef)
    {
        try
        {
            var result = await mediator.Send(new GetCreateAccountTaskListQuery(accountId, hashedAccountId, userRef));

            if (result == null)
            {
                logger.LogWarning("Returning not found for create account task list.");
                return NotFound();
            }

            var model = (GetCreateAccountTaskListResponse)result;

            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting create account task list.");
            return BadRequest();
        }
    }
    
    [HttpGet]
    [Route("{accountId}/account-task-list")]
    public async Task<IActionResult> GetEmployerAccountTaskList([FromRoute] long accountId, [FromQuery] string hashedAccountId)
    {
        try
        {
            var result = await mediator.Send(new GetEmployerAccountTaskListQuery
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
            logger.LogError(e, "Error getting account task list");
            return BadRequest();
        }
    }
    
    [HttpGet]
    [Route("{accountId}/teams")]
    public async Task<IActionResult> GetTeams([FromRoute] long accountId)
    {
        try
        {
            var result = await mediator.Send(new GetTasksQuery()
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
            logger.LogError(e, "Error getting tasks for account {Id}", accountId);
            return BadRequest();
        }
    }
}