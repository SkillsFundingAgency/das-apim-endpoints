using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ManageApprenticeships.Api.Models;
using SFA.DAS.ManageApprenticeships.Application.Queries.GetPledges;

namespace SFA.DAS.ManageApprenticeships.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class PledgesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PledgesController> _logger;

        public PledgesController(IMediator mediator, ILogger<PledgesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetPledges([Required]long accountId)
        {
            try
            {
                var response = await _mediator.Send(new GetPledgesQuery()
                {
                    AccountId = accountId,
                });

                var model = (GetPledgesResponse)response;

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting pledges");
                return BadRequest();
            }
        }
    }
}