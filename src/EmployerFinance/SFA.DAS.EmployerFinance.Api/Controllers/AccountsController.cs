using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Api.Models.Accounts;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountTeamMembersWhichReceiveNotifications;

namespace SFA.DAS.EmployerFinance.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<TransfersController> _logger;

        public AccountsController(IMapper mapper, IMediator mediator, ILogger<TransfersController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{accountId}/users/which-receive-notifications")]
        public async Task<IActionResult> GetAccountTeamMembersWhichReceiveNotifications(long accountId)
        {
            try
            {
                var response = await _mediator.Send(new GetAccountTeamMembersWhichReceiveNotificationsQuery()
                {
                    AccountId = accountId,
                });

                return Ok(_mapper.Map<GetAccountTeamMembersWhichReceiveNotificationsResponse>(response));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting account team members which receive notifications");
                return BadRequest();
            }
        }
    }
}
