﻿using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Api.Models;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionCurrent;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionHistory;

namespace SFA.DAS.EmployerFinance.Api.Controllers
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
                var response = await _mediator.Send(new GetEnglishFractionHistoryQuery()
                {
                    HashedAccountId = hashedAccountId,
                    EmpRef = empRef
                });

                var model = (GetEnglishFractionResponse)response;

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
                var response = await _mediator.Send(new GetEnglishFractionCurrentQuery()
                {
                    HashedAccountId = hashedAccountId,
                    EmpRefs = empRefs
                });

                var model = (GetEnglishFractionResponse)response;

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting english fraction current");
                return BadRequest();
            }
        }
    }
}