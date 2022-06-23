using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ManageApprenticeships.Api.Models;
using SFA.DAS.ManageApprenticeships.Application.Queries.GetProvider;
using SFA.DAS.ManageApprenticeships.Application.Queries.GetProviders;

namespace SFA.DAS.ManageApprenticeships.Api.Controllers
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
                    Providers = response.Providers.Select(c=>(GetProviderResponse)c)
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
        [Route("{id}")]
        public async Task<IActionResult> GetProvider(int id)
        {
            try
            {
                var response = await _mediator.Send(new GetProviderQuery
                {
                    Id = id
                });
                var model = (GetProviderResponse) response.Provider;
                
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting provider {id}");
                return BadRequest();
            }
        }
    }
}