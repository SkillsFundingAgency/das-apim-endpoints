using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.LevyTransferMatching.Api.Authentication;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetAmount;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetCreate;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetJobRole;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetLevel;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetSector;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationApproved;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationApprovalOptions;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationApprovalOptions;


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
        [Route("accounts/{accountId}/pledges")]
        public async Task<IActionResult> Pledges(long accountId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetPledgesQuery(accountId));

                var response = new GetPledgesResponse
                {
                    Pledges = queryResult.Pledges.Select(x => new GetPledgesResponse.Pledge 
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        RemainingAmount = x.RemainingAmount,
                        ApplicationCount = x.ApplicationCount
                    })
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get Pledges result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
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
                Locations = createPledgeRequest.Locations,
                UserId = createPledgeRequest.UserId,
                UserDisplayName = createPledgeRequest.UserDisplayName
            });

            return new CreatedResult(
                $"/accounts/{accountId}/pledges/{commandResult.PledgeId}",
                (PledgeIdDto)commandResult.PledgeId);
        }

        [HttpGet]
        [Route("accounts/{accountId}/pledges/create/amount")]
        public async Task<IActionResult> Amount(long accountId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetAmountQuery{ AccountId = accountId });

                if (queryResult == null)
                {
                    return NotFound();
                }

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
                    JobRoles = queryResult.JobRoles,
                    Sectors = queryResult.Sectors
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get JobRoles result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize(Policy = PolicyNames.PledgeAccess)]
        [HttpGet]
        [Route("accounts/{accountId}/pledges/{pledgeId}/applications")]
        public async Task<IActionResult> PledgeApplications(int pledgeId)
        {
            var queryResult = await _mediator.Send(new GetApplicationsQuery { PledgeId = pledgeId });

            return Ok(new GetApplicationsResponse
            {
                Applications = queryResult?.Applications.Select(x => (GetApplicationsResponse.Application)x)
            });
        }
        
        [Authorize(Policy = PolicyNames.PledgeAccess)]
        [HttpGet]
        [Route("accounts/{accountId}/pledges/{pledgeId}/applications/{applicationId}")]
        public async Task<IActionResult> Application(int pledgeId, int applicationId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetApplicationQuery()
                {
                    PledgeId = pledgeId,
                    ApplicationId = applicationId,
                });

                if (queryResult != null)
                {
                    var response = (GetApplicationResponse)queryResult;

                    return Ok(response);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get application");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize(Policy = PolicyNames.PledgeAccess)]
        [HttpPost]
        [Route("accounts/{accountId}/pledges/{pledgeId}/applications/{applicationId}")]
        public async Task<IActionResult> Application(long accountId, int pledgeId, int applicationId, [FromBody] SetApplicationOutcomeRequest outcomeRequest)
        {
            try
            {
                await _mediator.Send(new SetApplicationOutcomeCommand
                {
                    ApplicationId = applicationId,
                    PledgeId = pledgeId, 
                    UserId = outcomeRequest.UserId,
                    UserDisplayName = outcomeRequest.UserDisplayName,
                    Outcome = outcomeRequest.Outcome
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to approve/reject application");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize(Policy = PolicyNames.PledgeAccess)]
        [HttpGet]
        [Route("accounts/{accountId}/pledges/{pledgeId}/applications/{applicationId}/approval-options")]
        public async Task<IActionResult> ApplicationApprovalOptions(int pledgeId, int applicationId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetApplicationApprovalOptionsQuery()
                {
                    PledgeId = pledgeId,
                    ApplicationId = applicationId,
                });

                if (queryResult == null)
                {
                    return NotFound();
                }

                var response = (GetApplicationApprovalOptionsResponse)queryResult;

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get application approval options");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize(Policy = PolicyNames.PledgeAccess)]
        [HttpPost]
        [Route("accounts/{accountId}/pledges/{pledgeId}/applications/{applicationId}/approval-options")]
        public async Task<IActionResult> SetApplicationApprovalOptions(int pledgeId, int applicationId, [FromBody] SetApplicationApprovalOptionsRequest request)
        {
            await _mediator.Send(new SetApplicationApprovalOptionsCommand
            {
                PledgeId = pledgeId,
                ApplicationId = applicationId,
                UserDisplayName = request.UserDisplayName,
                UserId = request.UserId,
                AutomaticApproval = request.AutomaticApproval
            });

            return Ok();
        }

        [Authorize(Policy = PolicyNames.PledgeAccess)]
        [HttpGet]
        [Route("accounts/{accountId}/pledges/{pledgeId}/applications/{applicationId}/approved")]
        public async Task<IActionResult> ApplicationApproved(long accountId, int pledgeId, int applicationId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetApplicationApprovedQuery { PledgeId = pledgeId, ApplicationId = applicationId });

                var response = new GetApplicationApprovedResponse
                {
                    EmployerAccountName = queryResult.EmployerAccountName,
                    AutomaticApproval = queryResult.AutomaticApproval
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get ApplicationApproved result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}