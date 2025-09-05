using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries.GetVacancies;
using SFA.DAS.Vacancies.Application.Vacancies.Queries.GetVacancy;
using SFA.DAS.Vacancies.Enums;
using SFA.DAS.Vacancies.Services;
using System;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading.Tasks;

namespace SFA.DAS.Vacancies.Api.Controllers.v2;

[ApiController]
[ApiVersion("2")]
public class VacancyController(IMediator mediator, ILogger<VacancyController> logger, IMetrics metrics)
    : ControllerBase
{
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
    /// /vacancy?PageNumber=5&amp;PageSize=10&amp;Routes=example&amp;NationWideOnly=true&amp;PostedInLastNumberOfDays=30
    /// ```
    /// </remarks>
    /// <param name="accountIdentifier"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("/vacancy")]
    [ProducesResponseType(typeof(GetVacanciesListResponseV2), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetVacancies(
        [FromHeader(Name = "x-request-context-subscription-name")] string accountIdentifier,
        [FromQuery] SearchVacancyRequestV2 request)
    {
        try
        {
            var account = new AccountIdentifier(accountIdentifier);
            var queryResponse = await mediator.Send(new GetVacanciesQuery
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Ukprn = account.Ukprn != null && request.FilterBySubscription.HasValue && request.FilterBySubscription.Value ? account.Ukprn.Value : request.Ukprn,
                AccountPublicHashedId = request.FilterBySubscription.HasValue && request.FilterBySubscription.Value ? account.AccountHashedId : null,
                AccountLegalEntityPublicHashedId = request.AccountLegalEntityPublicHashedId,
                EmployerName = request.EmployerName,
                AccountIdentifier = account,
                Lat = request.Lat,
                Lon = request.Lon,
                Routes = request.Routes,
                Sort = request.Sort?.ToString(),
                DistanceInMiles = request.DistanceInMiles,
                NationWideOnly = request.NationWideOnly,
                StandardLarsCode = request.StandardLarsCode,
                PostedInLastNumberOfDays = request.PostedInLastNumberOfDays,
                AdditionalDataSources = request.AdditionalDataSources?.Select(x => x.ToString()).ToList(),
            });

            return Ok((GetVacanciesListResponseV2)queryResponse);

        }
        catch (SecurityException)
        {
            logger.LogInformation("Unable to get vacancies - {AccountLegalEntityPublicHashedId} is not associated with subscription {AccountIdentifier}.", request.AccountLegalEntityPublicHashedId, accountIdentifier);
            return new StatusCodeResult((int)HttpStatusCode.Forbidden);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error attempting to get vacancies");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    /// GET vacancy by reference number
    /// </summary>
    /// <remarks>Returns details of a specific vacancy. If no vacancy found then a 404 response is returned.</remarks>
    /// <param name="vacancyReference">Vacancy reference in the following format 10001122</param>
    /// <returns></returns>
    [HttpGet]
    [Route("/vacancy/{vacancyReference}")]
    [ProducesResponseType(typeof(GetVacancyResponseV2), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetVacancy([FromRoute] string vacancyReference)
    {
        var includeInMetrics = false;
        try
        {
            var result = await mediator.Send(new GetVacancyQuery
            {
                VacancyReference = vacancyReference
            });

            var response = (GetVacancyResponseV2)result;
            if (response == null)
            {
                return NotFound();
            }
            includeInMetrics = result.Vacancy.VacancySource == DataSource.Raa;
            return Ok(response);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error attempting to get vacancy");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
        finally
        {
            if (includeInMetrics) metrics.IncreaseVacancyViews(vacancyReference.TrimVacancyReference());
        }
    }
}