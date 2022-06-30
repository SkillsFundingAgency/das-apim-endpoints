using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models.Apprentices.ChangeEmployer;
using SFA.DAS.Approvals.Application.Apprentices.Queries;
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
        [Route("/provider/{providerId}/apprentices/{apprenticeshipId}/change-employer/select-delivery-model")]
        public async Task<IActionResult> ChangeEmployerSelectDeliveryModel(long providerId, long apprenticeshipId)
        {
            var result = await _mediator.Send(new GetSelectDeliveryModelQuery
                { ApprenticeshipId = apprenticeshipId, ProviderId = providerId });

            var response =  new GetSelectDeliveryModelResponse
            {
                LegalEntityName = result.LegalEntityName,
                DeliveryModels = result.DeliveryModels
            };

            return Ok(response);
        }

    }
}