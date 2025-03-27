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
    public class AccountProviderLegalEntitiesController(
        IMediator mediator,
        ILogger<AccountProviderLegalEntitiesController> logger)
        : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get([FromQuery] int? ukprn, [FromQuery] List<Operation> operations)
        {
            try
            {
                var result = await mediator.Send(new GetAccountProviderLegalEntitiesQuery { Ukprn = ukprn, Operations = operations });

                if (result?.AccountProviderLegalEntities == null)
                    return NotFound();

                return Ok(result.AccountProviderLegalEntities);
            }
            catch (Exception e)
            {
                var loggerData = ukprn.ToString().Replace('\n', '_').Replace('\r', '_');
                logger.LogError(e, $"Error attempting to get AccountProviderLegalEntities for UKPRN: {loggerData}");
                return BadRequest();
            }
        }
    }
}
