﻿using System.Net;
using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Reservations.Application.Rules.Queries.GetAvailableDates;
using SFA.DAS.Reservations.Api.Models;

namespace SFA.DAS.Reservations.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class RulesController(IMediator mediator, ILogger<RulesController> logger) : ControllerBase
    {
        [HttpGet]
        [Route("available-dates/{accountLegalEntityId}")]
        public async Task<IActionResult> GetAvailableDates(long accountLegalEntityId = 0)
        {
            try
            {
                var result = await mediator.Send(new GetAvailableDatesQuery
                {
                    AccountLegalEntityId = accountLegalEntityId
                });

                return Ok((GetAvailableDatesApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error calling GetAvailableDates");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
