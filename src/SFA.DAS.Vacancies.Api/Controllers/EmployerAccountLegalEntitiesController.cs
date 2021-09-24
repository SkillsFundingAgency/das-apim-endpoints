using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer;

namespace SFA.DAS.Vacancies.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class EmployerAccountLegalEntitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployerAccountLegalEntitiesController> _logger;

        public EmployerAccountLegalEntitiesController(IMediator mediator, ILogger<EmployerAccountLegalEntitiesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("{encodedAccountId}")]
        public async Task<IActionResult> GetList([FromRoute]string encodedAccountId)
        {
            try
            {
                var queryResponse = await _mediator.Send(new GetLegalEntitiesForEmployerQuery
                {
                    EncodedAccountId = encodedAccountId
                });

                return Ok((GetEmployerAccountLegalEntitiesListResponse) queryResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get account legal entities for employer [{encodedAccountId}]");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}