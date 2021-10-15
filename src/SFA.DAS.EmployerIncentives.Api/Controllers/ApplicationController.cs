using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication;
using SFA.DAS.EmployerIncentives.Application.Commands.CreateApplication;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateApplication;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApplication;
using SFA.DAS.EmployerIncentives.Application.Queries.GetBankingData;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApplicationAccountLegalEntity;
using System;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Application.Commands.SaveApprenticeshipDetails;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.ApprenticeshipDetails;
using SFA.DAS.EmployerIncentives.Exceptions;

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
        public async Task<IActionResult> CreateApplication(CreateApplicationRequest request)
        {
            await _mediator.Send(new CreateApplicationCommand(request.ApplicationId, request.AccountId, request.AccountLegalEntityId, request.ApprenticeshipIds));

            return new CreatedResult($"/accounts/{request.AccountId}/applications/{request.ApplicationId}", null);
        }

        [HttpPut]
        [Route("/accounts/{accountId}/applications")]
        public async Task<IActionResult> UpdateApplication(UpdateApplicationRequest request)
        {
            await _mediator.Send(new UpdateApplicationCommand(request.ApplicationId, request.AccountId, request.ApprenticeshipIds));

            return new OkObjectResult($"/accounts/{request.AccountId}/applications/{request.ApplicationId}");
        }

        [HttpPatch]
        [Route("/accounts/{accountId}/applications")]
        public async Task<IActionResult> ConfirmApplication(ConfirmApplicationRequest request)
        {
            try
            {
                await _mediator.Send(new ConfirmApplicationCommand(request.ApplicationId, request.AccountId,  request.DateSubmitted, request.SubmittedByEmail, request.SubmittedByName));
                return new OkResult();
            }
            catch (UlnAlreadySubmittedException)
            {
                return Conflict("Application contains a ULN which has already been submitted");
            }
        }

        [HttpGet]
        [Route("/accounts/{accountId}/applications/{applicationId}")]
        public async Task<IActionResult> GetApplication(long accountId, Guid applicationId, [FromQuery] bool includeApprenticeships = true)
        {
            var result = await _mediator.Send(new GetApplicationQuery
            {
                AccountId = accountId,
                ApplicationId = applicationId,
                IncludeApprenticeships = includeApprenticeships
            });

            var response = new ApplicationResponse { Application = result.Application };

            return Ok(response);
        }

        [HttpGet]
        [Route("/accounts/{accountId}/applications/{applicationId}/bankingDetails/")]
        public async Task<IActionResult> GetBankingDetails(long accountId, Guid applicationId, string hashedAccountId)
        {
            var result = await _mediator.Send(new GetBankingDataQuery
            {
                AccountId = accountId,
                ApplicationId = applicationId,
                HashedAccountId = hashedAccountId
            });

            var response = (BankingDetailsDto)result.Data;

            return Ok(response);
        }

        [HttpGet]
        [Route("/accounts/{accountId}/applications/{applicationId}/accountlegalentity")]
        public async Task<IActionResult> GetApplicationAccountLegalEntity(long accountId, Guid applicationId)
        {
            var result = await _mediator.Send(new GetApplicationAccountLegalEntityQuery
            {
                AccountId = accountId,
                ApplicationId = applicationId
            });

            return Ok(result);
        }

        [HttpPatch]
        [Route("/accounts/{accountId}/applications/{applicationId}/apprenticeships")]
        public async Task<IActionResult> SaveApprenticeshipDetailsDetails(long accountId, Guid applicationId, [FromBody] ApprenticeshipDetailsRequest request)
        {
            await _mediator.Send(new SaveApprenticeshipDetailsCommand(request));

            return new OkResult();
        }
    }
}