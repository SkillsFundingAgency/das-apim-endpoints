﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.Providers.Queries;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProvidersController : ControllerBase
    {
        private readonly ILogger<ProvidersController> _logger;
        private readonly IMediator _mediator;

        public ProvidersController (ILogger<ProvidersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _mediator.Send(new GetProvidersQuery());
                
                var model = new GetProvidersListResponse
                {
                    Providers = result.Providers.Select(c=>(GetProvidersResponse)c)
                };
                return Ok(model);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting all providers");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{providerId}/courses/{trainingCode}")]
        public async Task<IActionResult> GetProviderCoursesDeliveryModel(long providerId, string trainingCode)
        {
            try
            {
                var result = await _mediator.Send(new GetProviderCoursesDeliveryModelQuery(providerId, trainingCode));
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Provider Courses Delivery Models for Provider {providerId} and course {trainingCode}", providerId, trainingCode);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            }
        }
    }
}
