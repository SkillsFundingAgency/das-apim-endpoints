using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetVacancies(int pageNumber = 1, int pageSize = 10, string accountLegalEntityPublicHashedId = null, int? ukprn = null, string accountPublicHashedId = null)
        {
            try
            {
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