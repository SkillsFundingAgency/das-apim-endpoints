﻿using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplication;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetAccepted;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationsController> _logger;

        public ApplicationsController(IMediator mediator, ILogger<ApplicationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("accounts/{accountId}/applications")]
        public async Task<IActionResult> GetApplications(long accountId)
        {
            var result = await _mediator.Send(new GetApplicationsQuery
            {
                AccountId = accountId
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("accounts/{accountId}/applications/{applicationId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Application(int applicationId)
        {
            try
            {
                var result = await _mediator.Send(new GetApplicationQuery()
                {
                    ApplicationId = applicationId,
                });

                if (result != null)
                {
                    return Ok((GetApplicationResponse)result);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get {nameof(Application)} result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("accounts/{accountId}/applications/{applicationId}/accept-funding")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AcceptFunding(long accountId, int applicationId, [FromBody] AcceptFundingRequest request)
        {
            var successfulOperation = await _mediator.Send(new AcceptFundingCommand
            {
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName,
                AccountId = accountId,
                ApplicationId = applicationId
            });

            if (successfulOperation)
            {
                return NoContent();
            }

            _logger.LogInformation($"Failed to accept funding for accountId: {accountId}, applicationId: {applicationId}");

            return BadRequest();
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("/accounts/{accountId}/applications/{applicationId}/accepted")]
        public async Task<IActionResult> Accepted(int applicationId)
        {
            try
            {
                var result = await _mediator.Send(new GetAcceptedQuery()
                {
                    ApplicationId = applicationId,
                });

                if (result != null)
                {
                    return Ok((GetAcceptedResponse)result);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get {nameof(Accepted)} result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
