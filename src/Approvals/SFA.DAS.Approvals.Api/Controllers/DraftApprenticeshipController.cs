﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries;
using System;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Api.Models.DraftApprenticeships;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetAddDraftApprenticeshipDetails;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeship;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipDeliveryModel;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    public class DraftApprenticeshipController : Controller
    {
        private readonly ILogger<DraftApprenticeshipController> _logger;
        private readonly IMediator _mediator;

        public DraftApprenticeshipController(ILogger<DraftApprenticeshipController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("[controller]/{cohortId}")]
        public async Task<IActionResult> GetAll(long cohortId)
        {
            try
            { 
                var result = await _mediator.Send(new GetDraftApprenticeshipsQuery(cohortId));
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting cohort {id}", cohortId);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("employer/{accountId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit")]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit")]
        public async Task<IActionResult> GetEditDraftApprenticeship(long cohortId, long draftApprenticeshipId, [FromQuery] string courseCode)
        {
            try
            {
                var result = await _mediator.Send(new GetEditDraftApprenticeshipQuery{ CohortId = cohortId, DraftApprenticeshipId = draftApprenticeshipId, CourseCode = courseCode});
                
                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetEditDraftApprenticeshipResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in GetEditDraftApprenticeship cohort {cohortId} draft apprenticeship {draftApprenticeshipId}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("employer/{accountId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit/select-delivery-model")]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit/select-delivery-model")]
        public async Task<IActionResult> GetEditDraftApprenticeshipDeliveryModel(long cohortId, long draftApprenticeshipId, [FromQuery] string courseCode)
        {
            try
            {
                var result = await _mediator.Send(new GetEditDraftApprenticeshipDeliveryModelQuery { CohortId = cohortId, DraftApprenticeshipId = draftApprenticeshipId, CourseCode = courseCode });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetEditDraftApprenticeshipDeliveryModelResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in GetEditDraftApprenticeship cohort {cohortId} draft apprenticeship {draftApprenticeshipId}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("employer/{accountId}/unapproved/{cohortId}/apprentices/add/details")]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/add/details")]
        public async Task<IActionResult> GetAddDraftApprenticeshipDetails(long cohortId, [FromQuery] string courseCode)
        {
            try
            {
                var result = await _mediator.Send(new GetAddDraftApprenticeshipDetailsQuery { CohortId = cohortId, CourseCode = courseCode });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetAddDraftApprenticeshipDetailsResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in GetAddDraftApprenticeshipDetails cohort {cohortId}");
                return BadRequest();
            }
        }
    }
}
