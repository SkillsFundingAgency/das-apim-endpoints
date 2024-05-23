using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Reservations.Application.AccountProviderLegalEntities.Queries;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.Reservations.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountProviderLegalEntitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountProviderLegalEntitiesController> _logger;

        public AccountProviderLegalEntitiesController(IMediator mediator, ILogger<AccountProviderLegalEntitiesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get([FromQuery] int? ukprn, [FromQuery] List<Operation> operations)
        {
            try
            {
                var result = await _mediator.Send(new GetAccountProviderLegalEntitiesQuery { Ukprn = ukprn, Operations = operations });

                if (result?.AccountProviderLegalEntities == null)
                    return NotFound();

                return Ok(result.AccountProviderLegalEntities);
            }
            catch (Exception e)
            {
                var loggerData = ukprn.ToString().Replace('\n', '_').Replace('\r', '_');
                _logger.LogError(e, $"Error attempting to get AccountProviderLegalEntities for UKPRN: {loggerData}");
                return BadRequest();
            }
        }
    }
}
