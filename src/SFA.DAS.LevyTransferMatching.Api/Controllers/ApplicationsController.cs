﻿using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplication;

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
        [ProducesResponseType((int)HttpStatusCode.OK)]
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
        public async Task<IActionResult> Application(int applicationId)
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
    }
}
