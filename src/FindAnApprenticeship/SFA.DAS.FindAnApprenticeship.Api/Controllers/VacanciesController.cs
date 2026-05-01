using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Api.Models.Vacancies;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;
using SFA.DAS.FindAnApprenticeship.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.Extensions;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VacanciesController(IMediator mediator, ILogger<VacanciesController> logger, IMetrics metrics)
        : Controller
    {
        [HttpGet]
        [Route("{vacancyReference}")]
        [ProducesResponseType(typeof(GetApprenticeshipVacancyApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchByVacancyReference([FromRoute] string vacancyReference, [FromQuery] Guid? candidateId = null)
        {
            var includeInMetrics = false;
            try
            {
                var result = await mediator.Send(new GetApprenticeshipVacancyQuery
                { VacancyReference = vacancyReference, CandidateId = candidateId });
                if (result == null) return new StatusCodeResult((int)HttpStatusCode.NotFound);
                includeInMetrics = result.ApprenticeshipVacancy.VacancySource == VacancyDataSource.Raa;
                return Ok((GetApprenticeshipVacancyApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting vacancy details by reference:{vacancyReference}", vacancyReference);
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
            finally
            {
                if(includeInMetrics) metrics.IncreaseVacancyViews(vacancyReference);
            }
        }

        [HttpPost]
        [Route("{vacancyReference}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Apply([FromRoute] string vacancyReference, [FromBody] PostApplyApiRequest request)
        {
            try
            {
                var result = await mediator.Send(new ApplyCommand
                { CandidateId = request.CandidateId, VacancyReference = vacancyReference });

                if (result == null) return new StatusCodeResult((int)HttpStatusCode.NotFound);

                // increase the count of vacancy started counter metrics.
                metrics.IncreaseVacancyStarted(vacancyReference);
                
                return Created(result.ApplicationId.ToString(),(PostApplyApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error posting vacancy details by reference:{vacancyReference}", vacancyReference);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("statistics")]
        [ProducesResponseType(typeof(GetSearchIndexStatisticsResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSearchIndexStatistics(
            [FromServices] ICacheStorageService cacheStorageService,
            [FromServices] IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient)
        {
            // check the cache
            var stats = await cacheStorageService.RetrieveFromCache<GetSearchIndexStatisticsResponse>(nameof(GetSearchIndexStatisticsResponse));
            if (stats != null)
            {
                return Ok(stats);
            }

            // get the data
            var response = await findApprenticeshipApiClient.Get<GetSearchIndexStatisticsResponse>(new GetSearchIndexStatisticsRequest());
            
            // we're going to expire the cache at 5mins past the hour so that the indexer has time to run
            var cacheLifetime = DateTime.UtcNow.TimeUntilMinutesPastHour(5);
            
            await cacheStorageService.SaveToCache(nameof(GetSearchIndexStatisticsResponse), response, cacheLifetime);
            return Ok(response);
        }
    }
}
