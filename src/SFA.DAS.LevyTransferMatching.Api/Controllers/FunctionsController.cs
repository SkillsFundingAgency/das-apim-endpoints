﻿using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge;
using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [Route("functions")]
    [ApiController]
    public class FunctionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FunctionsController> _logger;

        public FunctionsController(IMediator mediator, ILogger<FunctionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Route("application-approved")]
        [HttpPost]
        public async Task<IActionResult> ApplicationApproved(ApplicationApprovedRequest request)
        {
            try
            {
                var result = await _mediator.Send(new DebitPledgeCommand
                {
                    PledgeId = request.PledgeId,
                    Amount = request.Amount,
                    ApplicationId = request.ApplicationId
                });

                if (!result.StatusCode.IsSuccess())
                {
                    _logger.LogError($"Error attempting to Debit Pledge {result.ErrorContent}");
                }

                return new StatusCodeResult((int) result.StatusCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to Debit Pledge");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
