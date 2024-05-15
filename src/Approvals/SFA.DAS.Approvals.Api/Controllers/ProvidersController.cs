using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.Providers.Queries;
using SFA.DAS.Approvals.Application.DeliveryModels.Queries;
using SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command;
using SFA.DAS.Approvals.Application.ProviderUsers.Commands;
using SFA.DAS.Approvals.Application.ProviderUsers.Queries;
using Newtonsoft.Json.Linq;
using SFA.DAS.Approvals.InnerApi.Requests;

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
        [Route("{ukprn}/users")]
        public async Task<IActionResult> GetUsers(long ukprn)
        {
            try
            {
                var response = await _mediator.Send(new GetProviderUsersQuery
                {
                    Ukprn = ukprn
                });

                var model = new GetProvidersUsersResponse
                {
                    Users = response.Users
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting all provider users");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{providerId}/courses/{trainingCode}")]
        [Obsolete("Will be replaced by /{providerId}/courses?trainingCode={trainingCode}")]
        public async Task<IActionResult> GetProviderCoursesDeliveryModel(long providerId, string trainingCode = "", [FromQuery] long accountLegalEntityId = 0)
        {
            try
            {
                var result = await _mediator.Send(new GetDeliveryModelsQuery(providerId, trainingCode, accountLegalEntityId));
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Provider Courses Delivery Models for Provider {providerId} and course {trainingCode}", providerId, trainingCode);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            }
        }

        [HttpGet]
        [Route("{providerId}/courses")]
        public async Task<IActionResult> GetProviderCoursesDeliveryModelByQuery(long providerId, [FromQuery] string trainingCode, [FromQuery] long accountLegalEntityId = 0)
        {
            try
            {
                var result = await _mediator.Send(new GetDeliveryModelsQuery(providerId, trainingCode, accountLegalEntityId));
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Provider Courses Delivery Models for Provider {providerId} and course {trainingCode}", providerId, trainingCode);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("{ukprn}/emails")]
        public async Task<IActionResult> EmailUsers(long ukprn, [FromBody] ProviderEmailRequest request)
        {
            try
            {
                await _mediator.Send(new ProviderEmailCommand
                {
                    ProviderId = ukprn,
                    ProviderEmailRequest = request
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error sending provider emails");
                return BadRequest();
            }
        }
    }
}