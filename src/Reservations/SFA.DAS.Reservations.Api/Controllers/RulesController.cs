using System.Net;
using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Reservations.Application.Rules.Queries.GetAvailableDates;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.Reservations.Api.Models;

namespace SFA.DAS.Reservations.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class RulesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RulesController> _logger;

        public RulesController(IMediator mediator, ILogger<RulesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("available-dates/{accountLegalEntityId}")]
        public async Task<IActionResult> GetAvailableDates(long accountLegalEntityId = 0)
        {
            try
            {
                var result = await _mediator.Send(new GetAvailableDatesQuery
                {
                    AccountLegalEntityId = accountLegalEntityId
                });

                return Ok((GetAvailableDatesApiResponse)result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

    }

}
