using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models.Cohorts;
using SFA.DAS.Approvals.Application.Cohorts.Commands;
using SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort;
using SFA.DAS.Approvals.Application.Cohorts.Queries;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipCourse;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipDetails;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetCohortDetails;
using SFA.DAS.Approvals.Exceptions;
using System;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipDeliveryModel;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetConfirmEmployer;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    public class CohortController : ControllerBase
    {
        private readonly ILogger<DraftApprenticeshipController> _logger;
        private readonly IMediator _mediator;

        public CohortController(ILogger<DraftApprenticeshipController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("[controller]/{cohortId}")]
        public async Task<IActionResult> Get(long cohortId)
        {
            try
            {
                var result = await _mediator.Send(new GetCohortQuery(cohortId));
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting cohort with {cohortId}", e);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("employer/{accountId}/unapproved/{cohortId}")]
        [Route("provider/{providerId}/unapproved/{cohortId}")]
        public async Task<IActionResult> GetCohortDetails(long cohortId)
        {
            try
            {
                var result = await _mediator.Send(new GetCohortDetailsQuery { CohortId = cohortId });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetCohortDetailsResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in get cohort details - cohort id {cohortId}");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("employer/{accountId}/unapproved/{cohortId}")]
        [Route("provider/{providerId}/unapproved/{cohortId}")]
        public async Task<IActionResult> Details(long cohortId, [FromBody] DetailsPostRequest request)
        {
            try
            {
                var command = new PostDetailsCommand
                {
                    CohortId = cohortId,
                    SubmissionType = request.SubmissionType,
                    Message = request.Message,
                    UserInfo = request.UserInfo
                };

                await _mediator.Send(command);

                return Ok();
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in Post Cohort Details - cohort id {cohortId}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("employer/{accountId}/unapproved/add/apprenticeship")]
        [Route("provider/{providerId}/unapproved/add/apprenticeship")]
        public async Task<IActionResult> GetAddDraftApprenticeshipDetails([FromQuery] long accountLegalEntityId, [FromQuery] long? providerId, [FromQuery] string courseCode)
        {
            try
            {
                var result = await _mediator.Send(new GetAddDraftApprenticeshipDetailsQuery
                    { ProviderId = providerId, AccountLegalEntityId = accountLegalEntityId, CourseCode = courseCode });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetAddDraftApprenticeshipDetailsResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in GetAddDraftApprenticeshipDetails ale {accountLegalEntityId}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("employer/{accountId}/unapproved/add/select-course")]
        [Route("provider/{providerId}/unapproved/add/select-course")]
        public async Task<IActionResult> GetAddDraftApprenticeshipCourse([FromQuery] long accountLegalEntityId, [FromQuery] long? providerId)
        {
            try
            {
                var result = await _mediator.Send(new GetAddDraftApprenticeshipCourseQuery
                    { ProviderId = providerId, AccountLegalEntityId = accountLegalEntityId });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetAddDraftApprenticeshipCourseResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in GetAddDraftApprenticeshipCourse ale {accountLegalEntityId}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("provider/{providerId}/unapproved/add/confirm-employer")]
        public async Task<IActionResult> GetConfirmEmployer()
        {
            try
            {
                var result = await _mediator.Send(new GetConfirmEmployerQuery());

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetConfirmEmployerResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in GetConfirmEmployer");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("employer/{accountId}/unapproved/add/select-delivery-model")]
        [Route("provider/{providerId}/unapproved/add/select-delivery-model")]
        public async Task<IActionResult> GetAddDraftApprenticeshipDeliveryModel([FromQuery] long accountLegalEntityId, [FromQuery] long? providerId, [FromQuery] string courseCode)
        {
            try
            {
                var result = await _mediator.Send(new GetAddDraftApprenticeshipDeliveryModelQuery
                    { ProviderId = providerId, AccountLegalEntityId = accountLegalEntityId, CourseCode = courseCode});

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetAddDraftApprenticeshipDeliveryModelResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in GetAddDraftApprenticeshipDeliveryModel ale {accountLegalEntityId}");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("cohorts")]
        public async Task<IActionResult> Create([FromBody] CreateCohortRequest request)
        {
            var command = new CreateCohortCommand
            {
                ActualStartDate = request.ActualStartDate,
                StartDate = request.StartDate,
                AccountId = request.AccountId,
                AccountLegalEntityId = request.AccountLegalEntityId,
                Cost = request.Cost,
                CourseCode = request.CourseCode,
                DateOfBirth = request.DateOfBirth,
                DeliveryModel = request.DeliveryModel,
                Email = request.Email,
                ReservationId = request.ReservationId,
                EmploymentEndDate = request.EmploymentEndDate,
                EmploymentPrice = request.EmploymentPrice,
                EndDate = request.EndDate,
                FirstName = request.FirstName,
                IgnoreStartDateOverlap = request.IgnoreStartDateOverlap,
                LastName = request.LastName,
                IsOnFlexiPaymentPilot = request.IsOnFlexiPaymentPilot,
                OriginatorReference = request.OriginatorReference,
                PledgeApplicationId = request.PledgeApplicationId,
                ProviderId = request.ProviderId,
                TransferSenderId = request.TransferSenderId,
                Uln = request.Uln,
                UserInfo = request.UserInfo,
                RequestingParty = request.RequestingParty
            };

            var result = await _mediator.Send(command);

            return Ok(new CreateCohortResponse
            {
                CohortId = result.CohortId,
                CohortReference = result.CohortReference
            });
        }
    }
}
