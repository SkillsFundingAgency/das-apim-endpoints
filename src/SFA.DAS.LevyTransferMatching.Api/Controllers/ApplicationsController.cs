using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationStatus;

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
            var viewModel = await _mediator.Send(new GetApplicationsQuery
            {
                AccountId = accountId
            });

            return Ok(viewModel);
        }

        [HttpGet]
        [Route("accounts/{accountId}/applications/{applicationId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ApplicationStatus(int applicationId)
        {
            try
            {
                var result = await _mediator.Send(new GetApplicationStatusQuery()
                {
                    ApplicationId = applicationId,
                });

                if (result != null)
                {
                    return Ok((GetApplicationStatusResponse)result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get {nameof(ApplicationStatus)} result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
