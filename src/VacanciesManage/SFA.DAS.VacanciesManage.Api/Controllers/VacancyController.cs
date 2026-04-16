using System;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.VacanciesManage.Api.Models;
using SFA.DAS.VacanciesManage.Application.Recruit.Commands.CreateVacancy;
using SFA.DAS.VacanciesManage.InnerApi.Requests;

namespace SFA.DAS.VacanciesManage.Api.Controllers
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

        /// <summary>
        /// POST apprenticeship vacancy
        /// </summary>
        /// <remarks>Creates an apprenticeship vacancy using the specified values</remarks>
        /// <param name="id">The unique ID of the Apprenticeship advert.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}")]
        [ProducesResponseType(typeof(CreateVacancyResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(CreateVacancyExampleForbiddenResponse), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(CreateVacancyExampleBadRequestResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateVacancy(
            [FromHeader(Name = "x-request-context-subscription-name")] string accountIdentifier, 
            [FromRoute]Guid id, 
            [FromBody]CreateVacancyRequest request, 
            [FromHeader(Name = "x-request-context-subscription-is-sandbox")] bool? isSandbox = false)
        {
            try
            {
                var account = new AccountIdentifier(accountIdentifier);

                if (isSandbox.HasValue && isSandbox.Value)
                {
                    if (id == Guid.Empty)
                        return new BadRequestObjectResult(new {errors = new[]{"Unable to create Vacancy. Vacancy already submitted"}});
                    if (id == Guid.Parse("11111111-1111-1111-1111-111111111111"))
                        return new StatusCodeResult((int) HttpStatusCode.TooManyRequests);
                }
                
                switch (account.AccountType)
                {
                    case AccountType.Unknown:
                        return new StatusCodeResult((int) HttpStatusCode.Forbidden);
                    case AccountType.Provider when account.Ukprn == null:
                        return new BadRequestObjectResult("Account Identifier is not in the correct format.");
                }

                var postVacancyRequestData = (PostVacancyRequestData)request;
                var postVacancyV2RequestData = (PostVacancyV2RequestData)request;
                postVacancyRequestData.OwnerType = (OwnerType)account.AccountType;
                postVacancyRequestData.AccountType = account.AccountType;
                var contactDetails = new ContactDetails
                {
                    Email = request.SubmitterContactDetails.Email,
                    Name = request.SubmitterContactDetails.Name,
                    Phone = request.SubmitterContactDetails.Phone,
                };
                
                switch (account.AccountType)
                {
                    case AccountType.Provider:
                        postVacancyRequestData.User.Ukprn = account.Ukprn.Value;
                        postVacancyRequestData.ProviderContact = contactDetails;
                        break;
                    case AccountType.Employer:
                        postVacancyRequestData.EmployerAccountId = account.AccountHashedId;
                        postVacancyRequestData.EmployerContact = contactDetails;
                        break;
                }
                
                postVacancyV2RequestData.Contact = new PostVacancyV2Contact
                {
                    Email = request.SubmitterContactDetails.Email,
                    Name = request.SubmitterContactDetails.Name,
                    Phone = request.SubmitterContactDetails.Phone,
                };

                var response = await _mediator.Send(new CreateVacancyCommand
                {
                    Id = id,
                    AccountIdentifier = account,
                    PostVacancyRequestData = postVacancyRequestData,
                    PostVacancyV2RequestData = postVacancyV2RequestData,
                    IsSandbox = isSandbox ?? false
                });

                return new CreatedResult("", new CreateVacancyResponse { VacancyReference = response.VacancyReference });
            }
            catch (HttpRequestContentException e)
            {
                var content = ReverseMapFieldNamesForErrors(request, e.ErrorContent);
                return StatusCode((int) e.StatusCode, content);
            }
            catch (SecurityException)
            {
                return new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating vacancy");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        private static string ReverseMapFieldNamesForErrors(CreateVacancyRequest request, string content)
        {
            var result = content
                .Replace("ProgrammeId", "standardLarsCode", StringComparison.CurrentCultureIgnoreCase)
                .Replace(@"EmployerName""", @"alternativeEmployerName""", StringComparison.CurrentCultureIgnoreCase)
                .Replace(".Country", ".postcode", StringComparison.CurrentCultureIgnoreCase)
                .Replace("Country must be England", "Postcode must be in England", StringComparison.CurrentCultureIgnoreCase)
                .Replace("EmployerLocationInformation", "recruitingNationallyDetails", StringComparison.CurrentCultureIgnoreCase);
            
            if (request.Address is not null)
            {
                return result
                    .Replace("Addresses[0]", "address", StringComparison.CurrentCultureIgnoreCase)
                    .Replace("Address", "address", StringComparison.CurrentCultureIgnoreCase);
            }
            
            return result
                .Replace("Addresses", "multipleAddresses", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}