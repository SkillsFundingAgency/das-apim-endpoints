﻿using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.Application.Providers.Queries.GetProvider;

namespace SFA.DAS.Reservations.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProvidersController(
        ILogger<TrainingCoursesController> logger,
        IMediator mediator)
        : ControllerBase
    {
        [HttpGet]
        [Route("{ukprn}")]
        public async Task<IActionResult> GetProvider(int ukprn)
        {
            try
            {
                var queryResult = await mediator.Send(new GetProviderQuery{Ukprn = ukprn});

                var model = (GetProviderResponse) queryResult.Provider;
                
                if (model == null)
                {
                    return NotFound();
                }

                return Ok(model);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error attempting to get training provider, UKPRN: [{ukprn}]");
                return BadRequest();
            }
        }
    }
}