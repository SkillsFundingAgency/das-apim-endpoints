using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetContactDetails;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetIndex;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetDetail;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSector;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunities;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunities.GetConfirmation;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetApply;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetMoreDetails;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetApplicationDetails;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSelectAccount;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    public class OpportunityController : ControllerBase
    {
        private readonly ILogger<OpportunityController> _logger;
        private readonly IMediator _mediator;

        public OpportunityController(ILogger<OpportunityController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("opportunities")]
        public async Task<IActionResult> GetIndex([FromQuery] IEnumerable<string> sectors = null)
        {
            try
            {
                var result = await _mediator.Send(new GetIndexQuery() { Sectors = sectors });

                var response = new GetIndexResponse
                {
                    Opportunities = result.Opportunities.Select(x => new GetIndexResponse.Opportunity
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        IsNamePublic = x.IsNamePublic,
                        DasAccountName = x.DasAccountName,
                        Sectors = x.Sectors,
                        JobRoles = x.JobRoles,
                        Levels = x.Levels,
                        Locations = x.Locations
                    }),
                    Sectors = result.Sectors,
                    JobRoles = result.JobRoles,
                    Levels = result.Levels,
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get Index result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("accounts/{accountId}/opportunities/{opportunityId}/apply")]
        public async Task<IActionResult> Apply(long accountId, int opportunityId)
        {
            try
            {
                var applyQueryResult = await _mediator.Send(new GetApplyQuery { OpportunityId = opportunityId });

                if (applyQueryResult == null)
                {
                    return NotFound();
                }

                var response = new GetApplyResponse
                {
                    Opportunity = applyQueryResult.Opportunity,
                    Sectors = applyQueryResult.Sectors,
                    JobRoles = applyQueryResult.JobRoles,
                    Levels = applyQueryResult.Levels,
                    PledgeLocations = applyQueryResult.Opportunity.Locations.Select(x => new GetApplyResponse.PledgeLocation { Id = x.Id, Name = x.Name })
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get Apply result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("/accounts/{accountId}/opportunities/{opportunityId}/apply")]
        public async Task<IActionResult> Apply(long accountId, int opportunityId, [FromBody] ApplyRequest request)
        {
            var result = await _mediator.Send(new CreateApplicationCommand
            {
                EmployerAccountId = accountId,
                PledgeId = opportunityId,
                EncodedAccountId = request.EncodedAccountId,
                Details = request.Details,
                StandardId = request.StandardId,
                NumberOfApprentices = request.NumberOfApprentices,
                StartDate = request.StartDate,
                HasTrainingProvider = request.HasTrainingProvider,
                Sectors = request.Sectors,
                Locations = request.Locations,
                AdditionalLocation = request.AdditionalLocation,
                SpecificLocation = request.SpecificLocation,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddresses = request.EmailAddresses,
                BusinessWebsite = request.BusinessWebsite,
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName
            });

            return Created($"/accounts/{accountId}/opportunities/{opportunityId}/apply", (ApplyResponse)result);
        }

        [HttpGet]
        [Route("/accounts/{accountId}/opportunities/{opportunityId}/apply/confirmation")]
        public async Task<IActionResult> Confirmation(long accountId, int opportunityId)
        {
            var result = await _mediator.Send(new GetConfirmationQuery
            {
                OpportunityId = opportunityId
            });

            if (result != null)
            {
                return Ok((GetConfirmationResponse)result);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/accounts/{accountId}/opportunities/{opportunityId}/apply/application-details")]
        [Route("/accounts/{accountId}/opportunities/{opportunityId}/create/application-details")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApplicationDetails(long accountId, int opportunityId, [FromQuery] string standardId)
        {
            try
            {
                var result = await _mediator.Send(new GetApplicationDetailsQuery { OpportunityId = opportunityId, StandardId = standardId });

                if(result.Opportunity == null)
                {
                    return NotFound();
                }

                return Ok(new ApplicationDetailsResponse
                {
                    Opportunity = result.Opportunity,
                    Standards = result.Standards,
                    Sectors = result.Sectors,
                    JobRoles = result.JobRoles,
                    Levels = result.Levels
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting Application Details");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("accounts/{accountId}/opportunities/{opportunityId}/apply/more-details")]
        [Route("accounts/{accountId}/opportunities/{opportunityId}/create/more-details")]
        public async Task<IActionResult> MoreDetails(long accountId, int opportunityId)
        {
            try
            {
                var moreDetailsQueryResult = await _mediator.Send(new GetMoreDetailsQuery { OpportunityId = opportunityId });

                var response = new GetMoreDetailsResponse
                {
                    Opportunity = moreDetailsQueryResult.Opportunity,
                    Sectors = moreDetailsQueryResult.Sectors,
                    JobRoles = moreDetailsQueryResult.JobRoles,
                    Levels = moreDetailsQueryResult.Levels
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get MoreDetails result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("accounts/{accountId}/opportunities/{opportunityId}/create/sector")]
        [Route("accounts/{accountId}/opportunities/{opportunityId}/apply/sector")]
        public async Task<IActionResult> Sector(int opportunityId)
        {
            try
            {
                var sectorQueryResult = await _mediator.Send(new GetSectorQuery { OpportunityId = opportunityId });

                var response = new GetSectorResponse
                {
                    Opportunity = sectorQueryResult.Opportunity,
                    Sectors = sectorQueryResult.Sectors,
                    JobRoles = sectorQueryResult.JobRoles,
                    Levels = sectorQueryResult.Levels,
                    PledgeLocations = sectorQueryResult.Opportunity.Locations.Select(x => new GetSectorResponse.PledgeLocation { Id = x.Id, Name = x.Name})
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
        [Route("accounts/{accountId}/opportunities/{opportunityId}/apply/contact-details")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ContactDetails(int opportunityId)
        {
            try
            {
                var result = await _mediator.Send(new GetContactDetailsQuery()
                {
                    OpportunityId = opportunityId,
                });

                if (result != null)
                {
                    return Ok((GetContactDetailsResponse)result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get {nameof(GetContactDetailsQuery)} result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("opportunities/{opportunityId}")]
        public async Task<IActionResult> Detail(int opportunityId)
        {
            try
            {
                var detailQueryResult = await _mediator.Send(new GetDetailQuery { OpportunityId = opportunityId });

                if(detailQueryResult == null)
                {
                    return NotFound();
                }

                var response = new GetDetailResponse
                {
                    Opportunity = detailQueryResult.Opportunity,
                    Sectors = detailQueryResult.Sectors,
                    JobRoles = detailQueryResult.JobRoles,
                    Levels = detailQueryResult.Levels,
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get Detail result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("opportunities/{opportunityId}/select-account")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SelectAccount(int opportunityId, string userId)
        {
            try
            {
                var getSelectAccountQueryResult = await _mediator.Send(new GetSelectAccountQuery
                {
                    UserId = userId,
                });

                return Ok((GetSelectAccountResponse)getSelectAccountQueryResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get SelectAccount result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}