﻿using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication;
using SFA.DAS.EmployerIncentives.Application.Commands.CreateApplication;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApplication;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(IMediator mediator, ILogger<ApplicationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Route("/accounts/{accountId}/applications")]
        public async Task<IActionResult> PostApplication(CreateApplicationRequest request)
        {
            var applicationId = await _mediator.Send(new CreateApplicationCommand(request.ApplicationId, request.AccountId, request.AccountLegalEntityId, request.ApprenticeshipIds));

            return new CreatedResult($"/accounts/{request.AccountId}/applications/{request.ApplicationId}", null);
        }

        [HttpPatch]
        [Route("applications")]
        public async Task<IActionResult> ConfirmApplication(ConfirmApplicationRequest request)
        {
            await _mediator.Send(new ConfirmApplicationCommand(request.ApplicationId, request.AccountId, request.DateSubmitted, request.SubmittedBy));

            return new OkResult();
        }
		
		[HttpGet]
        [Route("/accounts/{accountId}/applications/{applicationId}")]
        public async Task<IActionResult> GetApplication(long accountId, Guid applicationId)
        {
            var result = await _mediator.Send(new GetApplicationQuery
            {
                AccountId = accountId,
                ApplicationId = applicationId
            });

            var response = new ApplicationResponse { Application = result.Application };

            return Ok(response);
        }
    }
}