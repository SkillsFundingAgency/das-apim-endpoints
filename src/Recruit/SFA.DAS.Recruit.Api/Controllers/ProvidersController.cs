using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetDashboardByUkprn;
using SFA.DAS.Recruit.Application.Queries.GetProvider;
using SFA.DAS.Recruit.Application.Queries.GetProviders;
using SFA.DAS.Recruit.InnerApi.Requests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProvidersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProvidersController> _logger;

        public ProvidersController (IMediator mediator, ILogger<ProvidersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllProviders()
        {
            try
            {
                var response = await _mediator.Send(new GetProvidersQuery());
                var model = new GetProvidersListResponse
                {
                    Providers = response.Providers.Select(c => (GetProviderResponse)c)
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
        [Route("{ukprn:int}")]
        public async Task<IActionResult> GetProvider(int ukprn)
        {
            try
            {
                var response = await _mediator.Send(new GetProviderQuery { UKprn = ukprn });

                if (response?.Provider == null)
                    return NotFound();

                return Ok((GetProviderResponse)response.Provider);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting provider information");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{ukprn:int}/dashboard")]
        public async Task<IActionResult> GetDashboard([FromRoute] int ukprn, [FromQuery] ApplicationStatus status)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetDashboardByUkprnQuery(ukprn, status));

                return Ok(queryResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting employer dashboard stats");
                return BadRequest();
            }
        }
    }
}
