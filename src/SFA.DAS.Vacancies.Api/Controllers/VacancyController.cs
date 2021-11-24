using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;


namespace SFA.DAS.Vacancies.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class VacancyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<VacancyController> _logger;



        public VacancyController(IMediator mediator, ILogger<VacancyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetVacancies([FromHeader(Name = "x-request-context-subscription-name")] string accountIdentifier, int pageNumber = 1, int pageSize = 10, string accountLegalEntityPublicHashedId = null, int? ukprn = null)
        {
            try
            {
                var account = new AccountIdentifier(accountIdentifier);

                var queryResponse = await _mediator.Send(new GetVacanciesQuery
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Ukprn = account.Ukprn ?? ukprn,
                    AccountPublicHashedId = account.AccountPublicHashedId,
                    AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId
                });

                return Ok((GetVacanciesListResponse)queryResponse);

            }
            catch (SecurityException e)
            {
                _logger.LogInformation($"Unable to get vacancies - {accountLegalEntityPublicHashedId} is not associated with subscription {accountIdentifier}.");
                return new StatusCodeResult((int) HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get vacancy");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}