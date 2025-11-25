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
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipPriorLearningSummary;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetViewDraftApprenticeship;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetRplRequirements;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.SyncLearnerData;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddEmail;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.Reference;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    public class DraftApprenticeshipController(
        ILogger<DraftApprenticeshipController> logger,
        IMediator mediator)
        : Controller
    {
        [HttpGet]
        [Route("[controller]/{cohortId}")]
        public async Task<IActionResult> GetAll(long cohortId)
        {
            try
            {
                var result = await mediator.Send(new GetDraftApprenticeshipsQuery(cohortId));
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting cohort {id}", cohortId);
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
                var result = await mediator.Send(new GetEditDraftApprenticeshipQuery{ CohortId = cohortId, DraftApprenticeshipId = draftApprenticeshipId, CourseCode = courseCode});
                
                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetEditDraftApprenticeshipResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error in GetEditDraftApprenticeship cohort {cohortId} draft apprenticeship {draftApprenticeshipId}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("employer/{accountId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/view")]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/view")]
        public async Task<IActionResult> GetViewDraftApprenticeship(long cohortId, long draftApprenticeshipId)
        {
            try
            {
                var result = await mediator.Send(new GetViewDraftApprenticeshipQuery { CohortId = cohortId, DraftApprenticeshipId = draftApprenticeshipId });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetViewDraftApprenticeshipResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error in GetViewDraftApprenticeship cohort {cohortId} draft apprenticeship {draftApprenticeshipId}");
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
                var result = await mediator.Send(new GetEditDraftApprenticeshipDeliveryModelQuery { CohortId = cohortId, DraftApprenticeshipId = draftApprenticeshipId, CourseCode = courseCode });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetEditDraftApprenticeshipDeliveryModelResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error in GetEditDraftApprenticeshipDeliveryModel cohort {cohortId} draft apprenticeship {draftApprenticeshipId}");
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
                var result = await mediator.Send(new GetEditDraftApprenticeshipCourseQuery { CohortId = cohortId, DraftApprenticeshipId = draftApprenticeshipId});

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetEditDraftApprenticeshipCourseResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error in GetEditDraftApprenticeshipCourse cohort {cohortId} draft apprenticeship {draftApprenticeshipId}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("employer/{accountId}/unapproved/{cohortId}/apprentices/add/details")]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/add/details")]
        public async Task<IActionResult> GetAddDraftApprenticeshipDetails(long cohortId, [FromQuery] string courseCode, [FromQuery] DateTime? startDate)
        {
            try
            {
                var result = await mediator.Send(new GetAddDraftApprenticeshipDetailsQuery { CohortId = cohortId, CourseCode = courseCode, StartDate = startDate });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetAddDraftApprenticeshipDetailsResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error in GetAddDraftApprenticeshipDetails cohort {cohortId}");
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
                var result = await mediator.Send(new GetAddDraftApprenticeshipCourseQuery { CohortId = cohortId, DraftApprenticeshipId = draftApprenticeshipId });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok((GetAddDraftApprenticeshipCourseResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error in GetEditDraftApprenticeshipCourse cohort {cohortId} draft apprenticeship {draftApprenticeshipId}");
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
                TrainingPrice = request.TrainingPrice,
                EndPointAssessmentPrice = request.EndPointAssessmentPrice,
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
                RequestingParty = request.RequestingParty,
                LearnerDataId = request.LearnerDataId
            };

            var result = await mediator.Send(command);

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
                TrainingPrice = request.TrainingPrice,
                EndPointAssessmentPrice = request.EndPointAssessmentPrice,
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
                RequestingParty = request.RequestingParty,
                HasLearnerDataChanges = request.HasLearnerDataChanges,
                LastLearnerDataSync = request.LastLearnerDataSync
            };

            await mediator.Send(command);

            return Ok();
        }

        [HttpGet]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit/prior-learning-data")]
        public async Task<IActionResult> GetPriorLearningData(long cohortId, long draftApprenticeshipId)
        {
            var result = await mediator.Send(new GetEditDraftApprenticeshipPriorLearningDataQuery(cohortId,draftApprenticeshipId));

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit/prior-learning-data")]
        public async Task<IActionResult> AddPriorLearningData(long cohortId, long draftApprenticeshipId, [FromBody] AddPriorLearningDataRequest request)
        {
            var command = new AddPriorLearningDataCommand
            {
                CohortId = cohortId,
                DraftApprenticeshipId = draftApprenticeshipId,
                DurationReducedBy = request.DurationReducedBy,
                DurationReducedByHours = request.DurationReducedByHours,
                IsDurationReducedByRpl = request.IsDurationReducedByRpl,
                PriceReducedBy = request.PriceReducedBy,
                TrainingTotalHours = request.TrainingTotalHours
            };

            var response = await mediator.Send(command);

            return Ok(response);
        }

        [HttpGet]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit/prior-learning-summary")]
        public async Task<IActionResult> GetPriorLearningSummary(long cohortId, long draftApprenticeshipId)
        {
            var result = await mediator.Send(new GetEditDraftApprenticeshipPriorLearningSummaryQuery(cohortId, draftApprenticeshipId));

            return Ok(result);
        }

        [HttpGet]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/recognise-prior-learning")]
        public async Task<IActionResult> GetRplRequirements(long cohortId, long draftApprenticeshipId, [FromQuery] string courseId)
        {
            try
            {
                var result = await mediator.Send(new GetRplRequirementsQuery { CourseId = courseId });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in GetRplRequirements cohort {CohortId} draft apprenticeship {DraftApprenticeshipId}", cohortId, draftApprenticeshipId);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/sync-learner-data")]
        public async Task<IActionResult> SyncLearnerData(long providerId, long cohortId, long draftApprenticeshipId)
        {
        try
        {
            var updatedDraftApprenticeship = await mediator.Send(new SyncLearnerDataCommand
            {
                ProviderId = providerId,
                CohortId = cohortId,
                DraftApprenticeshipId = draftApprenticeshipId
            });

            return Ok(new SyncLearnerDataResponse
            {
                Success = true,
                Message = "Learner data has been successfully merged",
                UpdatedDraftApprenticeship = updatedDraftApprenticeship
            });
        }
        catch (LearnerDataSyncException ex)
        {
            logger.LogWarning(ex, "Learner data sync failed for provider {ProviderId} cohort {CohortId} draft apprenticeship {DraftApprenticeshipId}: {Message}", 
                providerId, cohortId, draftApprenticeshipId, ex.Message);
            return Ok(new SyncLearnerDataResponse
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error in SyncLearnerData provider {ProviderId} cohort {CohortId} draft apprenticeship {DraftApprenticeshipId}", providerId, cohortId, draftApprenticeshipId);
            return BadRequest(new SyncLearnerDataResponse
            {
                Success = false,
                Message = "An error occurred while syncing learner data."
            });
        }
        }

        [HttpPost]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/add/email")]
        public async Task<IActionResult> AddEmail(long cohortId, long draftApprenticeshipId, [FromBody] AddDraftApprenticeEmailRequest request)
        {
            try
            {
                var command = new DraftApprenticeshipAddEmailCommand
                {
                    CohortId = cohortId,
                    DraftApprenticeshipId = draftApprenticeshipId,
                    Email = request.Email
                };

                var response = await mediator.Send(command);

                return Ok(response);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in add email for cohort {CohortId} draft apprenticeship {DraftApprenticeshipId}", cohortId, draftApprenticeshipId);
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/setReference")]
        public async Task<IActionResult> SetReference(long cohortId, long draftApprenticeshipId, [FromBody] DraftApprenticeshipSetReferenceRequest request)
        {
            try
            {
                var command = new DraftApprenticeshipSetReferenceCommand
                {
                    CohortId = cohortId,
                    DraftApprenticeshipId = draftApprenticeshipId,
                    Reference = request.Reference,
                    Party = request.Party,
                };

                var response = await mediator.Send(command);

                return Ok(response);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in set reference for cohort {CohortId} draft apprenticeship {DraftApprenticeshipId}", cohortId, draftApprenticeshipId);
                return BadRequest();
            }
        }
    }
}
