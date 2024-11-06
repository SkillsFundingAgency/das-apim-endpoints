using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using System.Net;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SavedSearchesController(
        IMediator mediator,
        ILogger<SavedSearchesController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] DateTime lastRunDateTime, 
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int maxApprenticeshipSearchResultCount = 5,
            [FromQuery] VacancySort sortOrder = VacancySort.AgeDesc, 
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Get Saved Searches invoked");

            try
            {
                var result = await mediator.Send(new GetSavedSearchesQuery(lastRunDateTime,
                        pageNumber,
                        pageSize,
                        maxApprenticeshipSearchResultCount,
                        sortOrder),
                    cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error invoking Get Saved Searches");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("sendNotification")]
        public async Task<IActionResult> SendNotification(
            [FromBody] SavedSearchApiRequest savedSearchApiRequest,
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Post send saved search notification invoked");

            try
            {
                await mediator.Send(new PostSendSavedSearchNotificationCommand
                {
                    Id = savedSearchApiRequest.Id,
                    Categories = savedSearchApiRequest.Categories?.Select(category => new PostSendSavedSearchNotificationCommand.Category
                    {
                        Id = category.Id,
                        Name = category.Name
                    }).ToList(),
                    Levels = savedSearchApiRequest.Levels?.Select(level => new PostSendSavedSearchNotificationCommand.Level
                    {
                        Code = level.Code,
                        Name = level.Name
                    }).ToList(),
                    Distance = savedSearchApiRequest.Distance,
                    DisabilityConfident = savedSearchApiRequest.DisabilityConfident,
                    Location = savedSearchApiRequest.Location,
                    SearchTerm = savedSearchApiRequest.SearchTerm,
                    UnSubscribeToken = savedSearchApiRequest.UnSubscribeToken,
                    User = new PostSendSavedSearchNotificationCommand.UserDetails
                    {
                        Id = savedSearchApiRequest.User.Id,
                        Email = savedSearchApiRequest.User.Email,
                        FirstName = savedSearchApiRequest.User.FirstName,
                        MiddleNames = savedSearchApiRequest.User.MiddleNames,
                        LastName = savedSearchApiRequest.User.LastName
                    },
                    Vacancies = savedSearchApiRequest.Vacancies.Select(vacancy => new PostSendSavedSearchNotificationCommand.Vacancy
                    {
                        Id = vacancy.Id,
                        ClosingDate = vacancy.ClosingDate,
                        Distance = vacancy.Distance,
                        EmployerName = vacancy.EmployerName,
                        Title = vacancy.Title,
                        TrainingCourse = vacancy.TrainingCourse,
                        VacancyReference = vacancy.VacancyReference,
                        Wage = vacancy.Wage,
                        Address = new PostSendSavedSearchNotificationCommand.Address
                        {
                            AddressLine1 = vacancy.Address.AddressLine1,
                            AddressLine2 = vacancy.Address.AddressLine2,
                            AddressLine3 = vacancy.Address.AddressLine3,
                            AddressLine4 = vacancy.Address.AddressLine4,
                            Postcode = vacancy.Address.Postcode,
                        }
                    }).ToList()
                }, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error invoking Post Send saved Search notification");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
