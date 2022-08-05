using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Api.Models.Accounts;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountMinimumSignedAgreementVersion;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountTeamMembersWhichReceiveNotifications;

namespace SFA.DAS.EmployerFinance.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TransfersController> _logger;

        public AccountsController(IMediator mediator, ILogger<TransfersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{accountId}/users/which-receive-notifications")]
        public async Task<IActionResult> GetAccountTeamMembersWhichReceiveNotifications(long accountId)
        {
            try
            {
                var result = await _mediator.Send(new GetAccountTeamMembersWhichReceiveNotificationsQuery()
                {
                    AccountId = accountId,
                });

                return Ok((GetAccountTeamMembersWhichReceiveNotificationsResponse)(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting account team members which receive notifications");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{_accountId}/minimum-signed-agreement-version")]
        public async Task<IActionResult> GetAccountMinimumSignedAgreementVersion(long accountId)
        {
            try
            {
                var result = await _mediator.Send(new GetAccountMinimumSignedAgreementVersionQuery()
                {
                    AccountId = accountId,
                });

                return Ok(new GetAccountMinimumSignedAgreementVersionResponse { MinimumSignedAgreementVersion = result.MinimumSignedAgreementVersion });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting account minimum signed agreement version");
                return BadRequest();
            }
        }
    }
}
