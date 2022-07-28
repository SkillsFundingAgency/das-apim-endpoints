using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries;
using System;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Api.Models.DraftApprenticeships;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeship;
using SFA.DAS.Approvals.InnerApi.Responses;

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
        [Route("{party}/{partyId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit")]
        public async Task<IActionResult> GetEditDraftApprenticeship(Party party, long partyId, long cohortId, long draftApprenticeshipId)
        {
            try
            {
                var result = await _mediator.Send(new GetEditDraftApprenticeshipQuery{ Party = party, PartyId = partyId, CohortId = cohortId, DraftApprenticeshipId = draftApprenticeshipId});
                
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(new GetEditDraftApprenticeshipResponse
                {
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    DateOfBirth = result.DateOfBirth,
                    ReservationId = result.ReservationId,
                    Email  = result.Email,
                    Uln   = result.Uln,
                    DeliveryModel =  result.DeliveryModel,
                    CourseCode  = result.CourseCode,
                    StandardUId = result.StandardUId,
                    CourseName = result.CourseName,
                    StartDate = result.StartDate,
                    EndDate = result.EndDate,
                    Cost = result.Cost,
                    EmploymentPrice = result.EmploymentPrice,
                    EmploymentEndDate = result.EmploymentEndDate,
                    EmployerReference = result.EmployerReference,
                    ProviderReference = result.ProviderReference,
                    ProviderId = result.ProviderId,
                    AccountLegalEntityId = result.AccountLegalEntityId,
                    ProviderName = result.ProviderName,
                    LegalEntityName = result.LegalEntityName,
                    IsContinuation = result.IsContinuation,
                    HasMultipleDeliveryModelOptions = result.HasMultipleDeliveryModelOptions,
                    RecognisePriorLearning = result.RecognisePriorLearning,
                    DurationReducedBy = result.DurationReducedBy,
                    PriceReducedBy = result.PriceReducedBy,
                    RecognisingPriorLearningStillNeedsToBeConsidered = result.RecognisingPriorLearningStillNeedsToBeConsidered
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in GetEditDraftApprenticeship cohort {cohortId} draft apprenticeship {draftApprenticeshipId}");
                return BadRequest();
            }
        }
    }
}
