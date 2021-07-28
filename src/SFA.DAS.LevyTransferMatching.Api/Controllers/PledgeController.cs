using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetJobRoles;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetAmount;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetCreate;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetJobRole;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetLevel;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetSector;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    public class PledgeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PledgeController> _logger;

        public PledgeController(IMediator mediator, ILogger<PledgeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("pledges")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPledges()
        {
            var result = await _mediator.Send(new GetPledgesQuery());

            return Ok(result.Select(x => (PledgeDto)x));
        }

        [HttpGet]
        [Route("accounts/{accountId}/pledges/create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetCreateQuery());

                var response = new GetCreateResponse
                {
                    Levels = queryResult.Levels,
                    Sectors = queryResult.Sectors,
                    JobRoles = queryResult.JobRoles
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get Create result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("accounts/{accountId}/pledges/create")]
        public async Task<IActionResult> CreatePledge(long accountId, [FromBody]CreatePledgeRequest createPledgeRequest)
        {
            var commandResult = await _mediator.Send(new CreatePledgeCommand
            {
                AccountId = accountId,
                Amount = createPledgeRequest.Amount,
                IsNamePublic = createPledgeRequest.IsNamePublic,
                DasAccountName = createPledgeRequest.DasAccountName,
                JobRoles = createPledgeRequest.JobRoles,
                Levels = createPledgeRequest.Levels,
                Sectors = createPledgeRequest.Sectors,
                Locations = createPledgeRequest.Locations
            });

            return new CreatedResult(
                $"/accounts/{accountId}/pledges/{commandResult.PledgeId}",
                (PledgeIdDto)commandResult.PledgeId);
        }

        [HttpGet]
        [Route("accounts/{accountId}/pledges/create/amount")]
        public async Task<IActionResult> Amount(string accountId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetAmountQuery{EncodedAccountId = accountId });

                var response = new GetAmountResponse
                {
                    DasAccountName = queryResult.DasAccountName,
                    RemainingTransferAllowance = queryResult.RemainingTransferAllowance
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get Amount result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("accounts/{accountId}/pledges/create/sector")]
        public async Task<IActionResult> Sector()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetSectorQuery());

                var response = new GetSectorResponse
                {
                    Sectors = queryResult.Sectors
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get Sector result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("accounts/{accountId}/pledges/create/level")]
        public async Task<IActionResult> Level()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetLevelQuery());

                var response = new GetLevelResponse
                {
                    Levels = queryResult.Levels
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get Level result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("accounts/{accountId}/pledges/create/job-role")]
        public async Task<IActionResult> JobRole()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetJobRoleQuery());

                var response = new GetJobRoleResponse()
                {
                    JobRoles = queryResult.JobRoles
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get JobRoles result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}