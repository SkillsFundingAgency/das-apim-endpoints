using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.Application.Transfers.Queries.GetTransferValidity;

namespace SFA.DAS.Reservations.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TransfersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TransfersController> _logger;

        public TransfersController(IMediator mediator, ILogger<TransfersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("validity")]
        public async Task<IActionResult> GetTransferValidity(long senderId, long receiverId, int? pledgeApplicationId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetTransferValidityQuery { SenderId = senderId, ReceiverId = receiverId, PledgeApplicationId = pledgeApplicationId});

                var model = (GetTransferValidityResponse)queryResult;

                if (model == null)
                {
                    return NotFound();
                }

                return Ok(model);
            }
            catch (Exception e)
            {
                var applicationId = pledgeApplicationId.HasValue ? pledgeApplicationId.Value.ToString() : "null";
                _logger.LogError(e, $"Error attempting to get transfer validity for sender {senderId}, receiver {receiverId}, pledge application {applicationId}");
                return BadRequest();
            }
        }
    }
}
