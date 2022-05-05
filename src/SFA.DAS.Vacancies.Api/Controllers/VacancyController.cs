using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using Microsoft.OpenApi.Extensions;
using SFA.DAS.SharedOuterApi.Extensions;
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

        /// <summary>
        /// GET list of vacancies
        /// </summary>
        /// <remarks>
        /// ### Returns list of Vacancies based on your subscription. ###
        /// - If `FilterBySubscription` is `true` then for employer subscriptions this will automatically filter by your account.
        /// - If `FilterBySubscription` is `true` then for providers it will automatically filter by UKPRN.
        /// - If you provide a `AccountLegalEntityPublicHashedId` it must come from `GET accountslegalentities` or a forbidden result will be returned.
        /// ### Examples ###
        /// Get all of a subscription's vacancies sorted by age descending (oldest first):
        /// ```
        /// /vacancy?Sort=AgeDesc&amp;FilterBySubscription=true
        /// ```
        /// Get all vacancies within a 20 mile radius of Coventry (52.408056, -1.510556), sorted by distance (closest first) for standards 123 and 345:
        /// ```
        /// /vacancy?Lat=52.408056&amp;Lon=-1.510556&amp;Sort=DistanceAsc&amp;DistanceInMiles=20&amp;StandardLarsCode=123&amp;StandardLarsCode=345
        /// ```
        /// Get all nationwide vacancies for route 'example' posted within the last 30 days, page 5, size 10:
        /// ```
        /// /vacancy?PageNumber=5&PageSize=10&Routes=example&NationWideOnly=true&PostedInLastNumberOfDays=30
        /// ```
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(GetVacanciesListResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> GetVacancies([FromHeader(Name = "x-request-context-subscription-name")] string accountIdentifier, [FromQuery]SearchVacancyRequest request)
        {
            try
            {
                var account = new AccountIdentifier(accountIdentifier);

                var queryResponse = await _mediator.Send(new GetVacanciesQuery
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    Ukprn = account.Ukprn != null && request.FilterBySubscription.HasValue && request.FilterBySubscription.Value ? account.Ukprn.Value : request.Ukprn,
                    AccountPublicHashedId =  request.FilterBySubscription.HasValue && request.FilterBySubscription.Value ? account.AccountHashedId : null,
                    AccountLegalEntityPublicHashedId = request.AccountLegalEntityPublicHashedId,
                    AccountIdentifier = account,
                    Lat = request.Lat,
                    Lon = request.Lon,
                    Routes = request.Routes,
                    Sort = request.Sort?.ToString(),
                    DistanceInMiles = request.DistanceInMiles,
                    NationWideOnly = request.NationWideOnly,
                    StandardLarsCode = request.StandardLarsCode,
                    PostedInLastNumberOfDays = request.PostedInLastNumberOfDays
                });

                return Ok((GetVacanciesListResponse)queryResponse);

            }
            catch (SecurityException e)
            {
                _logger.LogInformation($"Unable to get vacancies - {request.AccountLegalEntityPublicHashedId} is not associated with subscription {accountIdentifier}.");
                return new StatusCodeResult((int) HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get vacancies");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// GET vacancy by reference number
        /// </summary>
        /// <remarks>Returns details of a specific vacancy. If no vacancy found then a 404 response is returned.</remarks>
        /// <param name="vacancyReference">Vacancy reference in the following format 10001122</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{vacancyReference}")]
        [ProducesResponseType(typeof(GetVacancyResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetVacancy([FromRoute] string vacancyReference)
        {
            try
            {
                var result = await _mediator.Send(new GetVacancyQuery
                {
                    VacancyReference = vacancyReference
                });

                var response = (GetVacancyResponse)result;
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get vacancy");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
        
    }
}