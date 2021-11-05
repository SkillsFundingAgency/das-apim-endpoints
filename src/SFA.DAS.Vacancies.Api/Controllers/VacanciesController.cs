using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;


namespace SFA.DAS.Vacancies.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class VacanciesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<VacanciesController> _logger;



        public VacanciesController(IMediator mediator, ILogger<VacanciesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("vacancies")]
        public async Task<IActionResult> GetVacancies([FromHeader(Name = "x-request-context-subscription-name")] string accountIdentifier, int pageNumber = 1, int pageSize = 10, string accountLegalEntityPublicHashedId = null, int? ukprn = null, string accountPublicHashedId = null)
        {
            try
            {
                var account = new AccountIdentifier(accountIdentifier);
                switch (account.AccountType)
                { 
                    case AccountType.Employer:
                        accountPublicHashedId = account.AccountPublicHashedId;
                        //check legalEntityPublishHashedID  matches account before setting value
                        break;
                    case AccountType.Provider:
                        //then the accountLegalEntityPublicHashedId can be set but they must have permission to act on its behalf.
                        break;
                    case AccountType.Unknown:
                        accountLegalEntityPublicHashedId = null;
                        accountPublicHashedId = null;
                        break;
                }

                var queryResponse = await _mediator.Send(new GetVacanciesQuery
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Ukprn = ukprn,
                    AccountPublicHashedId = accountPublicHashedId,
                    AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId
                });

                return Ok((GetVacanciesListResponse)queryResponse);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get vacancy");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}