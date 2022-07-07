using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models.Apprentices.ChangeEmployer;
using SFA.DAS.Approvals.Application.Apprentices.Commands.ChangeEmployer.Confirm;
using SFA.DAS.Approvals.Application.Apprentices.Queries;
using SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.Inform;
using SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.SelectDeliveryModel;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ApprenticesController: ControllerBase
    {
        private readonly ILogger<ApprenticesController> _logger;
        private readonly IMediator _mediator;

        public ApprenticesController(ILogger<ApprenticesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetApprenticeQuery{ ApprenticeId = id});
                if (result == null)
                {
                    return NotFound();
                }                
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Apprentice data {id}", id);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/change-employer/inform")]
        public async Task<IActionResult> ChangeEmployerInform(long providerId, long apprenticeshipId)
        {
            try
            {
                var result = await _mediator.Send(new GetInformQuery
                { ApprenticeshipId = apprenticeshipId, ProviderId = providerId });

                if (result == null)
                {
                    return NotFound();
                }

                var response = new GetInformResponse
                {
                    LegalEntityName = result.LegalEntityName,
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting change employer - inform, apprenticeship {id}", apprenticeshipId);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/change-employer/confirm")]
        public async Task<IActionResult> ChangeEmployerConfirm(long providerId, long apprenticeshipId, [FromBody] ConfirmRequest request)
        {
            try
            {
                await _mediator.Send(new ConfirmCommand
                {
                    ApprenticeshipId = apprenticeshipId,
                    ProviderId = providerId,
                    AccountLegalEntityId = request.AccountLegalEntityId,
                    Price = request.Price,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    EmploymentEndDate = request.EmploymentEndDate,
                    EmploymentPrice = request.EmploymentPrice,
                    DeliveryModel = request.DeliveryModel,
                    UserInfo = request.UserInfo
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting change employer - inform, apprenticeship {id}", apprenticeshipId);
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/change-employer/select-delivery-model")]
        public async Task<IActionResult> ChangeEmployerSelectDeliveryModel(long providerId, long apprenticeshipId)
        {
            try
            {
                var result = await _mediator.Send(new GetSelectDeliveryModelQuery
                    { ApprenticeshipId = apprenticeshipId, ProviderId = providerId });

                if (result == null)
                {
                    return NotFound();
                }

                var response = new GetSelectDeliveryModelResponse
                {
                    LegalEntityName = result.LegalEntityName,
                    DeliveryModels = result.DeliveryModels
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting change employer - select-delivery-model, apprenticeship {id}", apprenticeshipId);
                return BadRequest();
            }
        }
    }
}