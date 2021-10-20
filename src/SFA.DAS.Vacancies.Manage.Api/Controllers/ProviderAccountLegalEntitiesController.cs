using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.Application.Providers.Queries.GetProviderAccountLegalEntities;

namespace SFA.DAS.Vacancies.Manage.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProviderAccountLegalEntitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderAccountLegalEntitiesController> _logger;

        public ProviderAccountLegalEntitiesController (IMediator mediator, ILogger<ProviderAccountLegalEntitiesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpGet]
        [Route("{ukprn}")]
        public async Task<IActionResult> GetList([FromRoute]int ukprn)
        {
            try
            {
                var queryResponse = await _mediator.Send(new GetProviderAccountLegalEntitiesQuery
                {
                    Ukprn = ukprn
                });

                return Ok((GetProviderAccountLegalEntitiesListResponse) queryResponse);

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get account legal entities for provider [{ukprn}]");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}