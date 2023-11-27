using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Api.Models.Reservations;
using SFA.DAS.EmployerAccounts.Application.Queries.GetReservations;

namespace SFA.DAS.EmployerAccounts.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly IMediator _mediator;

        public ReservationController(IMediator mediator, ILogger<ReservationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{accountId}")]
        public async Task<IActionResult> GetReservations(long accountId)
        {
            try
            {
                var response = await _mediator.Send(new GetReservationsQuery
                {
                    AccountId = accountId
                });

                var model = new GetReservationsResponse
                {
                    Reservations = response.Reservations.Select(c => (ReservationsResponse)c)
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting reservations");
                return BadRequest();
            }
        }
    }
}