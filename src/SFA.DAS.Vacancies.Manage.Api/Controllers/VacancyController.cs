using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Commands.CreateVacancy;

namespace SFA.DAS.Vacancies.Manage.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class VacancyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<VacancyController> _logger;

        public VacancyController (IMediator mediator, ILogger<VacancyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> CreateVacancy([FromHeader(Name = "x-request-context-subscription-name")] string accountIdentifier, [FromRoute]Guid id, [FromBody]CreateVacancyRequest request)
        {
            try
            {
                var account = new AccountIdentifier(accountIdentifier);

                if (account.IsSandbox)
                {
                    if (id == Guid.Empty)
                        return new StatusCodeResult((int) HttpStatusCode.AlreadyReported);
                    if (id == Guid.Parse("11111111-1111-1111-1111-111111111111"))
                        return new StatusCodeResult((int) HttpStatusCode.TooManyRequests);
                }
                
                switch (account.AccountType)
                {
                    case AccountType.Unknown:
                        return new StatusCodeResult((int) HttpStatusCode.Forbidden);
                    case AccountType.Provider when account.Ukprn == null:
                        return new BadRequestObjectResult("Account Identifier is not in the correct format.");
                    case AccountType.Provider:
                        request.User = new VacancyUser
                        {
                            Ukprn = account.Ukprn.Value
                        };
                        break;
                    case AccountType.Employer:
                        request.EmployerAccountId = account.AccountPublicHashedId;
                        break;
                }

                var response = await _mediator.Send(new CreateVacancyCommand
                {
                    Id = id,
                    PostVacancyRequestData = request
                });

                return new CreatedResult("", new {response.VacancyReference});
            }
            catch (HttpRequestContentException e)
            {
                return StatusCode((int) e.StatusCode, e.ErrorContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating vacancy");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}