using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries;
using System;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Api.Models.DraftApprenticeships;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddDraftApprenticeship;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.UpdateDraftApprenticeship;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetAddDraftApprenticeshipCourse;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetAddDraftApprenticeshipDetails;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeship;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipCourse;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipDeliveryModel;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddPriorLearningData;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipPriorLearningData;

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
                _logger.LogError(e, $"Error in GetEditDraftApprenticeshipDeliveryModel cohort {cohortId} draft apprenticeship {draftApprenticeshipId}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("employer/{accountId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit/select-course")]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit/select-course")]
        public async Task<IActionResult> GetEditDraftApprenticeshipCourse(long cohortId, long draftApprenticeshipId)
        {
            try
            {
                var result = await _mediator.Send(new GetEditDraftApprenticeshipCourseQuery { CohortId = cohortId, DraftApprenticeshipId = draftApprenticeshipId});

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetEditDraftApprenticeshipCourseResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in GetEditDraftApprenticeshipCourse cohort {cohortId} draft apprenticeship {draftApprenticeshipId}");
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

        [HttpGet]
        [Route("employer/{accountId}/unapproved/{cohortId}/apprentices/add/select-course")]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/add/select-course")]
        public async Task<IActionResult> GetAddDraftApprenticeshipCourse(long cohortId, long draftApprenticeshipId)
        {
            try
            {
                var result = await _mediator.Send(new GetAddDraftApprenticeshipCourseQuery { CohortId = cohortId, DraftApprenticeshipId = draftApprenticeshipId });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetAddDraftApprenticeshipCourseResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in GetEditDraftApprenticeshipCourse cohort {cohortId} draft apprenticeship {draftApprenticeshipId}");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("cohorts/{cohortId}/draft-apprenticeships")]
        public async Task<IActionResult> AddDraftApprenticeship(long cohortId, [FromBody] AddDraftApprenticeshipRequest request)
        {
            var command = new AddDraftApprenticeshipCommand
            {
                ActualStartDate = request.ActualStartDate,
                StartDate = request.StartDate,
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
                ProviderId = request.ProviderId,
                Uln = request.Uln,
                UserInfo = request.UserInfo,
                UserId = request.UserId,
                CohortId = cohortId,
                RequestingParty = request.RequestingParty
            };

            var result = await _mediator.Send(command);

            return Ok(new AddDraftApprenticeshipResponse
            {
                DraftApprenticeshipId = result.DraftApprenticeshipId
            });
        }

        [HttpPut]
        [Route("cohorts/{cohortId}/draft-apprenticeships/{apprenticeshipId}")]
        public async Task<IActionResult> UpdateDraftApprenticeship(long cohortId, long apprenticeshipId, [FromBody] UpdateDraftApprenticeshipRequest request)
        {
            var command = new UpdateDraftApprenticeshipCommand
            {
                ActualStartDate = request.ActualStartDate,
                StartDate = request.StartDate,
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
                Uln = request.Uln,
                UserInfo = request.UserInfo,
                CourseOption = request.CourseOption,
                Reference = request.Reference,
                CohortId = cohortId,
                ApprenticeshipId = apprenticeshipId,
                RequestingParty = request.RequestingParty
            };

            await _mediator.Send(command);

            return Ok();
        }

        [HttpGet]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit/prior-learning-data")]
        public async Task<IActionResult> GetPriorLearningData(long cohortId, long draftApprenticeshipId)
        {
            var result = await _mediator.Send(new GetEditDraftApprenticeshipPriorLearningDataQuery(cohortId,draftApprenticeshipId));

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit/prior-learning-data")]
        public async Task<IActionResult> PriorLearningData(long cohortId, long draftApprenticeshipId, [FromBody] InnerApi.Requests.AddPriorLearningDataRequest request)
        {
            var command = new AddPriorLearningDataCommand
            {
                CohortId = cohortId,
                DraftApprenticeshipId = draftApprenticeshipId,
                CostBeforeRpl = request.CostBeforeRpl,
                DurationReducedBy = request.DurationReducedBy,
                DurationReducedByHours = request.DurationReducedByHours,
                IsDurationReducedByRpl = request.IsDurationReducedByRpl,
                PriceReducedBy = request.PriceReducedBy,
                TrainingTotalHours = request.TrainingTotalHours
            };

            var response = await _mediator.Send(command);

            return Ok(response);
        }
    }
}
